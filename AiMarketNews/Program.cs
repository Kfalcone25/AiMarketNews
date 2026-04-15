using AiMarketNews.Services.LLM;
using AiMarketNews.Services.NewsService.MockNews;

var newsService = new MockNewsService();
var llmService = new LlmService();

var article = newsService.GetTopHeadlines().First();

// ======================
// FIRST CALL (ANALYSIS)
// ======================
var initialResponse = await llmService.AnalyzeArticleAsync(article);

var initialResult = initialResponse.Data;
var initialUsage = initialResponse.Usage;

// ======================
// SECOND CALL (CRITIQUE)
// ======================
var improvedResponse = await llmService.CritiqueAndImproveAsync(article, initialResult);

var improvedResult = improvedResponse.Data;
var improvedUsage = improvedResponse.Usage;

// ======================
// COST CALCULATION
// ======================
double costPer1K = 0.00015;

double initialCost = (initialUsage.TotalTokens / 1000.0) * costPer1K;
double improvedCost = (improvedUsage.TotalTokens / 1000.0) * costPer1K;

double totalTokens = initialUsage.TotalTokens + improvedUsage.TotalTokens;
double totalCost = initialCost + improvedCost;

// ======================
// OUTPUT RESULTS
// ======================

Console.WriteLine("=== INITIAL RESULT ===");
Console.WriteLine($"Sector: {initialResult.PrimarySector}");
Console.WriteLine($"Sentiment: {initialResult.Sentiment}");
Console.WriteLine($"Summary: {initialResult.MarketImpactSummary}");
Console.WriteLine($"Companies: {string.Join(", ", initialResult.AffectedCompanies)}");

Console.WriteLine("\n=== IMPROVED RESULT ===");
Console.WriteLine($"Sector: {improvedResult.PrimarySector}");
Console.WriteLine($"Sentiment: {improvedResult.Sentiment}");
Console.WriteLine($"Summary: {improvedResult.MarketImpactSummary}");
Console.WriteLine($"Companies: {string.Join(", ", improvedResult.AffectedCompanies)}");

// ======================
// TOKEN USAGE
// ======================

Console.WriteLine("\n=== TOKEN USAGE ===");

Console.WriteLine("\nFirst Call (Analysis):");
Console.WriteLine($"Input Tokens: {initialUsage.InputTokens}");
Console.WriteLine($"Output Tokens: {initialUsage.OutputTokens}");
Console.WriteLine($"Total Tokens: {initialUsage.TotalTokens}");
Console.WriteLine($"Cost: ${initialCost:F6}");

Console.WriteLine("\nSecond Call (Critique):");
Console.WriteLine($"Input Tokens: {improvedUsage.InputTokens}");
Console.WriteLine($"Output Tokens: {improvedUsage.OutputTokens}");
Console.WriteLine($"Total Tokens: {improvedUsage.TotalTokens}");
Console.WriteLine($"Cost: ${improvedCost:F6}");

Console.WriteLine("\n=== TOTAL ===");
Console.WriteLine($"Total Tokens: {totalTokens}");
Console.WriteLine($"Total Cost: ${totalCost:F6}");