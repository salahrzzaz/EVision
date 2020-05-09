using System;
using System.Threading.Tasks;

namespace Application.Data.Seeds
{
    public interface ISeedData
    {
        Task InitSeedData();
    }
}
