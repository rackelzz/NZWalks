using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]      //---> same as [Route("Regions")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)    // constructor
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]               // Get Request
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();
            // returns domin objects, map to DTO to retutn the list to users 

            // return DTO regions                                   // manual code - without mapper
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population
            //    };
            //    regionsDTO.Add(regionDTO);
            //});


            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);      // pass regions from DB to mapper and get DTO to return

            return Ok(regionsDTO);     //200 success responce 
        }

        [HttpGet]
        [Route("{id:guid}")]        //forced guid type value 
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var regionDOM = await regionRepository.GetAsync(id);

            if (regionDOM == null)
            { 
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(regionDOM);
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // pass in DTO -> convert to domain model -> save to DB with .AddAsync -> convert domain model back to DTO

            // validiate the request:
            if (!ValidateAddRegionAsync(addRegionRequest))
            { 
                return BadRequest(ModelState);
            }

            // convert request (DTO) to domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };

            // pass details of domain model to repository (saves to DB within AddAsync method)
            region = await regionRepository.AddAsync(region);


            // convert back to DTO to return 
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population

            };

            return CreatedAtAction(nameof(GetRegionAsync), new {id = regionDTO.Id}, regionDTO); // HTTP 201 created
                                                                                                //     CreatedAtAction(actionName            , routeValues            , value     )
                                                                                                // generates a location URL in the responce header to directly access the newly created ob
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            // Try to delete region from DB
            var regionDeleted = await regionRepository.DeleteAsync(id);


            // if null return NotFound
            if (regionDeleted == null) 
            {
                return NotFound();
            }

            // else convert responce to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = regionDeleted.Id,
                Code = regionDeleted.Code,
                Name = regionDeleted.Name,
                Area = regionDeleted.Area,
                Lat = regionDeleted.Lat,
                Long = regionDeleted.Long,
                Population = regionDeleted.Population
            };

            // return Ok responce 
            return Ok(regionDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            // validate DTO
            if (!ValidateUpdateRegionAsync(updateRegionRequest))
            { 
                return BadRequest(ModelState);
            }

            // Convert DTO to domian model

            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population,
                Area = updateRegionRequest.Area

            };

            // update region using repository 
            region = await regionRepository.UpdateAsync(id, region);

            // if null then NotFound
            if (region == null)
            {
                return NotFound();
            }

            // Else convert Domin back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            // Return Ok responce 
            return Ok(regionDTO);

        }

        #region Private Methods

        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // method will validate the passed in DTO
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(AddRegionRequest), $"Add region data is required.*");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code), 
                    $"{nameof(addRegionRequest.Code)} cannot be null, empty or white space.");
            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{nameof(addRegionRequest.Name)} cannot be null, empty or white space.");
            }
            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                    $"{nameof(addRegionRequest.Area)} cannot be less than or equal to zero.");
            }
            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                    $"{nameof(addRegionRequest.Population)} cannot be less than zero.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;

        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest) 
        {
            // method will validate the passed in DTO
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(AddRegionRequest), $"Add region data is required.*");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{nameof(updateRegionRequest.Code)} cannot be null, empty or white space.");
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    $"{nameof(updateRegionRequest.Name)} cannot be null, empty or white space.");
            }
            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                    $"{nameof(updateRegionRequest.Area)} cannot be less than or equal to zero.");
            }
            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                    $"{nameof(updateRegionRequest.Population)} cannot be less than zero.");
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
