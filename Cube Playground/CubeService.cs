using Cube_Playground.Enums;
using Cube_Playground.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Cube_Playground
{
    internal static class CubeService
    {
        private static HttpClient httpClient;
        public static AuthenticationToken? authenticationToken;

        public static Dictionary<string, string> stripHtml = new Dictionary<string, string>() { { "stripHtml", "true" } };

        static CubeService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
            httpClient.DefaultRequestHeaders.Add("x-page-current", "1");
            httpClient.DefaultRequestHeaders.Add("x-page-length", "50");
            httpClient.DefaultRequestHeaders.Add("api-version", "2.0");
        }

        public static async Task<bool> GetAuthenticationToken(string userName, string password, string clientId, string clientSecret)
        {
            FormUrlEncodedContent formContent = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                        { "grant_type", "password" },
                        { "username", userName },
                        { "password", password },
                        { "audience", "RegConnectApi" },
                        { "scope", "cubeapi" },
                        { "client_id", clientId },
                        { "client_secret", clientSecret }
                });

            HttpResponseMessage httpResponse = await httpClient.PostAsync(CubeEndpoints.TokenUrl, formContent);

            string responseContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"{httpResponse.StatusCode}: {httpResponse.ReasonPhrase}. {responseContent}");
            }

            if (string.IsNullOrEmpty(responseContent))
            {
                throw new Exception("Empty response from API Token endpoint");

            }
            authenticationToken = AuthenticationToken.Create(responseContent);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authenticationToken.access_token}");

            return true;
        }

        public static async Task<List<RegBook>> RegBookSearch(RegBookSearchRequest request) => await CubeService.JsonAPIPost<List<RegBook>>(CubeEndpoints.RegBookSearch, request, null, stripHtml) ?? new();

        public static async Task<List<RegSection>> RegSectionSearch(Guid sourceId, int versionOrdinal) => await CubeService.JsonAPIGet<List<RegSection>>(CubeEndpoints.RegSection, new List<string> { sourceId.ToString(), versionOrdinal.ToString() }, stripHtml) ?? new();

        public static async Task<T?> JsonAPIPost<T>(string url, CubeRequest request, List<string>? urlParameters = null, Dictionary<string, string>? queryParams = null) where T : new()
        {
            return await JsonAPICall<T>(url, urlParameters, queryParams, async (parametrizedUrl) =>
            {
                return await httpClient.PostAsJsonAsync(parametrizedUrl, request);
            }) ?? new();
        }

        public static async Task<T?> JsonAPIGet<T>(string url, List<string>? urlParameters = null, Dictionary<string, string>? queryParams = null) where T : new()
        {
            return await JsonAPICall<T>(url, urlParameters, queryParams, async (parametrizedUrl) =>
            {
                return await httpClient.GetAsync(parametrizedUrl);
            }) ?? new();
        }

        private static async Task<T?> JsonAPICall<T>(string url, List<string>? urlParams, Dictionary<string, string>? queryParams, Func<string, Task<HttpResponseMessage>> serverCall) where T : new()
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (authenticationToken == null || string.IsNullOrEmpty(authenticationToken.access_token))
            {
                throw new Exception("Authentication token is not set.");
            }

            string parametrizedUrl = FormatUrl(url, urlParams, queryParams);

            HttpResponseMessage httpResponse = await serverCall(parametrizedUrl);

            string responseBody = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"{httpResponse.StatusCode}: {httpResponse.ReasonPhrase}. {responseBody}");
            }

            if (string.IsNullOrEmpty(responseBody))
            {
                return new();
            }

            return JsonConvert.DeserializeObject<T>(responseBody) ?? new();
        }


        private static string FormatUrl(string url, List<string>? urlParameters, Dictionary<string, string>? queryParams)
        {
            string parametrizedUrl = urlParameters != null ? string.Format(url, urlParameters.ToArray()) : url;

            if (queryParams != null && queryParams.Count > 0)
            {
                parametrizedUrl += "?" + string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            }

            return parametrizedUrl;
        }
    }
}
