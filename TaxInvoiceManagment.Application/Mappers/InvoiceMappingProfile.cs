using AutoMapper;
using TaxInvoiceManagment.Application.Models.Dtos;
using TaxInvoiceManagment.Domain.Entities;

namespace TaxInvoiceManagment.Application.Mappers
{
    public class InvoiceMappingProfile : Profile
    {
        public InvoiceMappingProfile() 
        {
            // De Domain Model a DTO
            CreateMap<Invoice, InvoiceDto>();
            // De DTO a Domain Model
            CreateMap<InvoiceDto, Invoice>();
        }
    }
}
