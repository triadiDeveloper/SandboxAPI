using Application.DTOs.Organization;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Organization;
using Microsoft.AspNetCore.Mvc;

[Route("odata/Companies")]
public class CompaniesController : GenericODataController<Company, CompanyDto>
{
    public CompaniesController(IGenericRepository<Company> repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}