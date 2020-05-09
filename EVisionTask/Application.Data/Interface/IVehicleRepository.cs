using System;
using Application.Infrastructure.Data.Interfaces;
using Application.Infrastructure.Data.Models;

namespace Application.Data.Interface
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
    }
}
