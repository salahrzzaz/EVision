using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class CustomersController : EVisionApiMapperControllerBase<Customers, CustomersDto>
    {
        private readonly ISeedData _seedData;
        public CustomersController(IRepository<Customers> repository,
            IMapper mapper, ILogger<CustomersController> logger, ISeedData seedData) :
            base(repository, mapper, logger)
        {
            _seedData = seedData;
        }

      
        public override async Task<IActionResult> Get(int id)
        {
            return await base.Get(id);
        }







    }
}
