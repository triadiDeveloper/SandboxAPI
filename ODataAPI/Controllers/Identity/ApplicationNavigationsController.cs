using Application.DTOs.Identity;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Identity;

namespace WebAPI.Controllers.Identity
{
    public class ApplicationNavigationsController : GenericODataController<ApplicationNavigation, ApplicationNavigationDto>
    {
        public ApplicationNavigationsController(IGenericRepository<ApplicationNavigation> repository, IMapper mapper, string keyPropertyName = "Id") : base(repository, mapper, keyPropertyName)
        {
        }
    }
}
