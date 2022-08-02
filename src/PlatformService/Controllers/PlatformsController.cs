using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public PlatformsController(IPlatformRepository platRepo,
            IMapper mapper)
        {
            _platformRepository = platRepo;
            _mapper = mapper;
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
        public ActionResult<PlatformReadDto> CreatePlatform([FromForm]PlatformCreateDto model)
        { 
            var platform = _mapper.Map<Platform>(model);
            _platformRepository.CreatePlatform(platform);
            _platformRepository.SaveChanges();

            var platformRead = _mapper.Map<PlatformReadDto>(platform);

            return CreatedAtRoute(nameof(GetPlatformById), new { id = platformRead.Id }, platformRead);
        }

    }
}
