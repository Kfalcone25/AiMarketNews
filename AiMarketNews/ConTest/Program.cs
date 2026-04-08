using AiMarketNews.ConTest.Agent;
using AiMarketNews.ConTest.LLM;
using AiMarketNews.ConTest.Tools;
using System;
using System.Collections.Generic;
using System.Text;




var llm = new LlmClient();

var tools = new ToolRegistry();
tools.Register(new GetSectorTool());

var agent = new Agent(llm, tools);

Console.WriteLine("Enter headline:");
var input = Console.ReadLine();

var result = await agent.Run(input);

Console.WriteLine("\n=== RESULT ===");
Console.WriteLine(result);


