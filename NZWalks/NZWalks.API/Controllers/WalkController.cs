using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public class WalkController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IRegionRepository regionRepository;

        public WalkController(IWalkRepository walkRepository, IMapper mapper, IWalkDifficultyRepository walkDifficultyRepository, IRegionRepository regionRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.regionRepository = regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await walkRepository.GetAllAsync();
            if (!walks.Any())
                return NoContent();

            var walksDTO = mapper.Map<IEnumerable<Models.DTO.Walk>>(walks);

            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync([FromRoute] Guid id)
        {
            var walk = await walkRepository.GetAsync(id);
            if (walk == null)
                return NotFound();

            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequest request) {
            if (!await ValidateUpdateWalkAsync(request))
                return BadRequest(ModelState);

            var walk = mapper.Map<Models.Domain.Walk>(request);

            var updatedWalk = await walkRepository.UpdateAsync(id, walk);
            if (updatedWalk == null)
                return NotFound();

            var walkDTO = mapper.Map<Models.DTO.Walk>(updatedWalk);
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequest request)
        {
            if (!await ValidateAddWalkAsync(request))
                return BadRequest(ModelState);
            

            var walk = mapper.Map<Models.Domain.Walk>(request);
            walk = await walkRepository.AddAsync(walk);
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walk.Id }, walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync(Guid id)
        { 
            var walk = await walkRepository.DeleteAsync(id);
            if (walk == null)
                return NotFound();

            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);
        }

        #region private methods
        private async Task<bool> ValidateAddWalkAsync(AddWalkRequest request)
        { 
            var region = await regionRepository.GetAsync(request.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(AddWalkRequest.RegionId), "Invalid region id");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(request.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(AddWalkRequest.WalkDifficultyId), "Invalid walk difficulty id");
            }

            if (request.Length <= 0)
            { 
                ModelState.AddModelError(nameof(AddWalkRequest.Length), "Request can't be null, negative or zero");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                ModelState.AddModelError(nameof(AddRegionRequest.Name), "Name can't be null or empty or whitespace");
            }

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(UpdateWalkRequest request)
        {
            var region = await regionRepository.GetAsync(request.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(AddWalkRequest.RegionId), "Invalid region id");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(request.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(AddWalkRequest.WalkDifficultyId), "Invalid walk difficulty id");
            }

            if (request.Length <= 0)
            {
                ModelState.AddModelError(nameof(AddWalkRequest.Length), "Request can't be null, negative or zero");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                ModelState.AddModelError(nameof(AddRegionRequest.Name), "Name can't be null or empty or whitespace");
            }

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }

        #endregion
    }
}
