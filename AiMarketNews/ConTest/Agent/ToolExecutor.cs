using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AiMarketNews.ConTest.Agent
{
    public class ToolExecutor
    {
        private readonly ToolRegistry _registry;

        public ToolExecutor(ToolRegistry registry)
        {
            _registry = registry;
        }

        public async Task<string> Execute(ToolCall call)
        {
            try
            {
                var tool = _registry.Get(call.Name);

                if (tool == null)
                    return Error($"Tool '{call.Name}' not found");

                // Optional: validate JSON early
                if (string.IsNullOrWhiteSpace(call.Arguments))
                    return Error("Missing arguments");

                // Execute tool
                var result = await tool.Execute(call.Arguments);

                return result ?? "{}";
            }
            catch (JsonException ex)
            {
                return Error($"Invalid JSON arguments: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Error($"Execution failed: {ex.Message}");
            }
        }

        private string Error(string message)
        {
            return JsonSerializer.Serialize(new
            {
                error = message
            });
        }
    }
}
