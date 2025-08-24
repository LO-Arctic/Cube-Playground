using Newtonsoft.Json;

namespace Cube_Playground.Models
{
    internal class AuthenticationToken : CubeResponse
    {
        public static AuthenticationToken Create(string jsonStr)
        {
            return JsonConvert.DeserializeObject<AuthenticationToken>(jsonStr);
        }

        public string? access_token { get; set; }
        public string? expires_in { get; set; }
        public string? token_type { get; set; }
        public string? scope { get; set; }
    }
}
