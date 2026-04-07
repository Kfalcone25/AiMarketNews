using System;
using System.Collections.Generic;
using System.Text;

namespace AiMarketNews.ConTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var llm = new LlmService();

            Console.WriteLine("Enter a news headline:");
            var headline = Console.ReadLine();

            var result = await llm.ClassifyHeadline(headline);

            Console.WriteLine("\n=== Classification ===");
            Console.WriteLine($"Primary: {result.PrimarySector}");
            Console.WriteLine($"Secondary: {string.Join(", ", result.SecondarySectors)}");
            Console.WriteLine($"Sentiment: {result.Sentiment}");
            Console.WriteLine($"Confidence: {result.Confidence}");
            Console.WriteLine($"Reasoning: {result.Reasoning}");
        }
    }
}



/*
public class SectorMapper
{
    public Dictionary<string, string> EntityToSector = new()
    {
        { "NASA", "Aerospace & Defense" },
        { "Oil", "Energy" },
        { "AI", "Technology" }
    };

    public string Map(string entity)
    {
        return EntityToSector.ContainsKey(entity)
            ? EntityToSector[entity]
            : "Unknown";
    }
}
*/
