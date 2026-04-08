using AiMarketNews.ConTest;
using AiMarketNews.ConTest.Agent;
using AiMarketNews.ConTest.LLM;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

class Program2
{
    static async Task Main(string[] args)
    {
        var program = new MockProgram();
        await program.RunMock();
    }
}


public class MockProgram
{
     private readonly IConfiguration _config;
     private readonly string _apiKey;
     private readonly string _baseUrl;

     public MockProgram()
     {
         var builder = new ConfigurationBuilder()
             .SetBasePath("C:\\Users\\Connor\\source\\repos\\AiMarketNews\\AiMarketNews")      //Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
         _config = builder.Build();
         _apiKey = _config["OpenAIKey:ApiKey"];
         _baseUrl = _config["OpenAIKey:BaseUrl"] ?? "https://api.openai.com/v1/responses";
    }

    
    public async Task RunMock()
    {
        var llm = new LlmClient(); // uses your API key from appsettings.json

        var tools = new ToolRegistry();
        tools.Register(new MockMarketTool());

        var agent = new Agent(llm, tools);


        Console.WriteLine("Enter headline:");
        var input = Console.ReadLine();

        var result = await agent.Run(input);

        Console.WriteLine("\n=== FINAL RESULT ===");
        Console.WriteLine(result);
    }
       

}




    
