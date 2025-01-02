using Application.DTOs.Demography;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Demography;
using Microsoft.AspNetCore.Mvc;

[Route("odata/Countries")]
public class CountriesController : GenericODataController<Country, CountryDto>
{
    public CountriesController(IGenericRepository<Country> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}