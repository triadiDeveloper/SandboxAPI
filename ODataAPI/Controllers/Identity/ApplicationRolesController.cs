using Application.DTOs.Identity;
using Application.Exceptions;
using Domain.Entities.Identity;
using Infrastructure;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.ActionFilters;
using WebAPI.Extensions;

namespace WebAPI.Controllers.Identity
{
    public class ApplicationRolesController : ODataController
    {
        private readonly ILogger<ApplicationRolesController> _logger;
        private readonly CleanDBContext _dbContext;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ApplicationRolesController(CleanDBContext dbContext, RoleManager<ApplicationRole> roleManager, ILogger<ApplicationRolesController> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        // Get ~/Roles
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, MaxExpansionDepth = 5)]
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        public IActionResult Get()
        {
            return Ok(_roleManager.Roles.AsQueryable());
        }

        // GET ~/Roles(1)
        [EnableQuery]
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        public async Task<IActionResult> Get(string key)
        {
            var book = await _roleManager.FindByIdAsync(key);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        //Role By Id
        [HttpPost]
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        [Route($"{GlobalServiceRegister.RoutePrefix}/[controller]/ById")]
        public async Task<IActionResult> ById([FromODataUri] Guid Id)
        {
            var result = await _dbContext.Roles.AsNoTracking()
                .Where(x => x.Id == Id)
              //.Include(x => x.NavigationRoles).Include(x => x.Claims)
              .Select(x => new ApplicationRole
              {
                  Id = x.Id,
                  Name = x.Name,
                  Description = x.Description
              })
              .FirstOrDefaultAsync();

            if (result != null)
            {
                string QueryNavigationRole = $"select id,Applicationnavigationid,Applicationroleid from [identity].[Applicationnavigationrole] where Applicationroleid = '{result.Id}' order by ApplicationnavigationId";
                string QueryRoleClaim = $"select id,ApplicationControllerMethodId,Applicationroleid from [identity].[ApplicationRoleClaims] where Applicationroleid = '{result.Id}' order by ApplicationcontrollermethodId";

                var dtNavigation = await _dbContext.SelectData(QueryNavigationRole, false);
                var dtRoles = await _dbContext.SelectData(QueryRoleClaim, false);

                string JSONresult = JsonConvert.SerializeObject(dtNavigation);
                string JSONresultRole = JsonConvert.SerializeObject(dtRoles);

                var tempResult = JsonConvert.DeserializeObject<List<ApplicationNavigationRole>>(JSONresult);
                var tempResultRole = JsonConvert.DeserializeObject<List<ApplicationRoleClaim>>(JSONresultRole);

                if (tempResult != null && result.Id != null && tempResult.Select(x => x.ApplicationNavigationId).Any())
                {
                    result.ApplicationNavigationRoles = tempResult;
                    //join ids untuk in select query
                    var listNavigationId = string.Join(',', tempResult.Select(x => x.ApplicationNavigationId).ToList());
                    string QueryNavigation = $"select id,code,Name, case when controllerId is null then 0 else controllerId end as controllerId from [identity].[ApplicationNavigation] where id in ({listNavigationId})";
                    dtNavigation = await _dbContext.SelectData(QueryNavigation, false);
                    JSONresult = JsonConvert.SerializeObject(dtNavigation);

                    var tempNavigation = JsonConvert.DeserializeObject<List<ApplicationNavigation>>(JSONresult);

                    foreach (var itemNavigation in result.ApplicationNavigationRoles)
                    {
                        itemNavigation.ApplicationNavigation = tempNavigation?.Where(x => x.Id == itemNavigation.ApplicationNavigationId).FirstOrDefault();
                    }
                }

                if (tempResultRole != null && result != null && tempResultRole.Select(x => x.ApplicationControllerMethodId).Any())
                {
                    result.ApplicationRoleClaims = tempResultRole;

                    var listControllerMethodId = string.Join(',', tempResultRole.Select(x => x.ApplicationControllerMethodId).ToList());
                    QueryRoleClaim = $"select id,name, case when controllerid is null then 0 else controllerid end as controllerid from [identity].[ApplicationControllerMethod] where id in ({listControllerMethodId})";
                    dtRoles = await _dbContext.SelectData(QueryRoleClaim, false);
                    JSONresultRole = JsonConvert.SerializeObject(dtRoles);

                    var tempControllerMethod = JsonConvert.DeserializeObject<List<ApplicationControllerMethod>>(JSONresultRole);

                    foreach (var itemClaim in result.ApplicationRoleClaims)
                    {
                        itemClaim.ApplicationControllerMethod = tempControllerMethod?.Where(x => x.Id == itemClaim.ApplicationControllerMethodId).FirstOrDefault();
                    }
                }
            }

            return Ok(result);
        }

        // POST ~/Roles --> BODY JSON 
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        public async Task<IActionResult> Post([FromBody] ApplicationRoleDto fromBody)
        {
            // mapster
            var entity = fromBody.Adapt<ApplicationRole>();
            entity.NormalizedName = entity.Name;
            await _roleManager.CreateAsync(entity);


            return Created(entity);
        }

        // PUT ~/Roles(1) --> BODY JSON 
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        public async Task<IActionResult> Put(string key, [FromBody] ApplicationRoleDto fromBody)
        {
            ApplicationRole applicationRole = new ApplicationRole();
            //Check Apakah Insert atau Update, Masuk if kalo insert, masuk else kalo update
            var entity = await _dbContext.Roles.Where(s => s.Id == Guid.Parse(key)).Include(s => s.ApplicationRoleClaims).FirstOrDefaultAsync();
            var navRoles = await _dbContext.ApplicationNavigationRoles.Where(s => s.ApplicationRoleId == Guid.Parse(key)).ToListAsync();
            if (entity != null)
            {
                var param = fromBody?.Adapt<ApplicationRole>();

                #region Navigation Roles
                var detailExistNavigationRoleIds = navRoles.Select(s => s.ApplicationNavigationId).ToList();
                var updateNavigationRoleIds = param.ApplicationNavigationRoles.Where(s => detailExistNavigationRoleIds.Contains(s.ApplicationNavigationId)).ToList();
                var addNavigationRoleIds = param.ApplicationNavigationRoles.Where(s => !detailExistNavigationRoleIds.Contains(s.ApplicationNavigationId)).ToList();

                var navigationRoleIdsDto = param.ApplicationNavigationRoles.Select(s => s.ApplicationNavigationId).ToList();
                var deleteNavigationRoleIds = navRoles.Where(s => !navigationRoleIdsDto.Contains(s.ApplicationNavigationId)).ToList();

                //Fungsi Delete
                foreach (var item in deleteNavigationRoleIds)
                {
                    _dbContext.Remove(item);
                }

                //Fungsi Add
                foreach (var item in addNavigationRoleIds)
                {
                    await _dbContext.ApplicationNavigationRoles.AddAsync(item);
                }

                //Fungsi Update
                foreach (var item in updateNavigationRoleIds)
                {
                    _dbContext.ApplicationNavigationRoles.Update(item);
                }
                #endregion

                #region Role Claims
                var detailExistRoleClaimIds = entity.ApplicationRoleClaims.Select(s => s.ApplicationControllerMethodId).ToList();
                var addRoleClaimIds = param.ApplicationRoleClaims.Where(s => !detailExistRoleClaimIds.Contains(s.ApplicationControllerMethodId)).ToList();

                var roleClaimIdsDto = param.ApplicationRoleClaims.Select(s => s.ApplicationControllerMethodId).ToList();
                var deleteRoleClaimIds = entity.ApplicationRoleClaims.Where(s => !roleClaimIdsDto.Contains(s.ApplicationControllerMethodId)).ToList();

                //Fungsi Delete
                foreach (var item in deleteRoleClaimIds)
                {
                    _dbContext.Remove(item);
                }

                //Fungsi Add
                foreach (var item in addRoleClaimIds)
                {
                    await _dbContext.RoleClaims.AddAsync(item);
                }
                #endregion

                entity.NormalizedName = entity.Name;
                await _dbContext.SaveChangesAsync();
            }

            return Updated(entity);
        }

        // PATCH ~/Roles(1) --> BODY JSON (ONLY FIELD WAS CHANGED)
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        public async Task<IActionResult> Patch([FromBody] string key, [FromBody] Delta<ApplicationRole> fromBody)
        {
            var existing = await _roleManager.FindByIdAsync(key);
            if (existing == null)
            {
                return NotFound();
            }
            fromBody.Patch(existing);

            try
            {
                await _roleManager.UpdateAsync(existing);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.Clear();

                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("duplicate") == true)
                    {
                        ModelState.AddModelError("Code", "Code must be unique");
                    }
                }
                else
                {
                    ModelState.AddModelError(ex.HResult.ToString(), ex.Message);
                }

                return BadRequest(ModelState);
            }

