using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices.Http;
using PlatformService.DTOs;
using PlatformService.Interfaces;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandClient;

        public PlatformsController(
            IPlatformRepository platRepo,
            IMapper mapper,
            ICommandDataClient client)
        {
            _platformRepository = platRepo;
            _mapper = mapper;
            _commandClient = client;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platforms = _platformRepository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id:int}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById([FromRoute] int id)
        {
            Platform platform = _platformRepository.GetPlatformById(id);

            if (platform != null)
                return Ok(_mapper.Map<PlatformReadDto>(platform));
            
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform([FromForm]PlatformCreateDto model)
        { 
            var platform = _mapper.Map<Platform>(model);
            _platformRepository.CreatePlatform(platform);
            _platformRepository.SaveChanges();

            var platformRead = _mapper.Map<PlatformReadDto>(platform);

            try
            {
                await _commandClient.SendPlatformToCommand(platformRead);

                Console.WriteLine("Send platform to command - ok");
            }
            catch (Exception exp)
            {
                Console.WriteLine("Send platform to command - not ok");
            }


            return CreatedAtRoute(nameof(GetPlatformById), new { id = platformRead.Id }, platformRead);
        }

    }
}
