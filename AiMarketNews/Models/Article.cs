using System;
using System.Collections.Generic;
using System.Text;

namespace AiMarketNews.Models
{
	public class Article
	{
		public string Title { get; set; } = string.Empty;
		public string Source { get; set; } = string.Empty;
		public string Summary { get; set; } = string.Empty;
		public DateTime PublishedAt { get; set; }
		public string Url { get; set; } = string.Empty;
	}
}
