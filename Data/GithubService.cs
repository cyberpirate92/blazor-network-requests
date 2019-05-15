using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GithubRepositories.Data
{
    public class GithubService {
        private IHttpClientFactory httpClientFactory;
        private static string GITHUB_API_URL = "https://api.github.com";

        public GithubService(IHttpClientFactory clientFactory) {
            httpClientFactory = clientFactory;
        }

        public async Task<List<GithubRepository>> FetchUserRepositories(string username) {
            var repositories = new List<GithubRepository>();
            if (!string.IsNullOrEmpty(username)) {
                var httpClient = httpClientFactory.CreateClient();
                string reposUrl = GetUserReposUrl(username);
                Console.WriteLine(reposUrl);
                httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
                var apiResponse = await httpClient.GetAsync(reposUrl);
                if (apiResponse.IsSuccessStatusCode) {
                    string json = await apiResponse.Content.ReadAsStringAsync();
                    repositories.AddRange(JsonConvert.DeserializeObject<List<GithubRepository>>(json));
                }
            }
            return repositories;
        }

        private static string GetUserReposUrl(string username) {
            return $"{GITHUB_API_URL}/users/{username}/repos";
        }
    }
}