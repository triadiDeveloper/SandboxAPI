using Application.DTOs.HumanResource;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.HumanResource;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.HumanResource
{
    [Route("odata/Employees")]
    public class EmployeesController : GenericODataController<Employee, EmployeeDto>
    {
        public EmployeesController(IGenericRepository<Employee> repository, IMapper mapper, string keyPropertyName = "Id") : base(repository, mapper, keyPropertyName)
        {
        }
    }
}
