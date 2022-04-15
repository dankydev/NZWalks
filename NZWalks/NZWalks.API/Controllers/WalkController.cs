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

        public WalkController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
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
    }
}
