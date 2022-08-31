using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Region>> GetAllAsync()         //async and await and ToListAsync
        {
            return await nNZWalksDbContext.Regions.ToListAsync();
        }
    }
}
