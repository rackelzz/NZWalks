using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nNZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nNZWalksDbContext)
        {
            this.nNZWalksDbContext = nNZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nNZWalksDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await nNZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDiff = await nNZWalksDbContext.WalkDifficulty.FindAsync(id);
            if (existingWalkDiff != null)
            {
                nNZWalksDbContext.WalkDifficulty.Remove(existingWalkDiff);
                nNZWalksDbContext.SaveChangesAsync();
                return existingWalkDiff;
            }
            return null;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
           return await nNZWalksDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nNZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDiff = await nNZWalksDbContext.WalkDifficulty.FindAsync(id);
            if (existingWalkDiff == null)
            {
                return null;
            }

            existingWalkDiff.Code = walkDifficulty.Code;

            await nNZWalksDbContext.SaveChangesAsync();

            return existingWalkDiff;
        }
    }
}
