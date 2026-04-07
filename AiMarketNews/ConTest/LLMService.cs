using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;


namespace AiMarketNews.ConTest
{
    public class LlmService
    {
        private readonly IConfiguration _config;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public LlmService()
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

        public async Task<ClassificationResult> ClassifyHeadline(string headline)
        {
            var client = new RestClient(_baseUrl);

            var prompt = $@"
                            Classify this news headline into stock market sectors.

                            Headline: ""{headline}""

                            Return JSON with:
                            - primary_sector
                            - secondary_sectors (array)
                            - sentiment (positive, neutral, negative)
                            - confidence (0-1)
                            - reasoning

                            Use sectors like:
                            Energy, Technology, Industrials, Aerospace & Defense, Financials, Healthcare, etc.
                            ";



            var request = new RestRequest("", Method.Post);
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddHeader("Content-Type", "application/json");

            request.AddJsonBody(new
            {
                model = "gpt-4.1-mini",
                input = prompt
            });

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new Exception(response.ErrorMessage);

            using var doc = JsonDocument.Parse(response.Content);

            var text = doc.RootElement
                .GetProperty("output")[0]
                .GetProperty("content")[0]
                .GetProperty("text")
                .GetString();

            return JsonSerializer.Deserialize<ClassificationResult>(text);
        }
    }
}