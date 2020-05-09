using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Data.Interface;
using Application.Infrastructure.Data.Interfaces;
using Application.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Data.Seeds
{
    public class SeedData : ISeedData
    {
        private readonly IRepository<Customers> _customersRepository;
        private readonly IVehicleRepository _vehicleRepository;
        public SeedData(IRepository<Customers> customersRepository, IVehicleRepository vehicleRepository)
        {
            _customersRepository = customersRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task InitSeedData()
        {
            try
            {
                var customer1 = new Customers() { Name = "Customer1", Address = "Cairo, Nasr City" };
                var customer2 = new Customers() { Name = "Customer2", Address = "Cairo, Maadi" };
                var customer3 = new Customers() { Name = "Customer3", Address = "Cairo, 5th Settlement" };
                if (await _customersRepository.Count(c => c.Name == customer1.Name) == 0)
                {
                    var customer1Result = await _customersRepository.Insert(customer1);
                    var vehicle1 = new Vehicle() { CustomerId = customer1Result.Id, RegNumber = "ABC123", VehicleId = "YS2R4X20005399401" };
                    var vehicle2 = new Vehicle() { CustomerId = customer1Result.Id, RegNumber = "DEF456", VehicleId = "VLUR4X20009093588" };
                    var vehicle3 = new Vehicle() { CustomerId = customer1Result.Id, RegNumber = "GHI789", VehicleId = "VLUR4X20009048066" };

                    await _vehicleRepository.Insert(vehicle1);
                    await _vehicleRepository.Insert(vehicle2);
                    await _vehicleRepository.Insert(vehicle3);
                }
                if (await _customersRepository.Count(c => c.Name == customer2.Name) == 0)
                {
                    var customer2Result = await _customersRepository.Insert(customer2);
                    var vehicle1 = new Vehicle() { CustomerId = customer2Result.Id, RegNumber = "JKL012", VehicleId = "YS2R4X20005388011" };
                    var vehicle2 = new Vehicle() { CustomerId = customer2Result.Id, RegNumber = "MNO345", VehicleId = "YS2R4X20005387949" };

                    await _vehicleRepository.Insert(vehicle1);
                    await _vehicleRepository.Insert(vehicle2);

                }
                if (await _customersRepository.Count(c => c.Name == customer3.Name) == 0)
                {
                    var customer3Result = await _customersRepository.Insert(customer3);
                    var vehicle1 = new Vehicle() { CustomerId = customer3Result.Id, RegNumber = "PQR678", VehicleId = "YS2R4X20005387765" };
                    var vehicle2 = new Vehicle() { CustomerId = customer3Result.Id, RegNumber = "STU901", VehicleId = "YS2R4X20005387055" };

                    await _vehicleRepository.Insert(vehicle1);
                    await _vehicleRepository.Insert(vehicle2);
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
