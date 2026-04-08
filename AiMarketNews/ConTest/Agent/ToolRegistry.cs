using System;
using System.Collections.Generic;
using System.Text;

namespace AiMarketNews.ConTest.Agent
{
    public class ToolRegistry
    {
        private readonly Dictionary<string, ITool> _tools = new();

        public void Register(ITool tool)
        {
            _tools[tool.Name] = tool;
        }

        public ITool Get(string name)
        {
            return _tools.TryGetValue(name, out var tool) ? tool : null;
        }

        public IEnumerable<object> GetToolSchemas()
        {
            return _tools.Values.Select(t => t.Schema);
        }
    }
}
