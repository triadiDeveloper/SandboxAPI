using Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.ActionFilters;

public class DisableAuthorizeFilterAttribute : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context) { }
}

public class AuthorizeFilterAttribute : IActionFilter
{
    private readonly CleanDBContext _DBContext;
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _jwtConfig;

    public AuthorizeFilterAttribute(CleanDBContext dbContext, IConfiguration configuration)
    {
        _DBContext = dbContext;
        _configuration = configuration;
        _jwtConfig = _configuration.GetSection("JwtSettings");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        throw new NotImplementedException();
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        throw new NotImplementedException();
    }
}
