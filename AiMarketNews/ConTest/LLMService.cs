using RestSharp;
using System.Text.Json;


namespace AiMarketNews.ConTest
{
    public class LlmService
    {
        private readonly string _apiKey = "YOUR_API_KEY";

        public async Task<ClassificationResult> ClassifyHeadline(string headline)
        {
            var client = new RestClient("https://api.openai.com/v1/responses");

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