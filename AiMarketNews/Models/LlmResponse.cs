using System;
using System.Collections.Generic;
using System.Text;

namespace AiMarketNews.Models
{
	//Wrapper Return Object For API Response
	public class LlmResponse<T>
	{
		public T Data { get; set; } = default!;
		public LlmUsage Usage { get; set; } = new();
	}
}
