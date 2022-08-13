using AutoMapper;
using CommandService.DTOs;
using CommandService.Interfaces;
using CommandService.Models;
using System.Text.Json;

namespace CommandService.EventProcessing
{
    enum EventType
    { 
        PlatformPublished,
        Undetermined
    }

    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceFactory, IMapper mapper)
        {
            _serviceFactory = serviceFactory;
            _mapper = mapper;
        }


        private EventType DetermineEvent(string notifyMessage)
        {
            Console.WriteLine("Determine event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifyMessage);

            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> platform published event - Detected");
                    return EventType.PlatformPublished;

                default:
                    Console.WriteLine("--> platform published event - UNdetected");
                    return EventType.Undetermined;
            }
        }


        private void addPlatform(string platformPublished)
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                ICommandRepository repo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();

                var platformPub = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublished);

                var plat = _mapper.Map<Platform>(platformPub);

                if (!repo.ExternalPlatformExist(plat.ExternalId))
                {
                    repo.CreatePlatform(plat);
                    repo.SaveChanges();

                    Console.WriteLine("Platform Add");
                }
                else
                {
                    Console.WriteLine("Could't create platform%: already exists");
                }
            }              
        }

        public void ProcessEvent(string message)
        {
            EventType type = DetermineEvent(message);

            switch (type)
            {
                case EventType.PlatformPublished:
                    addPlatform(message);                    
                    break;

                default:
                    break;
            }
        }
    }
}
