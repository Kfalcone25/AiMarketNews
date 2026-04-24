using AiMarketNews.Services.LLM;
using AiMarketNews.Services.NewsService.MockNews;

var newsService = new MockNewsServiceAPI();
var llmService = new LlmService();

var article = newsService.GetTopHeadlines().Result.First();

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







//================================================================================================================
//SECOND HEADLINE CALL
// ======================
var article2 = newsService.GetTopHeadlines().Result[2];

// ======================
// FIRST CALL (ANALYSIS)
// ======================
var initialResponse2 = await llmService.AnalyzeArticleAsync(article2);

var initialResult2 = initialResponse2.Data;
var initialUsage2 = initialResponse2.Usage;

// ======================
// SECOND CALL (CRITIQUE)
// ======================
var improvedResponse2 = await llmService.CritiqueAndImproveAsync(article2, initialResult2);

var improvedResult2 = improvedResponse2.Data;
var improvedUsage2 = improvedResponse2.Usage;

// ======================
// COST CALCULATION
// ======================
double initialCost2 = (initialUsage2.TotalTokens / 1000.0) * costPer1K;
double improvedCost2 = (improvedUsage2.TotalTokens / 1000.0) * costPer1K;

double totalTokens2 = initialUsage2.TotalTokens + improvedUsage2.TotalTokens;
double totalCost2 = initialCost2 + improvedCost2;
















//================================================================================================================
//SECOND HEADLINE OUTPUT
void PrintHeader(string title)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"\n╔══════════════════════════════════════════════════╗");
    Console.WriteLine($"║ {title.PadRight(48)} ║");
    Console.WriteLine($"╚══════════════════════════════════════════════════╝");
    Console.ResetColor();
}

void PrintSection(string title)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\n─── {title} ───────────────────────────────────────");
    Console.ResetColor();
}

void PrintKeyValue(string key, string value)
{
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write($"{key,-15}: ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(value);
    Console.ResetColor();
}

// ======================
// OUTPUT
// ======================

PrintHeader("INITIAL RESULT");

PrintKeyValue("Article", article.Title);
PrintKeyValue("Sector", initialResult.PrimarySector);
PrintKeyValue("Sentiment", initialResult.Sentiment);
PrintKeyValue("Summary", initialResult.MarketImpactSummary);
PrintKeyValue("Companies", string.Join(", ", initialResult.AffectedCompanies));

PrintHeader("IMPROVED RESULT");

PrintKeyValue("Sector", improvedResult.PrimarySector);
PrintKeyValue("Sentiment", improvedResult.Sentiment);
PrintKeyValue("Summary", improvedResult.MarketImpactSummary);
PrintKeyValue("Companies", string.Join(", ", improvedResult.AffectedCompanies));

PrintHeader("TOKEN USAGE");

PrintSection("First Call (Analysis)");
PrintKeyValue("Input Tokens", initialUsage.InputTokens.ToString());
PrintKeyValue("Output Tokens", initialUsage.OutputTokens.ToString());
PrintKeyValue("Total Tokens", initialUsage.TotalTokens.ToString());
PrintKeyValue("Cost", $"${initialCost:F6}");

PrintSection("Second Call (Critique)");
PrintKeyValue("Input Tokens", improvedUsage.InputTokens.ToString());
PrintKeyValue("Output Tokens", improvedUsage.OutputTokens.ToString());
PrintKeyValue("Total Tokens", improvedUsage.TotalTokens.ToString());
PrintKeyValue("Cost", $"${improvedCost:F6}");

PrintSection("TOTAL");
PrintKeyValue("Total Tokens", totalTokens.ToString());

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"\n💰 Total Cost: ${totalCost:F6}");


// =================================================================================


PrintHeader("INITIAL RESULT 2");

PrintKeyValue("Article", article2.Title);
PrintKeyValue("Sector", initialResult2.PrimarySector);
PrintKeyValue("Sentiment", initialResult2.Sentiment);
PrintKeyValue("Summary", initialResult2.MarketImpactSummary);
PrintKeyValue("Companies", string.Join(", ", initialResult2.AffectedCompanies));

PrintHeader("IMPROVED RESULT");

PrintKeyValue("Sector", improvedResult2.PrimarySector);
PrintKeyValue("Sentiment", improvedResult2.Sentiment);
PrintKeyValue("Summary", improvedResult2.MarketImpactSummary);
PrintKeyValue("Companies", string.Join(", ", improvedResult2.AffectedCompanies));

PrintHeader("TOKEN USAGE");

PrintSection("First Call (Analysis)");
PrintKeyValue("Input Tokens", initialUsage2.InputTokens.ToString());
PrintKeyValue("Output Tokens", initialUsage2.OutputTokens.ToString());
PrintKeyValue("Total Tokens", initialUsage2.TotalTokens.ToString());
PrintKeyValue("Cost", $"${initialCost2:F6}");

PrintSection("Second Call (Critique)");
PrintKeyValue("Input Tokens", improvedUsage2.InputTokens.ToString());
PrintKeyValue("Output Tokens", improvedUsage2.OutputTokens.ToString());
PrintKeyValue("Total Tokens", improvedUsage2.TotalTokens.ToString());
PrintKeyValue("Cost", $"${improvedCost:F6}");

PrintSection("TOTAL");
PrintKeyValue("Total Tokens", totalTokens2.ToString());

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"\n💰 Total Cost: ${totalCost2:F6}");
Console.ResetColor();