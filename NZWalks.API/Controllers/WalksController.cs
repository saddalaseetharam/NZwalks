using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await walkRepository.GetAllAsync();
            var walksDTO = mapper.Map<List<Models.Domain.Walk>>(walks);
            return Ok(walksDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //fetch data from database
            var walkDomin = await walkRepository.GetAsync(id);

            //Convert Domain object to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomin);

            //return response
            return Ok(walkDTO);

        }
        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert DTO to domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionID = addWalkRequest.RegionID,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };
            walkDomain = await walkRepository.AddAsync(walkDomain);
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionID = walkDomain.RegionID,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,
            [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // Convert DTO to Domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionID = updateWalkRequest.RegionID,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            // Pass details to Repository - Get Domain object in response (or null)
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            // Handle Null (not found)
            if (walkDomain == null)
            {
                return NotFound();
            }

            // Convert back Domain to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionID = walkDomain.RegionID,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            // Return Response
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            // call Repository to delete walk
            var walkDomain = await walkRepository.DeleteAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDTO);
        }
    }
}
