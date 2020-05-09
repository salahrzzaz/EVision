using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Data.Interface;
using Application.Infrastructure.Data.Models;
using Application.Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Application.Data.Repository
{
    public class VehicleRepository: EntityFrameworkRepositoryBase<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(DbContext dbContext)
         : base(dbContext)
        {

        }

        public override async Task<IEnumerable<Vehicle>> List(Expression<Func<Vehicle, bool>> predicate)
        {
            var result = await Context.Set<Vehicle>().
                Where(predicate).Include(c => c.Customer).ToListAsync();
            return result;
        }
    }
}
