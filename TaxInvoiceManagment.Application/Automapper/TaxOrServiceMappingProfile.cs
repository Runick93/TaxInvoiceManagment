using AutoMapper;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Automapper
{
    public class TaxOrServiceMappingProfile : Profile
    {
        public TaxOrServiceMappingProfile()
        {
            // De Domain Model a DTO
            CreateMap<TaxOrService, TaxOrServiceDto>();
            // De DTO a Domain Model
            CreateMap<TaxOrServiceDto, TaxOrService>();
        }
    }
}
