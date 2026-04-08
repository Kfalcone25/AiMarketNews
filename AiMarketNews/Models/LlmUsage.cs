using System;
using System.Collections.Generic;
using System.Text;

namespace AiMarketNews.Models
{
	public class LlmUsage
	{
		//Tracks API Usage for LLM calls, including input and output token counts, to monitor costs and optimize prompts.
		public int InputTokens { get; set; }
		public int OutputTokens { get; set; }
		public int TotalTokens => InputTokens + OutputTokens;
	}
}
