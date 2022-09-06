using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var walkDiffDomain = await walkDifficultyRepository.GetAllAsync();
            var walkDiffDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDiffDomain);
            return Ok(walkDiffDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyByID")]
        public async Task<IActionResult> GetWalkDifficultyByID(Guid id)
        {
            var walkDiffDomain = await walkDifficultyRepository.GetAsync(id);
            if (walkDiffDomain == null)
            {
                return NotFound();
            }

            var walkDiffDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDiffDomain);
            return Ok(walkDiffDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficulty(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            // convert DTO to domain
            var walkDiffDomain = mapper.Map<Models.Domain.WalkDifficulty>(addWalkDifficultyRequest);

            // add to db through repositpry
            walkDiffDomain = await walkDifficultyRepository.AddAsync(walkDiffDomain);

            // convert domin to DTO
            var walkDiffDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDiffDomain);

            //return to user 
            return CreatedAtAction(nameof(GetWalkDifficultyByID), new { id = walkDiffDTO.Id }, walkDiffDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficulty([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            var walkDiffDomain = mapper.Map<Models.Domain.WalkDifficulty>(updateWalkDifficultyRequest);

            walkDiffDomain = await walkDifficultyRepository.UpdateAsync(id, walkDiffDomain);

            if (walkDiffDomain == null)
            {
                return NotFound();
            }

            var walkDiffDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDiffDomain);

            return Ok(walkDiffDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        { 
            var walkDiffDomain = await walkDifficultyRepository.DeleteAsync(id);
            if (walkDiffDomain == null)
            {
                return NotFound();
            }
            var walkDiffDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDiffDomain);
            return Ok(walkDiffDTO);
        }
    }
}
