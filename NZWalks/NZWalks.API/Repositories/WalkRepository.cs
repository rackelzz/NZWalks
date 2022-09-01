using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nNZWalksDbContext;

        public WalkRepository(NZWalksDbContext nNZWalksDbContext)
        {
            this.nNZWalksDbContext = nNZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            // Assign new ID
            walk.Id = Guid.NewGuid();
            await nNZWalksDbContext.Walks.AddAsync(walk);
            await nNZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await nNZWalksDbContext.Walks.FindAsync(id);
            if (existingWalk == null)
            { 
                return null;
            }
            nNZWalksDbContext.Walks.Remove(existingWalk);
            await nNZWalksDbContext.SaveChangesAsync();
            return existingWalk;

        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await 
                nNZWalksDbContext.Walks
                .Include(x => x.Region)             // return region object instead of ID
                .Include(x => x.WalkDifficulty)     // return WalkDifficulty object instead of ID
                .ToListAsync();
        }

        public async Task<Walk> GetOneAsync(Guid id)
        {
            return await
                nNZWalksDbContext.Walks
                .Include(x => x.Region)             // return region object instead of ID
                .Include(x => x.WalkDifficulty)     // return WalkDifficulty object instead of ID
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await nNZWalksDbContext.Walks.FindAsync(id);
            if (existingWalk != null)
            {
                existingWalk.Length = walk.Length;
                existingWalk.Name = walk.Name;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;
                await nNZWalksDbContext.SaveChangesAsync();
                return existingWalk;      
            }
            return null;
        }
    }
}
