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
        private static Dictionary<string, string> stripHtml = new Dictionary<string, string>() { { "stripHtml", "true" } };

        static CubeService()
        {
            httpClient = new HttpClient();
        }

        public static async Task GetAuthenticationToken(string userName, string password, string clientId, string clientSecret)
        {
            FormUrlEncodedContent formContent = CreateConnectFormContent(userName, password, clientId, clientSecret);

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
        }

        public static async Task<List<RegBook>> RegBookSearch(RegBookSearchRequest request) => await PagedJsonAPICall<RegBook>(HttpMethod.Post, CubeEndpoints.RegBookSearch, null, stripHtml, request) ?? new();

        public static async Task<List<RegSection>> RegSectionSearch(Guid sourceId, int versionOrdinal) => await PagedJsonAPICall<RegSection>(HttpMethod.Get, CubeEndpoints.RegSection, new List<string> { sourceId.ToString(), versionOrdinal.ToString() }, stripHtml) ?? new();

        private static async Task<List<T>?> PagedJsonAPICall<T>(HttpMethod method, string url, List<string>? urlParams, Dictionary<string, string>? queryParams, CubeModel? request = null) where T : CubeModel
        {
            int xPageCurrent = 1;
            int xPageCount = 1;

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (authenticationToken == null || string.IsNullOrEmpty(authenticationToken.access_token))
            {
                throw new Exception("Authentication token is not set.");
            }

            List<T> result = new();
            do
            {
                HttpRequestMessage httpRequest = CreateHttpRequest(method, FormatUrl(url, urlParams, queryParams), request);
                httpRequest.Headers.Add(CubeHttpHeader.XPageCurrent, (xPageCurrent++).ToString());

                HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);
                string responseBody = await httpResponse.Content.ReadAsStringAsync();
                xPageCount = int.Parse(GetResponseHeaderValue(httpResponse, CubeHttpHeader.XPageCount, "1"));

                if (string.IsNullOrEmpty(responseBody))
                {
                    return result;
                }

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception($"{httpResponse.StatusCode}: {httpResponse.ReasonPhrase}. {responseBody}");
                }

                List<T> currentPageRecords = JsonConvert.DeserializeObject<List<T>>(responseBody) ?? new();

                result.AddRange(currentPageRecords);

            } while (xPageCurrent <= xPageCount);

            return result;
        }

        private static string GetResponseHeaderValue(HttpResponseMessage httpResponse, string headerName, string defaultValue = "")
        {
            return httpResponse.Headers.Contains(headerName) ? httpResponse.Headers.GetValues(headerName).First() : defaultValue;
        }

        private static HttpRequestMessage CreateHttpRequest(HttpMethod method, string url, CubeModel request)
        {
            HttpRequestMessage httpRequest = new()
            {
                RequestUri = new Uri(url),
                Method = method,
                Content = request != null ? JsonContent.Create(request) : null
            };

            httpRequest.Headers.Add(CubeHttpHeader.Accept, CubeHttpHeaderValue.JsonContentType);
            httpRequest.Headers.Add(CubeHttpHeader.Accept, CubeHttpHeaderValue.TextContentType);
            httpRequest.Headers.Add(CubeHttpHeader.XPageLength, CubeHttpHeaderValue.XPageLength);
            httpRequest.Headers.Add(CubeHttpHeader.ApiVersion, CubeHttpHeaderValue.ApiVersion);
            httpRequest.Headers.Add(CubeHttpHeader.Authorization, string.Format(CubeHttpHeaderValue.Authorization, authenticationToken?.access_token));
            return httpRequest;
        }

        private static FormUrlEncodedContent CreateConnectFormContent(string userName, string password, string clientId, string clientSecret)
        {
            return new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                        { CubeFormParameter.GrantType, CubeFormParameterValue.GrantTypePassword},
                        { CubeFormParameter.Username, userName },
                        { CubeFormParameter.Password, password },
                        { CubeFormParameter.Audience, CubeFormParameterValue.AudienceRegConnect },
                        { CubeFormParameter.Scope, CubeFormParameterValue.ScopeCubeApi },
                        { CubeFormParameter.ClientId, clientId },
                        { CubeFormParameter.ClientSecret, clientSecret }
                });
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
