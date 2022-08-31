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

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nNZWalksDbContext.AddAsync(region);
            await nNZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
           var region = await nNZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (region == null)
            { 
                return null;
            }

            // delete the region from DB
            nNZWalksDbContext.Regions.Remove(region);
            await nNZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()         //async and await and ToListAsync
        {
            return await nNZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await nNZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
           var existingRegion = await nNZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.Population = region.Population;
            existingRegion.Lat = region.Lat;
            existingRegion.Long = region.Long;
            existingRegion.Area = region.Area;

            await nNZWalksDbContext.SaveChangesAsync();

            return existingRegion;
        }
    }
}
