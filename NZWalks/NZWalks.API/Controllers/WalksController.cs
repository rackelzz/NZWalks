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
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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
            // validate incoming req
            if (!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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
            // validate incoming request 
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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


        #region Private Methods

        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            //if (addWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest), 
            //        $"{nameof(addWalkRequest)} cannot be empty.");
            //    return false;
            //}
            //if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Name), 
            //        $"{nameof(addWalkRequest.Name)} cannot be null or whitespace.");
            //}
            //if (addWalkRequest.Length <= 0 )
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Length),
            //        $"{nameof(addWalkRequest.Length)} should be greater than zero.");
            //}

            // check if id is valid and exists 
            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                        $"{nameof(addWalkRequest.RegionId)} is invalid.");
            }

            var walkDiff = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDiff == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                            $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");
            }

            if (ModelState.ErrorCount > 0)
            { 
                return false;
            }

            return true;

        }


        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest) 
        {
            //if (updateWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest),
            //        $"{nameof(updateWalkRequest)} cannot be empty.");
            //    return false;
            //}
            //if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Name),
            //        $"{nameof(updateWalkRequest.Name)} cannot be null or whitespace.");
            //}
            //if (updateWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Length),
            //        $"{nameof(updateWalkRequest.Length)} should be greater than zero.");
            //}

            // check if id is valid and exists 
            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                        $"{nameof(updateWalkRequest.RegionId)} is invalid.");
            }

            var walkDiff = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDiff == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                            $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;

        }
        #endregion

    }
}

