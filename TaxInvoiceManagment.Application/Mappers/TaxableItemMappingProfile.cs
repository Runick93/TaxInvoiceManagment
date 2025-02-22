using AutoMapper;
using TaxInvoiceManagment.Application.Models.Dtos;
using TaxInvoiceManagment.Domain.Entities;

namespace TaxInvoiceManagment.Application.Mappers
{
    public class TaxableItemMappingProfile : Profile
    {
        public TaxableItemMappingProfile()
        {
            // De Domain Model a DTO
            CreateMap<TaxableItem, TaxableItemDto>();

            // De DTO a Domain Model
            CreateMap<TaxableItemDto, TaxableItem>();

        }
    }
}
