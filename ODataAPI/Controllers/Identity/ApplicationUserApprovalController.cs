using Application.DTOs.Identity;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Identity;

namespace WebAPI.Controllers.Identity
{
    public class ApplicationUserApprovalController : GenericODataController<ApplicationUserApproval, ApplicationUserApprovalDto>
    {
        public ApplicationUserApprovalController(IGenericRepository<ApplicationUserApproval> repository, IMapper mapper, string keyPropertyName = "Id") : base(repository, mapper, keyPropertyName)
        {
        }
    }
}
