﻿using AutoMapper;
using TaxInvoiceManagment.Application.Models.Dtos;
using TaxInvoiceManagment.Domain.Entities;

namespace TaxInvoiceManagment.Application.Mappers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // De Domain Model a DTO
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore()); // No existe en User

            // De DTO a Domain Model
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.TaxableItems, opt => opt.Ignore()); // Evitamos inicializar TaxableItems
        }
    }
}
