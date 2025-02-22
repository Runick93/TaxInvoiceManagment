using AutoMapper;
using TaxInvoiceManagment.Application.Models.Dtos;
using TaxInvoiceManagment.Domain.Entities;

namespace TaxInvoiceManagment.Application.Mappers
{
    public class TaxMappingProfile : Profile
    {
        public TaxMappingProfile()
        {
            // De Domain Model a DTO
            CreateMap<Tax, TaxDto>();
            // De DTO a Domain Model
            CreateMap<TaxDto, Tax>();
        }
    }
}
