using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http.Json;

namespace DallETest
{
    public class OpenAIHttpService : IOpenAIProxy
    {
        private readonly HttpClient _httpClient;
        private readonly string _subscriptionId;
        private readonly string _apiKey;

        public OpenAIHttpService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAi:ApiKey"];
            _subscriptionId = configuration["OpenAi:SubscriptionId"];

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration["OpenAi:Url"])
            };
        }

        public async Task<GenerateImageResponse> GenerateImages(GenerateImageRequest prompt, CancellationToken cancellation = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/v1/images/generations");

            var jsonRequest = JsonSerializer.Serialize(prompt, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            request.Content = new StringContent(jsonRequest);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            request.Headers.TryAddWithoutValidation("OpenAI-Organization", _subscriptionId);

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellation);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadFromJsonAsync<GenerateImageResponse>(cancellationToken: cancellation);
            return jsonResponse;
        }

        public async Task<byte[]> DownloadImage(string url)
        {
            var buffer = await _httpClient.GetByteArrayAsync(url);
            return buffer;
        }
    }
}
