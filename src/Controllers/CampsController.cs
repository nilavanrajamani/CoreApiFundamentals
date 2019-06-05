using AutoMapper;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository campRepository;
        private readonly IMapper mapper;

        public CampsController(ICampRepository campRepository, IMapper mapper)
        {
            this.campRepository = campRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CampModel[]>> GetCamps()
        {
            try
            {
                var result = await campRepository.GetAllCampsAsync();
                return mapper.Map<CampModel[]>(result);
            }            
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure"); 
            }
        }
    }
}
