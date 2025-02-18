using AutoMapper;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Automapper
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
