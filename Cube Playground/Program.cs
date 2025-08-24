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
            //GenerateForm();
        }


        private static void GenerateForm()
        {
            List<FormElement> formContent = IntegrationService.GenerateFormContent(typeof(RegSection));

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

                ComplianceModel complianceModel = new()
                {
                    FormId = -2,
                    DomainName = "PVT Domain",
                    DisplayRationaleContent = false,
                    ComplianceTiers = new()
                };

                Console.WriteLine("SECTIONS");

                int order = 0;

                foreach (RegSection section in regSectionSearchResponse.Where(x => x.ParentId == null))
                {
                    PrintSectionInfo(section, regSectionSearchResponse);
                    ComplianceTier childTier = new ComplianceTier()
                    {
                        Name = section.Title,
                        Order = order++,
                        Description = section.Content,
                        ChildTiers = new(),
                        Obligations = new()
                    };
                    complianceModel.ComplianceTiers.Add(childTier);
                    AddTierChildren(section, childTier, regSectionSearchResponse);
                }

                string jsonComplianceModel = JsonConvert.SerializeObject(complianceModel, Formatting.None, new JsonSerializerSettings());

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Check your parameters! {ex.Message}");
            }

        }

        private static void AddTierChildren(RegSection section, ComplianceTier tier, List<RegSection> allSections)
        {
            int order = 0;
            foreach (RegSection childSection in allSections.Where(x => x.ParentId == section.SectionId))
            {
                ComplianceTier childTier = new ComplianceTier()
                {
                    Name = childSection.Title,
                    Order = order++,
                    Description = childSection.Content,
                    Obligations = new(),
                    ChildTiers = new()
                };
                tier.ChildTiers.Add(childTier);
                AddTierChildren(childSection, childTier, allSections);
            }
        }

        private static void PrintSectionInfo(RegSection section, List<RegSection> allSections)
        {
            string title = new string(' ', section.Level * 2) + section.Title;
            Console.WriteLine(title);
            Console.WriteLine(new string('-', title.Length));
            Console.WriteLine(new string(' ', section.Level * 2) + IntegrationService.StripHtml(section.Content));
            Console.WriteLine();

            foreach (RegSection childSection in allSections.Where(x => x.ParentId == section.SectionId))
            {
                PrintSectionInfo(childSection, allSections);
            }
        }
    }
}
