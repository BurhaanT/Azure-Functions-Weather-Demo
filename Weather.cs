using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Azure_Function_Weather_Demo
{
    public static class Weather
    {
        [FunctionName("Get-Weather")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var apiKey = config["ApiKey"];
            var city = config["WeatherLocation:City"];
            var country = config["WeatherLocation:Country"];
            var weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city},{country}&appid={apiKey}";

            var client = new HttpClient();
            var response = await client.GetAsync(weatherUrl);
            var content = await response.Content.ReadAsStringAsync();

            return new OkObjectResult(content);
        }
    }
}
