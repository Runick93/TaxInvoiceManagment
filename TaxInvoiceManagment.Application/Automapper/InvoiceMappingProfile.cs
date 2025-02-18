using AutoMapper;
using TaxInvoiceManagment.Application.Dtos;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Application.Automapper
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
