using System;
using Application.Infrastructure.Data.Models;
using Application.Web.Dto;
using AutoMapper;
namespace Application.Web.MapperProfile
{
    public class VehiclesMapperProfile :  Profile
    {
        public VehiclesMapperProfile()
        {
            CreateMap<Vehicle, VehiclePostDto>();
            CreateMap<VehiclePostDto, Vehicle>();

            CreateMap<Vehicle, VehicleGetDto>()
                .ForMember(des => des.CustomerName, opt => opt.MapFrom(src => src.Customer == null ? "" : src.Customer.Name));

            CreateMap<VehicleGetDto, Vehicle>();
        }
    }
}
