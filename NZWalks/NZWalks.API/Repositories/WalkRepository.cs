using AutoMapper;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.Walk.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walk = await nZWalksDbContext.Walk.FirstOrDefaultAsync(x => x.Id == id);
            if (walk == null)
                return null;

            nZWalksDbContext.Remove(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<IEnumerable<Walk?>> GetAllAsync()
        {
            return await nZWalksDbContext.Walk
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk?> GetAsync(Guid id)
        {
            var walk = await nZWalksDbContext.Walk
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (walk == null)
                return null;

            return walk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var walkToUpdate = await nZWalksDbContext.Walk
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (walkToUpdate == null)
                return null;    

            walkToUpdate.Length = walk.Length;
            walkToUpdate.Name = walk.Name;
            walkToUpdate.RegionId = walk.RegionId;
            walkToUpdate.WalkDifficultyId = walk.WalkDifficultyId;

            await nZWalksDbContext.SaveChangesAsync();
            return walkToUpdate;
        }
    }
}
