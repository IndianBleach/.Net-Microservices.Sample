using AutoMapper;
using CommandService.DTOs;
using CommandService.Interfaces;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/c/platforms/{platformId:int}/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepository commandRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetAllCommandsForPlatform(int platformId)
        {
            Console.WriteLine("GetCommandsForPlatform / " + platformId);

            if (!_commandRepository.PlatformExists(platformId))
                return NotFound();

            var commands = _commandRepository.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public async Task<ActionResult<CommandReadDto>> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine("GetCommandForPlatform / " + platformId + " / " + commandId);

            if (!_commandRepository.PlatformExists(platformId))
                return NotFound();

            var command = _commandRepository.GetCommand(platformId, commandId);

            if (command == null)
                return NotFound();

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public async Task<ActionResult<CommandReadDto>> CreateCommand(int platformId, CreateCommandDto model)
        {
            if (!_commandRepository.PlatformExists(platformId))
                return NotFound();

            Command command = _mapper.Map<Command>(model);

            _commandRepository.CreateCommand(platformId, command);
            _commandRepository.SaveChanges();

            CommandReadDto commandDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform),
                new { platformId = platformId, commandId = commandDto.Id }, commandDto);
        }
    }
}
