namespace Cube_Playground.Enums
{
    internal static class CubeEndpoints
    {

        public static string TokenUrl = "https://uat-id.gocube.global/connect/token";
        public static string ApiConnectBase = "https://uat-connect.gocube.global/api/";
        public static string RegBookSearch = ApiConnectBase + "RegBook/Search";
        public static string RegSection = ApiConnectBase + "RegSection/regbook/{0}/{1}";
    }
}
