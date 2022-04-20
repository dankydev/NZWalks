using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public class RegionController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync() {
            var regions = await regionRepository.GetAllAsync();
            var regionDTOs = mapper.Map<List<Models.DTO.Region>>(regions);

            if (!regionDTOs.Any()) 
                return NoContent();
            

            return Ok(regionDTOs);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync([FromRoute] Guid id) {
            var region = await regionRepository.GetAsync(id);
            if (region == null) 
                return NotFound();
            

            var regionDTO = mapper.Map<Models.DTO.Region>(region);
                
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync([FromBody] AddRegionRequest request) 
        {
            // Validate data
            if (!ValidateAddRegionAsync(request)) 
                return BadRequest(ModelState);
            

            // Request to domain model
            var region = mapper.Map<Models.Domain.Region>(request);

            // Use repository
            region = await regionRepository.AddAsync(region);

            // Convert back to DTO
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync([FromRoute] Guid id)
        {
            var deletedRegion = await regionRepository.DeleteAsync(id);
            if (deletedRegion == null)
                return NotFound();

            var deletedRegionDTO = mapper.Map<Models.DTO.Region>(deletedRegion);

            return Ok(deletedRegionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] UpdateRegionRequest request)
        {
            if (!ValidateUpdateRegionAsync(request))
                return BadRequest(ModelState);

            var region = mapper.Map<Models.Domain.Region>(request);
            var updatedRegion = await regionRepository.UpdateAsync(id, region);
            if (updatedRegion == null)
                return NotFound();

            var updatedRegionDTO = mapper.Map<Models.DTO.Region>(updatedRegion);

            return Ok(updatedRegionDTO);
        }

        #region private methods
        private bool ValidateAddRegionAsync(AddRegionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                ModelState.AddModelError(nameof(AddRegionRequest.Code), "Code can't be null or empty or whitespace");
            }

            if (request.Area <= 0)
            {
                ModelState.AddModelError(nameof(AddRegionRequest.Area), "Area can't be null, zero, negative");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                ModelState.AddModelError(nameof(AddRegionRequest.Name), "Name can't be null or empty or whitespace");
            }

            if (request.Population <= 0)
            {
                ModelState.AddModelError(nameof(AddRegionRequest.Population), "Population can't be null, negative or zero");
            }

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }

        private bool ValidateUpdateRegionAsync(UpdateRegionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                ModelState.AddModelError(nameof(AddRegionRequest.Code), "Code can't be null or empty or whitespace");
            }

            if (request.Area <= 0)
            {
                ModelState.AddModelError(nameof(AddRegionRequest.Area), "Area can't be null, zero, negative");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                ModelState.AddModelError(nameof(AddRegionRequest.Name), "Name can't be null or empty or whitespace");
            }

            if (request.Population <= 0)
            {
                ModelState.AddModelError(nameof(AddRegionRequest.Population), "Population can't be null, negative or zero");
            }

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }
        #endregion
    }
}
