using Application.DTOs.Demography;
using Application.DTOs.HumanResource;
using Application.DTOs.Identity;
using Application.DTOs.Organization;
using AutoMapper;
using Domain.Entities.Demography;
using Domain.Entities.HumanResource;
using Domain.Entities.Identity;
using Domain.Entities.Organization;

namespace Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Demography

            CreateMap<Country, CountryDto>().ReverseMap();

            #endregion

            #region HumanResource

            CreateMap<Employee, EmployeeDto>().ReverseMap();

            #endregion

            #region Identity

            CreateMap<ApplicationController, ApplicationControllerDto>().ReverseMap();
            CreateMap<ApplicationNavigation, ApplicationNavigationDto>().ReverseMap();
            CreateMap<ApplicationNavigationRole, ApplicationNavigationRoleDto>().ReverseMap();
            CreateMap<ApplicationRole, ApplicationRoleDto>().ReverseMap();
            CreateMap<ApplicationRoleClaim, ApplicationRoleClaimDto>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
            CreateMap<ApplicationUserApproval, ApplicationUserApprovalDto>().ReverseMap();
            CreateMap<ApplicationUserApprovalDetail, ApplicationUserApprovalDetailDto>().ReverseMap();
            CreateMap<ApplicationUserInfo, ApplicationUserInfoDto>().ReverseMap();
            CreateMap<ApplicationUserLogin, ApplicationUserLoginDto>().ReverseMap();

            #endregion

            #region Organization

            CreateMap<Company, CompanyDto>().ReverseMap();

            #endregion
        }
    }
}