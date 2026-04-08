using System;
using System.Collections.Generic;
using System.Text;

namespace AiMarketNews.ConTest.Agent
{
    public class AgentMessage
    {
        public string Text { get; set; }
        public List<ToolCall> ToolCalls { get; set; } = new();
    }
}
