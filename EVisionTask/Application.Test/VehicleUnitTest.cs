using System;
using System.Net;
using System.Threading.Tasks;
using Application.Data.Interface;
using Application.Infrastructure.API.BaseResponses;
using Application.Infrastructure.Data.Models;
using Application.Web.Controllers;
using Application.Web.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Application.Test
{
    [TestClass]
    public class VehicleUnitTest
    {
        private VehiclesController _systemUnderTest;

        [TestInitialize]
        public void OnTestInitialize()
        {
            _systemUnderTest = null;
        }
        [TestMethod]
        public async Task TestMethod()
        {
            var repository = new Mock<IVehicleRepository>();
            var logger = new Mock<ILogger<VehiclesController>>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Vehicle, VehicleGetDto>()
                                   .ForMember(des => des.CustomerName, opt => opt.MapFrom(src => src.Customer == null ? "" : src.Customer.Name));
                cfg.CreateMap<VehicleGetDto, Vehicle>();
            });

            var mapper = mockMapper.CreateMapper();

            _systemUnderTest =
              new VehiclesController(repository.Object, mapper,  logger.Object );


            var result = await _systemUnderTest.GetVehiclesWithCustomers();

            Assert.IsNotNull(result, "result != null");
            var (statusCode, evisionResult) = result.ToEvisionResponse();
            Assert.AreEqual(HttpStatusCode.OK, statusCode, "Incorrect status code");

            Assert.IsNotNull(evisionResult, "dasaResult != null");
            var response = evisionResult.Response as VehicleGetDto;
            Assert.IsNotNull(response, "response != null");
        }

        
    }
}
