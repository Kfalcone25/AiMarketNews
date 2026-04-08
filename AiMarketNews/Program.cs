using AiMarketNews.Services.LLM;
using AiMarketNews.Services.NewsService.MockNews;

var newsService = new MockNewsService();var llmService = new LlmService();

var article = newsService.GetTopHeadlines().First();

var response = await llmService.AnalyzeArticleAsync(article);
//Extracting the analysis result
var result = response.Data;
//Token Usage Tracking
var usage = response.Usage;

//Cost Estimation
double costPer1K = 0.00015; // approx for gpt-4.1-mini
double cost = (usage.TotalTokens / 1000.0) * costPer1K;

//Result
Console.WriteLine("=== RESULT ===");
Console.WriteLine($"Sector: {result.PrimarySector}");
Console.WriteLine($"Sentiment: {result.Sentiment}");
Console.WriteLine($"Summary: {result.MarketImpactSummary}");
Console.WriteLine($"Companies: {string.Join(", ", result.AffectedCompanies)}");

//API Token Usage Tracking
Console.WriteLine("=== TOKEN USAGE ===");
Console.WriteLine($"Input Tokens: {usage.InputTokens}");
Console.WriteLine($"Output Tokens: {usage.OutputTokens}");
Console.WriteLine($"Total Tokens: {usage.TotalTokens}");

//Cost Estimation
Console.WriteLine("=== Estimated Cost ===");
Console.WriteLine($"Estimated Cost: ${cost:F6}");