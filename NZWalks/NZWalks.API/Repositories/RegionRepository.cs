using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository   //implementatin must implement all methods defined in interface
    {
        private readonly NZWalksDbContext nNZWalksDbContext;

        public RegionRepository(NZWalksDbContext nNZWalksDbContext) // injecting dbcontext through constructor 
        {
            this.nNZWalksDbContext = nNZWalksDbContext;
        }

        public IEnumerable<Region> GetAll()
        {
            return nNZWalksDbContext.Regions.ToList();
        }
    }
}
