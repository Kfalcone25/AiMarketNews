using AiMarketNews.ConTest.Agent;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace AiMarketNews.ConTest
{
    public class MockMarketTool : ITool
    {
        public string Name => "analyze_market_news";

        public object Schema => new
        {
            type = "function",
            function = new
            {
                name = Name,
                description = "Analyzes stock news and predicts movement",
                parameters = new
                {
                    type = "object",
                    properties = new
                    {
                        ticker = new { type = "string" },
                        headline = new { type = "string" }
                    },
                    required = new[] { "ticker", "headline" }
                }
            }
        };

        public Task<string> Execute(string argumentsJson)
        {
            // 🔹 Fake tool result
            var mockResult = new
            {
                prediction = "bullish",
                confidence = 0.92,
                reasoning = "Strong earnings and positive market sentiment"
            };

            return Task.FromResult(JsonSerializer.Serialize(mockResult));
        }
    }
}
