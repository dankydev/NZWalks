using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty?> DeleteAsync(Guid id)
        {
            var walkDifficultyToDelete = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
            if (walkDifficultyToDelete == null)
                return null;

            nZWalksDbContext.Remove(walkDifficultyToDelete);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficultyToDelete;
        }

        public async Task<IEnumerable<WalkDifficulty?>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficulty.ToListAsync();   
        }

        public async Task<WalkDifficulty?> GetAsync(Guid id)
        {
            var walkDifficulty = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
            if (walkDifficulty == null)
                return null;

            return walkDifficulty;
        }

        public async Task<WalkDifficulty?> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var walkDifficultyToUpdate = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
            if (walkDifficultyToUpdate == null)
                return null;

            walkDifficultyToUpdate.Code = walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficultyToUpdate;
        }
    }
}
