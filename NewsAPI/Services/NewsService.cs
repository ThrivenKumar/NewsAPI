using Microsoft.Extensions.Options;
using NewsAPI.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace NewsAPI.Services
{
    public interface INewsService
    {
        Task<NewsApiResponse> GetNewsAsync();
        Task<Dictionary<string, int>> FindCommonWordsAsync();
    }
    public class NewsService : INewsService
    {
        private readonly HttpClient _httpClient;
        private readonly NewsApiSettings _newsApiSettings;

        public NewsService(HttpClient httpClient, IOptions<NewsApiSettings> newsApiSettings)
        {
            _httpClient = httpClient;
            _newsApiSettings = newsApiSettings.Value;
            // Assuming API key is in appsettings.json
        }

        public async Task<NewsApiResponse> GetNewsAsync() {
            var url = $"{_newsApiSettings.BaseUrl}/top-headlines?country=us";
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            // Set the Authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _newsApiSettings.ApiKey);

            // Set the User-Agent header (IMPORTANT: Customize this!)
            request.Headers.UserAgent.ParseAdd("Chrome/1.0");
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"News API request failed with status code {response.StatusCode}. Details: {errorContent}");
            }
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Raw JSON Response: {content}");
            var newsApiResponse = JsonSerializer.Deserialize<NewsApiResponse>(content);
            return newsApiResponse;
        }

        public async Task<Dictionary<string,int>> FindCommonWordsAsync(){
            var newsApiResponse = await GetNewsAsync();
            var commonWords = newsApiResponse.Articles
                .SelectMany(article => article.Title.Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries))
                .GroupBy(word => word.ToLower())
                .OrderByDescending(group => group.Count())
                .ToDictionary(group => group.Key, group => group.Count());
            return commonWords;
        }
    }
}
