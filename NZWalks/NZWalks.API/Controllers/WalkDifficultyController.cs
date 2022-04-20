using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("WalkDifficulties")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllWalkDifficultiesAsync() 
        {
            var difficulties = await walkDifficultyRepository.GetAllAsync();
            if (!difficulties.Any())
                return NoContent();

            var difficultieDTOs = mapper.Map<IEnumerable<Models.DTO.WalkDifficulty>>(difficulties);

            return Ok(difficultieDTOs);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<ActionResult> GetWalkDifficultyAsync([FromRoute] Guid id) 
        { 
            var difficulty = await walkDifficultyRepository.GetAsync(id);
            if (difficulty == null) return NotFound();

            var difficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(difficulty);
            return Ok(difficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<ActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] UpdateWalkDifficultyRequest request)
        {
            if (!ValidateUpdateWalkDifficulty(request))
                return BadRequest(ModelState);

            var walkDifficultyToUpdate = mapper.Map<Models.Domain.WalkDifficulty>(request);
            var updatedDifficulty = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyToUpdate);
            if (updatedDifficulty == null)
                return NotFound();
        
            var updatedDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(updatedDifficulty);
            
            return Ok(updatedDifficultyDTO);
        }

        [HttpPost]
        public async Task<ActionResult> AddWalkDifficultyAsync([FromBody] AddWalkDifficultyRequest request) 
        {
            if (!ValidateAddWalkDifficulty(request)) 
                return BadRequest(ModelState);

            var difficultyToAdd = mapper.Map<Models.Domain.WalkDifficulty>(request);
            difficultyToAdd = await walkDifficultyRepository.AddAsync(difficultyToAdd);
            var addedDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(difficultyToAdd);
            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { Id = addedDifficultyDTO.Id }, addedDifficultyDTO);
        }
        
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult> DeleteWalkDifficultyAsync([FromRoute] Guid id)
        {
            var deletedDifficulty = await walkDifficultyRepository.DeleteAsync(id);
            if (deletedDifficulty == null) return NotFound();

            var deletedDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(deletedDifficulty);
            return Ok(deletedDifficultyDTO);
        }

        #region private methods
        private bool ValidateAddWalkDifficulty(AddWalkDifficultyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                ModelState.AddModelError(nameof(AddWalkDifficultyRequest.Code), "The code can't be null or whitespace");
            }

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }

        private bool ValidateUpdateWalkDifficulty(UpdateWalkDifficultyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                ModelState.AddModelError(nameof(UpdateWalkDifficultyRequest.Code), "The code can't be null or whitespace");
            }

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }

        #endregion
    }
}
