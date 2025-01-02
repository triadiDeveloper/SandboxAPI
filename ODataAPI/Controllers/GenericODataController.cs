using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;

[Route($"{GlobalServiceRegister.RoutePrefix}/[controller]")]
public class GenericODataController<TEntity, TDto> : ODataController where TEntity : class where TDto : class
{
    private readonly IGenericRepository<TEntity> _repository;
    private readonly IMapper _mapper;
    private readonly string _keyPropertyName;

    public GenericODataController(IGenericRepository<TEntity> repository, IMapper mapper, string keyPropertyName = "Id")
    {
        _repository = repository;
        _mapper = mapper;
        _keyPropertyName = keyPropertyName;
    }

    //[EnableQuery(PageSize = 100, MaxExpansionDepth = 8)]
    //[ServiceFilter(typeof(AuthorizeFilterAttribute))]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var dtos = await _repository.GetAllAsync();
        return Ok(dtos);
    }

    //[EnableQuery]
    //[ServiceFilter(typeof(AuthorizeFilterAttribute))]
    [HttpGet("{key}")]
    public async Task<IActionResult> GetById([FromODataUri] int key)
    {
        var dto = await _repository.GetByIdAsync(key);
        return dto != null ? Ok(dto) : NotFound();
    }

    //[ServiceFilter(typeof(AuthorizeFilterAttribute))]
    //[ServiceFilter(typeof(ValidationFilterAttribute))]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TDto dto)
    {
        if (dto == null)
        {
            return BadRequest(new { message = "The request body cannot be null." });
        }

        var entity = _mapper.Map<TEntity>(dto);
        await _repository.AddAsync(entity);

        var resultDto = _mapper.Map<TDto>(entity);
        var keyProperty = resultDto.GetType().GetProperty(_keyPropertyName);
        if (keyProperty == null)
        {
            return StatusCode(500, new { message = "Unable to find key property on DTO." });
        }

        var keyValue = keyProperty.GetValue(resultDto);
        return CreatedAtAction(nameof(GetById), new { key = keyValue }, resultDto);
    }

    //[ServiceFilter(typeof(AuthorizeFilterAttribute))]
    //[ServiceFilter(typeof(ValidationFilterAttribute))]
    [HttpPut("{key}")]
    public async Task<IActionResult> Put(int key, [FromBody] TDto dto)
    {
        if (dto == null)
        {
            return BadRequest(new { message = "The request body cannot be null." });
        }

        var entity = _mapper.Map<TEntity>(dto);
        typeof(TEntity).GetProperty(_keyPropertyName)?.SetValue(entity, key);

        try
        {
            await _repository.UpdateAsync(entity);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Conflict(new { message = "The entity was modified or deleted by another process.", details = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }

    //[ServiceFilter(typeof(AuthorizeFilterAttribute))]
    [HttpDelete("{key}")]
    public async Task<IActionResult> Delete(int key)
    {
        var entity = await _repository.GetByIdAsync(key);
        if (entity == null)
        {
            return NotFound(new { message = "The entity was not found." });
        }

        await _repository.DeleteAsync(key);
        return NoContent();
    }
}