using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using Microsoft.Extensions.Configuration;


namespace AiMarketNews.ConTest.LLM
{
    public class LlmClient
    {
        private readonly IConfiguration _config;
        private readonly RestClient _client;

        private readonly string _apiKey;
        private readonly string _baseUrl;

        public LlmClient()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath("C:\\Users\\Connor\\source\\repos\\AiMarketNews\\AiMarketNews")      //Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _config = builder.Build();
            _apiKey = _config["OpenAIKey:ApiKey"];
            _baseUrl = _config["OpenAIKey:BaseUrl"] ?? "https://api.openai.com/v1/responses";

            if (string.IsNullOrEmpty(_apiKey))
                throw new InvalidOperationException("OpenAI API key not configured in appsettings.json");
        }

        public async Task<LlmResponse> Send(Agent.AgentContext context, IEnumerable<object> tools)
        {
            var request = new RestRequest("", Method.Post);

            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddHeader("Content-Type", "application/json");

            request.AddJsonBody(new
            {
                model = "gpt-4.1-mini",
                input = context.Messages,
                tools = tools
            });

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new Exception(response.ErrorMessage);

            return new LlmResponse(response.Content);
        }
    }
}
