using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebAPI.Controllers.Identity
{
    public class ApplicationUserRolesController : ODataController
    {
        private readonly ILogger<ApplicationRolesController> _logger;
        private readonly CleanDBContext _dbContext;

        public ApplicationUserRolesController(CleanDBContext dbContext, ILogger<ApplicationRolesController> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, MaxExpansionDepth = 5)]
        public IActionResult Get()
        {
            return Ok(_dbContext.ApplicationUserRoles);
        }
    }
}
