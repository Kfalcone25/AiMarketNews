using AiMarketNews.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using NUnit.Framework;

//https://newsapi.org/docs/endpoints/top-headlines

namespace AiMarketNews.Services.NewsService.MockNews
{
    [TestFixture]
    public class APITest
    {
        [Test]
        public async Task TestNewsAPI()
        {
            var newsService = new MockNewsServiceAPI();
            await newsService.GetTopHeadlines();
        }
    }

    public class MockNewsServiceAPI //: INewsService
    {
        private readonly IConfiguration _config;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly string _model;

        public MockNewsServiceAPI()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _config = builder.Build();
            _apiKey = _config["NewsAPIKey:ApiKey"] ?? string.Empty;
            _baseUrl = _config["NewsAPIKey:BaseUrl"] ?? "https://newsapi.org/v2/top-headlines?country=us&apiKey=";

            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("OpenAI API key not configured in appsettings.json");
        }


        public async Task<List<Article>> GetTopHeadlines()
        {
            var articlesList = new List<Article>();

            //New Headline API Call
            var apiKey = $"{_apiKey}";
            var url = $"{_baseUrl}{_apiKey}";

            using var client = new HttpClient();
            await Task.Delay(500);
            var response = await client.GetStringAsync(url);

            using var doc = JsonDocument.Parse(response);
            var articles = doc.RootElement.GetProperty("articles");

            for (int i = 0; i <= 2; i++)
            {
                foreach (var article in articles.EnumerateArray())
                {
                    var title = article.GetProperty("title").GetString();
                    var source = article.GetProperty("source").GetProperty("name").GetString();
                    var description = article.GetProperty("description").GetString();
                    var published = article.GetProperty("publishedAt").GetString();
                    var urlArt = article.GetProperty("url").GetString();
                    
                    articlesList.Add(new Article
                    {
                        Title = title,
                        Source = source,
                        Summary = description,
                        PublishedAt = DateTime.Parse(published),
                        Url = urlArt
                    });
                }
            }

            return articlesList;
        }




    }
}
