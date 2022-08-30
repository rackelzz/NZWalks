using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

//DbContext represents a sessin with the database
// 1. Make connection 
// 2. Query 
// 3. Persist Data 

namespace NZWalks.API.Data
{
    public class NZWalksDbContext: DbContext
    {
        //ctor tab to create contructor 
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options) : base(options)
        {

        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }



    }
}
