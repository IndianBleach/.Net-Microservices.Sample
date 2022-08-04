using PlatformService.DTOs;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public HttpCommandDataClient(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json");

            string url = $"{_config["CommandService"]}/api/c/platforms";

            try
            {
                var response = await _client.PostAsync(url, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Post - OK");
                }
                else
                {
                    Console.WriteLine("Post - Not Ok");
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
    }
}
