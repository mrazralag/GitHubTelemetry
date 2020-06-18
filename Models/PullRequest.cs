using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GitHubTelemetry.Models
{
    public class PullRequest
    {
        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("merged_at")]
        public DateTime? Date { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        public List<File> Files { get; set; }

        public string Group { get; set; }

        public PullRequest()
        {
            Files = new List<File>();
        }
    }

    public class User
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class File
    {
        [JsonProperty("filename")]
        public string Name { get; set; }
    }
}
