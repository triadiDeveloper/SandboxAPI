using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApi.ActionFilters;

namespace WebAPI.Controllers.Identity
{
    public class ApplicationControllersController : ODataController
    {
        private readonly ILogger<ApplicationControllersController> _logger;
        private readonly CleanDBContext _dbContext;

        public ApplicationControllersController(CleanDBContext dbContext, ILogger<ApplicationControllersController> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // Get ~/Controllers
        [EnableQuery(PageSize = 50, AllowedQueryOptions = AllowedQueryOptions.All, MaxExpansionDepth = 5)]
        [ServiceFilter(typeof(AuthorizeFilterAttribute))]
        public IActionResult Get()
        {
            return Ok(_dbContext.ApplicationControllers.AsQueryable());
        }
    }
}
