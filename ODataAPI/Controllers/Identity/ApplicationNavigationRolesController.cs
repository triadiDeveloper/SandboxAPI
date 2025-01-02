﻿using Application.DTOs.Identity;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Identity;

namespace WebAPI.Controllers.Identity
{
    public class ApplicationNavigationRolesController : GenericODataController<ApplicationNavigationRole, ApplicationNavigationRoleDto>
    {
        public ApplicationNavigationRolesController(IGenericRepository<ApplicationNavigationRole> repository, IMapper mapper, string keyPropertyName = "Id") : base(repository, mapper, keyPropertyName)
        {
        }
    }
}