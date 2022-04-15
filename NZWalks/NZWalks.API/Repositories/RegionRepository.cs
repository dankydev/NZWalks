using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using AutoMapper;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        private readonly IMapper mapper;

        public RegionRepository(NZWalksDbContext nZWalksDbContext, IMapper mapper)
        {
            this.nZWalksDbContext = nZWalksDbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Region?>> GetAllAsync()
        {
            return await nZWalksDbContext.Region.ToListAsync();
        }

        public async Task<Region?> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Region.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nZWalksDbContext.Region.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var regionToDelete = await nZWalksDbContext.Region.FirstOrDefaultAsync(x => x.Id == id);
            if (regionToDelete == null)
                return null;

            nZWalksDbContext.Region.Remove(regionToDelete);
            await nZWalksDbContext.SaveChangesAsync();

            return regionToDelete;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var regionToUpdate = await nZWalksDbContext.Region.FirstOrDefaultAsync(x =>x.Id == id);
            if (regionToUpdate == null)
                return null;


            // regionToUpdate = mapper.Map<Region?>(region);
            // TODO: AutoMapper doesn't work, why?
            regionToUpdate.Latitude = region.Latitude;
            regionToUpdate.Longitude = region.Longitude;
            regionToUpdate.Population = region.Population;
            regionToUpdate.Name = region.Name;
            regionToUpdate.Code = region.Code;
            regionToUpdate.Area = region.Area;

            await nZWalksDbContext.SaveChangesAsync();

            return region;
        }
    }
}

