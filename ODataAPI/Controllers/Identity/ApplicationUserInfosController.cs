using Application.DTOs.Identity;
using Domain.Entities.Identity;
using Infrastructure;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers.Identity
{
    public class ApplicationUserInfosController : ODataController
    {
        protected readonly CleanDBContext _context;
        protected DbSet<ApplicationUserInfo> _dbset;

        public ApplicationUserInfosController(CleanDBContext context)
        {
            _context = context;
            _dbset = _context.Set<ApplicationUserInfo>();
        }

        public async Task<IActionResult> Post([FromBody] ApplicationUserInfoDto fromBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = fromBody.Adapt<ApplicationUserInfo>();
            entity.IpAddress = GetIP();

            await _dbset.AddAsync(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        public string GetIP()
        {
            string? ip = Request?.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString();

            if (!string.IsNullOrEmpty(ip))
            {
                return ip;
            }

            return string.Empty;
        }
    }
}
