using AutoMapper;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Application.DTOs;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<SaleDto, Sale>();
        CreateMap<SaleItemDto, SaleItem>();
    }
}
