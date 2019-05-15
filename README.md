# Blazor Network Requests
Simple Blazor client side app demonstrating how to make network requests

## To Run
Requires [dotnet core 3.0 preview](https://dotnet.microsoft.com/download/dotnet-core/3.0) installation 

```dotnet run```

---
# Explanation

## Create a service
```c#
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
```

## Register service for DI 
Register service for Dependency Injection in Startup.cs

```c#
public void ConfigureServices(IServiceCollection services)
{
    // ...
    services.AddSingleton<GithubService>();
}
```

## Inject Service into your razor component 
```c#
@inject GithubService githubService

@functions {
    private UsernameFormModel usernameFormModel = new UsernameFormModel();
    private List<GithubRepository> userRepositories = new List<GithubRepository>();

    private async Task HandleValidSubmit() {
        userRepositories = await githubService.FetchUserRepositories(usernameFormModel.Username);
    }
}
```