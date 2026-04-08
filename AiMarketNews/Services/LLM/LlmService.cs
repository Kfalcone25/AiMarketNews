using AiMarketNews.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AiMarketNews.Services.LLM
{
	public class LlmService
	{
		private readonly IConfiguration _config;
		private readonly string _apiKey;
		private readonly string _baseUrl;
		private readonly string _model;

		public LlmService()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			_config = builder.Build();

			_apiKey = _config["OpenAI:ApiKey"] ?? string.Empty;
			_baseUrl = _config["OpenAI:BaseUrl"] ?? "https://api.openai.com/v1/responses";
			_model = _config["OpenAI:Model"] ?? "gpt-4.1-mini";

			if (string.IsNullOrWhiteSpace(_apiKey))
				throw new InvalidOperationException("OpenAI API key not configured in appsettings.json");
		}

		public async Task<LlmResponse<MarketNewsAnalysisResult>> AnalyzeArticleAsync(Article article)
		{
			using var httpClient = new HttpClient();

			httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

			var prompt = $@"
					You are a financial news analysis assistant.

					Analyze the following news article for stock-market relevance.

					ARTICLE TITLE:
					{article.Title}

					SOURCE:
					{article.Source}

					SUMMARY:
					{article.Summary}

					FULL URL:
					{article.Url}

					PUBLISHED AT:
					{article.PublishedAt:O}

					Your task:
					1. Identify the primary market sector affected.
					2. Identify any secondary sectors affected.
					3. Identify the public companies most likely affected.
					4. Determine sentiment as one of: positive, neutral, negative.
					5. Estimate confidence from 0 to 1.
					6. Write a concise market impact summary explaining how the companies/sectors may be affected.
					7. List the key drivers behind the impact.
					8. List any risks, caveats, or uncertainty factors.
					9. Provide a short reasoning statement.

					Return only valid JSON.
					";

			var requestBody = new
			{
				model = _model,
				input = prompt,
				text = new
				{
					format = new
					{
						type = "json_schema",
						name = "market_news_analysis",
						schema = new
						{
							type = "object",
							additionalProperties = false,
							properties = new
							{
								primarySector = new { type = "string" },
								secondarySectors = new
								{
									type = "array",
									items = new { type = "string" }
								},
								affectedCompanies = new
								{
									type = "array",
									items = new { type = "string" }
								},
								sentiment = new
								{
									type = "string",
									@enum = new[] { "positive", "neutral", "negative" }
								},
								confidence = new { type = "number" },
								marketImpactSummary = new { type = "string" },
								keyDrivers = new
								{
									type = "array",
									items = new { type = "string" }
								},
								risksOrCaveats = new
								{
									type = "array",
									items = new { type = "string" }
								},
								reasoning = new { type = "string" }
							},
							required = new[]
							{
								"primarySector",
								"secondarySectors",
								"affectedCompanies",
								"sentiment",
								"confidence",
								"marketImpactSummary",
								"keyDrivers",
								"risksOrCaveats",
								"reasoning"
							}
						}
					}
				}
			};

			var json = JsonSerializer.Serialize(requestBody);

			var response = await httpClient.PostAsync(
				_baseUrl,
				new StringContent(json, Encoding.UTF8, "application/json")
			);

			var responseContent = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(responseContent))
			{
				throw new Exception($"OpenAI request failed. Status: {response.StatusCode}, Content: {responseContent}");
			}

			using var doc = JsonDocument.Parse(responseContent);

			string jsonText = ExtractOutputText(doc);
			
			var usage = new LlmUsage();

			if (doc.RootElement.TryGetProperty("usage", out var usageElement))
			{
				usage.InputTokens = usageElement.GetProperty("input_tokens").GetInt32();
				usage.OutputTokens = usageElement.GetProperty("output_tokens").GetInt32();
			}

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};

			var result = JsonSerializer.Deserialize<MarketNewsAnalysisResult>(jsonText, options);

			if (result == null)
				throw new Exception("Failed to deserialize OpenAI response into MarketNewsAnalysisResult.");

			return new LlmResponse<MarketNewsAnalysisResult>
			{
				Data = result,
				Usage = usage
			};
		}

		private static string ExtractOutputText(JsonDocument doc)
		{
			if (doc.RootElement.TryGetProperty("output_text", out var outputTextElement))
			{
				var outputText = outputTextElement.GetString();
				if (!string.IsNullOrWhiteSpace(outputText))
					return outputText;
			}

			if (doc.RootElement.TryGetProperty("output", out var outputArray) &&
				outputArray.ValueKind == JsonValueKind.Array)
			{
				foreach (var outputItem in outputArray.EnumerateArray())
				{
					if (outputItem.TryGetProperty("content", out var contentArray) &&
						contentArray.ValueKind == JsonValueKind.Array)
					{
						foreach (var contentItem in contentArray.EnumerateArray())
						{
							if (contentItem.TryGetProperty("text", out var textElement))
							{
								var text = textElement.GetString();
								if (!string.IsNullOrWhiteSpace(text))
									return text;
							}
						}
					}
				}
			}

			throw new Exception("Could not find output text in OpenAI response.");
		}
	}
}