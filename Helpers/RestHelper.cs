using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace GitHubTelemetry.Helpers
{
    public class RestHelper
    {
        public static string BaseURL { get; set; }

        public static T Execute<T>(RestRequest request, HttpBasicAuthenticator basicAuthenticator)
        {
            return ConvertResponse<T>(Execute(request, basicAuthenticator));
        }

        public static IRestResponse Execute(RestRequest request, HttpBasicAuthenticator basicAuthenticator)
        {
            var client = GetClient(basicAuthenticator);

            client.AddHandler("application/json", NewtonsoftJsonSerializer.Default);

            return client.Execute(request);
        }

        private static RestClient GetClient(HttpBasicAuthenticator basicAuthenticator)
        {
            return new RestClient(BaseURL)
            {
                Authenticator = basicAuthenticator
            };
        }

        public static T ConvertResponse<T>(IRestResponse response)
        {
            T result = default(T);

            result = JsonConvert.DeserializeObject<T>(response.Content);

            return result;
        }

        /// <summary>
        /// This method will generate essential configurations to initiate a restful API call
        /// </summary>
        /// <returns>Returns a base request object with all the essential headers</returns>
        public static RestRequest GetBaseRequest()
        {
            var request = new RestRequest();
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;

            request.AddHeader("content-type", "application/json");

            return request;
        }
    }
}
