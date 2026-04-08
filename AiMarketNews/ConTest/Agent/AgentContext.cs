using System;
using System.Collections.Generic;
using System.Text;

namespace AiMarketNews.ConTest.Agent
{
    public class AgentContext
    {
        public List<object> Messages { get; } = new();

        public void AddUserMessage(string text)
        {
            Messages.Add(new { role = "user", content = text });
        }

        public void AddAssistantMessage(string text)
        {
            Messages.Add(new { role = "assistant", content = text });
        }

        public void AddToolResult(string toolName, string result)
        {
            Messages.Add(new
            {
                role = "tool",
                name = toolName,
                content = result
            });
        }
    }
}
