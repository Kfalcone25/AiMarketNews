using AiMarketNews.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiMarketNews.Services.NewsService.MockNews
{
	public class MockNewsService : INewsService
	{
		public List<Article> GetTopHeadlines()
		{
			return new List<Article>
			{
				new Article
				{
					Title = "Nvidia shares surge as AI demand drives record revenue",
					Source = "Reuters",
					Summary = "Nvidia reported record-breaking quarterly revenue driven by strong demand for AI chips, exceeding analyst expectations and boosting investor confidence.",
					PublishedAt = DateTime.Now.AddHours(-2),
					Url = "https://www.reuters.com/technology/nvidia-shares-surge-ai-demand-2024"
				},
				new Article
				{
					Title = "AI tools continue changing developer workflows",
					Source = "Mock Tech Daily",
					Summary = "Teams are experimenting with AI-assisted workflows to speed up development and testing.",
					PublishedAt = DateTime.Now.AddHours(-1),
					Url = "https://example.com/article-2"
				}
			};
		}
	}
}
