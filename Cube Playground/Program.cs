using Cube_Playground.Models;
using Cube_Playground.RAP;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cube_Playground
{
    internal class Program
    {
        private static readonly string username = "apiusertoken@arctic-intelligence.com";
        private static readonly string password = "m7jdgFA^P:Up";
        private static readonly string client_id = "CubeAPIClient";
        private static readonly string client_secret = "1a6)pUTrGY94kXm";

        static async Task Main(string[] args)
        {
            await Cube();
        }

        private static void GenerateForm()
        {
            List<FormElement> formContent = IntegrationService.GenerateFormContent(typeof(RegBook));

            string jsonFormContent = JsonConvert.SerializeObject(formContent, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });



        }

        private static async Task Cube()
        {
            try
            {
                Console.WriteLine("Obtaining Access Token");
                await CubeService.GetAuthenticationToken(username, password, client_id, client_secret);
                Console.WriteLine("Access Token obtained successfully.");
                Console.WriteLine();

                Console.WriteLine("Loading Books");
                List<RegBook> regBookSearchResponse = await CubeService.RegBookSearch(new RegBookSearchRequest() { BookPublishStatus = "Published" });
                Console.WriteLine($"Found {regBookSearchResponse.Count} books.");
                Console.WriteLine();
                RegBook myBook = regBookSearchResponse.First();
                Console.WriteLine($"First Book Title: {myBook.Title}");
                Console.WriteLine($"First Book Source Id: {myBook.SourceId}");

                List<RegSection> regSectionSearchResponse = await CubeService.RegSectionSearch(myBook.SourceId, myBook.VersionOrdinal);
                Console.WriteLine();

                Console.WriteLine("SECTIONS");
                foreach (RegSection section in regSectionSearchResponse.Where(x => x.ParentId == null))
                {
                    PrintSectionInfo(section, regSectionSearchResponse);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Check your parameters! {ex.Message}");
            }

        }

        private static void PrintSectionInfo(RegSection section, List<RegSection> allSections)
        {
            Console.WriteLine(new string(' ', section.Level * 2) + section.Title);

            foreach (RegSection childSection in allSections.Where(x => x.ParentId == section.SectionId))
            {
                PrintSectionInfo(childSection, allSections);
            }
        }
    }
}