            return Updated(existing);
        }

        // DELETE ~/Roles(1) --> BODY JSON (ONLY FIELD WAS CHANGED)
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            var entity = await _dbContext.Roles.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                var userRoles = await _dbContext.UserRoles.Where(s => s.RoleId == key).Select(s => s.RoleId).ToListAsync();
                if (userRoles.Any())
                    throw new NotFoundException("Proses gagal! Data telah digunakan pada user lain.");

                await _roleManager.DeleteAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.Clear();

                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("conflicted") && ex.InnerException.Message.Contains("UserRoles"))
                    {
                        throw new NotFoundException("Proses gagal! Data telah digunakan pada user lain.");
                    }
                    else if (ex.InnerException.Message.Contains("conflicted") && ex.InnerException.Message.Contains("NavigationRole"))
                    {
                        throw new NotFoundException("Proses gagal! Data telah digunakan pada role lain.");
                    }
                    else if (ex.InnerException.Message.Contains("conflicted") && ex.InnerException.Message.Contains("NavigationMobileRole"))
                    {
                        throw new NotFoundException("Proses gagal! Data telah digunakan pada role lain.");
                    }
                    else if (ex.InnerException.Message.Contains("conflicted") && ex.InnerException.Message.Contains("RoleClaims"))
                    {
                        throw new NotFoundException("Proses gagal! Data telah digunakan pada control role lain.");
                    }
                }
                else
                {
                    throw new NotFoundException("Proses gagal! " + ex.Message);
                }

                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
