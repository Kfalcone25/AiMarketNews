using AiMarketNews.ConTest.Agent;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AiMarketNews.ConTest.LLM
{
    public class LlmResponse
    {
        private readonly JsonDocument _doc;

        public LlmResponse(string json)
        {
            _doc = JsonDocument.Parse(json);
        }

        public AgentMessage GetMessage()
        {
            var content = _doc.RootElement
                .GetProperty("output")[0]
                .GetProperty("content")[0];

            var message = new AgentMessage();

            if (content.TryGetProperty("text", out var text))
                message.Text = text.GetString();

            if (content.TryGetProperty("tool_calls", out var calls))
            {
                foreach (var c in calls.EnumerateArray())
                {
                    message.ToolCalls.Add(new ToolCall
                    {
                        Name = c.GetProperty("name").GetString(),
                        Arguments = c.GetProperty("arguments").GetRawText()
                    });
                }
            }

            return message;
        }
    }
}
