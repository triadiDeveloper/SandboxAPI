using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApi.ActionFilters;

namespace WebAPI.Controllers.Identity
{
    public class ApplicationRoleClaimsController : ODataController
    {
        private readonly CleanDBContext _dbContext;

        public ApplicationRoleClaimsController(CleanDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get ~/ControllerMethods
        [EnableQuery(PageSize = 50, AllowedQueryOptions = AllowedQueryOptions.All, MaxExpansionDepth = 5)]
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        public IActionResult Get()
        {
            return Ok(_dbContext.RoleClaims.AsQueryable());
        }
    }
}
