using AiMarketNews.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiMarketNews.Services.NewsService
{
	public interface INewsService
	{
		List<Article> GetTopHeadlines();
	}
}
