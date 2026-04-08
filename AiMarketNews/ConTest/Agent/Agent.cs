using AiMarketNews.ConTest.LLM;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiMarketNews.ConTest.Agent
{
    public class Agent
    {
        private readonly LlmClient _llm;
        private readonly ToolRegistry _tools;
        private readonly ToolExecutor _executor;

        public Agent(LlmClient llm, ToolRegistry tools)
        {
            _llm = llm;
            _tools = tools;
            _executor = new ToolExecutor(tools);
        }

        public async Task<string> Run(string input)
        {
            var context = new AgentContext();
            context.AddUserMessage(input);

            int maxIterations = 10;

            for (int i = 0; i < maxIterations; i++)
            {
                var response = await _llm.Send(context, _tools.GetToolSchemas());
                var message = response.GetMessage();

                if (message.ToolCalls.Any())
                {
                    foreach (var call in message.ToolCalls)
                    {
                        var result = await _executor.Execute(call);

                        context.AddToolResult(call.Name, result);
                    }

                    continue;
                }

                context.AddAssistantMessage(message.Text);
                return message.Text;
            }

            throw new Exception("Max agent iterations reached");
        }
    }
}
