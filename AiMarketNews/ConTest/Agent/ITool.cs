using System;
using System.Collections.Generic;
using System.Text;

namespace AiMarketNews.ConTest.Agent
{
    public interface ITool
    {
        string Name { get; }
        object Schema { get; }
        Task<string> Execute(string argumentsJson);
    }
}
