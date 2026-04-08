using AiMarketNews.ConTest.Agent;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AiMarketNews.ConTest.Tools
{
    public class GetSectorTool : ITool
    {
        public string Name => "get_sector";

        public object Schema => new
        {
            type = "function",
            function = new
            {
                name = Name,
                description = "Get stock sector for a company",
                parameters = new
                {
                    type = "object",
                    properties = new
                    {
                        company = new { type = "string" }
                    },
                    required = new[] { "company" }
                }
            }
        };

        public Task<string> Execute(string argumentsJson)
        {
            var args = JsonSerializer.Deserialize<Dictionary<string, string>>(argumentsJson);

            var company = args["company"];

            // Fake logic (replace with API)
            var sector = company.ToLower().Contains("tesla")
                ? "Automotive / EV"
                : "Technology";

            return Task.FromResult($"{{ \"sector\": \"{sector}\" }}");
        }
    }
}
