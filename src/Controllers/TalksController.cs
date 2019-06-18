using AutoMapper;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers
{
    [ApiController]
    [Route("/api/camps/{moniker}/talks")]
    public class TalksController : ControllerBase
    {
        private readonly ICampRepository campRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public TalksController(ICampRepository campRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this.campRepository = campRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<TalkModel[]>> Get(string moniker)
        {
            try
            {
                var result = await campRepository.GetTalksByMonikerAsync(moniker, true);
                if (result == null) { return NotFound(); };
                return mapper.Map<TalkModel[]>(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TalkModel>> Get(string moniker, int id)
        {
            try
            {
                var result = await campRepository.GetTalkByMonikerAsync(moniker, id, true);
                if (result == null) { return NotFound(); };
                return mapper.Map<TalkModel>(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TalkModel>> Post(string moniker, TalkModel talkModel)
        {
            try
            {
                Camp camp = await campRepository.GetCampAsync(moniker, true);
                if (camp == null) { return BadRequest("Camp does not exists"); }

                Talk talk = mapper.Map<Talk>(talkModel);
                talk.Camp = camp;

                if (talkModel.Speaker == null) { return BadRequest("Speaker ID Required"); }
                Speaker speaker = await campRepository.GetSpeakerAsync(talkModel.Speaker.SpeakerId);
                if (speaker == null) { return BadRequest("Speaker could not be found"); }
                talk.Speaker = speaker;
                campRepository.Add(talk);


                if (await campRepository.SaveChangesAsync())
                {
                    string talkUrl = linkGenerator.GetPathByAction("Get", "Talks", new { moniker = moniker, id = talk.TalkId });
                    return Created(talkUrl, mapper.Map<TalkModel>(talk));
                }
                else
                {
                    return BadRequest("Failer to save new Talk");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TalkModel>> Put(string moniker, int id, TalkModel talkModel)
        {
            try
            {
                Talk talk = await campRepository.GetTalkByMonikerAsync(moniker, id, true);
                if (talk == null) { return NotFound("Couldn't find the talk"); }

                mapper.Map(talkModel, talk);
                if (talkModel.Speaker != null)
                {
                    Speaker speaker = await campRepository.GetSpeakerAsync(talkModel.Speaker.SpeakerId);
                    if (speaker != null)
                    {
                        talk.Speaker = speaker;
                    }
                }

                if (await campRepository.SaveChangesAsync())
                {
                    return mapper.Map<TalkModel>(talk);
                }
                return BadRequest("Failed to update database");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(string moniker, int id)
        {
            try
            {
                Talk talk = await campRepository.GetTalkByMonikerAsync(moniker, id, true);
                if (talk == null) { return NotFound("Couldn't find the talk"); }

                campRepository.Delete(talk);
                
                if (await campRepository.SaveChangesAsync())
                {
                    return Ok();
                }
                return BadRequest("Failed to delete talk");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }


    }
}
