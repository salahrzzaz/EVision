using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data.Interface;
using Application.Data.Seeds;
using Application.Infrastructure.API.BaseControllers;
using Application.Infrastructure.Data.Base;
using Application.Infrastructure.Data.Interfaces;
using Application.Infrastructure.Data.Models;
using Application.Web.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Application.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : EVisionApiMapperControllerBase<Vehicle, VehiclePostDto>
    {
        private readonly IVehicleRepository _vehicleRepository;
        public VehiclesController(IVehicleRepository repository, IMapper mapper,
            ILogger<VehiclesController> logger) :
            base(repository, mapper, logger)
        {
            _vehicleRepository = repository;
        }

        [Route("GetVehiclesWithCustomers")]
        [HttpGet]
        public async Task<IActionResult> GetVehiclesWithCustomers(int CustomerId = 0, bool? statues = null)
        {
            var result  = await _vehicleRepository.List(c => (c.Status == statues || statues == null) && (CustomerId == 0 || c.CustomerId == CustomerId));
            var mapped = Mapper.Map<List<VehicleGetDto>>(result);
            return Success("Entity Found", mapped);
        }
    }
}
