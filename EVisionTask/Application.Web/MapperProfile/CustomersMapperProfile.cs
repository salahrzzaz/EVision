using System;
using Application.Infrastructure.Data.Models;
using Application.Web.Dto;
using AutoMapper;

namespace Application.Web.MapperProfile
{
    public class CustomersMapperProfile :  Profile
    {
        public CustomersMapperProfile()
        {
            CreateMap<Customers, CustomersDto>();

            CreateMap<CustomersDto, Customers>();
        }
    }
}
