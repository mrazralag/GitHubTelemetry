using GitHubTelemetry.Helpers;
using GitHubTelemetry.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace GitHubTelemetry.Services
{
    /// <summary>
    /// Service provider for GitHub data points
    /// </summary>
    public class GitHubService
    {
        private IConfiguration configuration;

        public GitHubService(IConfiguration injectedConfiguration)
        {
            configuration = injectedConfiguration;
        }

        /// <summary>
        /// Gets pull requests that have been merged along with files updated, and user data
        /// </summary>
        /// <returns>List of Pull Request objects</returns>
        public List<PullRequest> GetMergedPullRequests()
        {
            List<PullRequest> pullRequests = new List<PullRequest>();

            Console.WriteLine($"Getting all pull requests");
            pullRequests = GetHubData<List<PullRequest>>();
            pullRequests = pullRequests.Where(x => x.Date.HasValue && x.Date.Value < DateTime.Today.Date).ToList();

            Console.WriteLine($"Getting all file and user data");
            foreach (PullRequest pr in pullRequests)
            {
                List<File> pullRequestFiles = GetHubData<List<File>>($"pulls/{pr.Number}/files");
                pr.Files.AddRange(pullRequestFiles.Where(x => !Regex.IsMatch(x.Name, "([^\\s]+(\\.(?i)(png|svg))$)")));

                User pullRequestUser = GetHubData<User>($"users/{pr.User.Login}", true);
                pr.Group = Regex.IsMatch(pullRequestUser.Email ?? "", "^[A-Za-z0-9._%+-]+@microsoft.com$") ? "MSFT" : "External";
            }
            
            return pullRequests;
        }

        /// <summary>
        /// Rest API communication
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourceURL"></param>
        /// <param name="useBaseAPI"></param>
        /// <returns></returns>
        private T GetHubData<T>(string resourceURL = "pulls?state=closed", bool useBaseAPI = false)
        {
            RestHelper.BaseURL = useBaseAPI ? configuration["git_base_url"] : configuration["git_repo_url"];

            RestRequest restRequest = RestHelper.GetBaseRequest();
            restRequest.Resource = resourceURL;
            restRequest.Method = Method.GET;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            return RestHelper.Execute<T>(restRequest, new HttpBasicAuthenticator(configuration["git_username"], configuration["git_access_token"]));
        }
    }
}
