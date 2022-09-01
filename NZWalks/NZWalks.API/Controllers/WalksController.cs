using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            //fetch data form db -- the returns domain walks 
            var walksDomin = await walkRepository.GetAllAsync();

            //convert domian walks to DTO walks 
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomin);

            //return responce 
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetSingleWalkAsync")]
        public async Task<IActionResult> GetSingleWalkAsync(Guid id)
        {
            // Get Walk Domin object from DB 
            var walkDomain = await walkRepository.GetOneAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            // Convert domin to DTO
            var walksDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // responce 
            return Ok(walksDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            // convert DTO to Domain obj
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId

            };

            // Pass domin obj to Repository to add to DB
            walkDomain = await walkRepository.AddAsync(walkDomain);

            // convert back to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // send Ok snd DTO back
            return CreatedAtAction(nameof(GetSingleWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // convert DTO to domain 
            var walkDomin = mapper.Map<Models.Domain.Walk>(updateWalkRequest);

            // call repositpry function 
            walkDomin = await walkRepository.UpdateAsync(id, walkDomin);

            // check for null 
            if (walkDomin == null)
            {
                return NotFound();
            }
            // convert to DTO 
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomin);

            // send responce 
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            //call delete 
            var walkDomain = await walkRepository.DeleteAsync(id);

            //check if null 
            if (walkDomain == null)
            {
                return NotFound();
            }
            // convert to DTO and reteurn 
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDTO);

        }
    }
}

