using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApi.ActionFilters;
using WebAPI.Extensions;

namespace WebAPI.Controllers.Identity
{
    public class ApplicationControllerMethodsController : ODataController
    {
        private readonly CleanDBContext _dbContext;

        public ApplicationControllerMethodsController(CleanDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [EnableQuery(PageSize = 50, AllowedQueryOptions = AllowedQueryOptions.All, MaxExpansionDepth = 5)]
        [ServiceFilter(typeof(DisableAuthorizeFilterAttribute))]
        public IActionResult Get()
        {
            return Ok(_dbContext.ApplicationControllerMethods.AsQueryable());
        }
    }
}
