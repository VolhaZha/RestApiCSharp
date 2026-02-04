using Allure.NUnit;
using Allure.NUnit.Attributes;
using RestApiCSharp.ConstantsTestingGeneral;
using RestSharp;
using System.Net;

namespace RestApiCSharp.Tests
{
    [AllureNUnit]
    public class ZipCodeTests : BaseApiTest
    {
        [SetUp]
        public void Setup()
        {
            var zipCodes = new List<string> { "oz", "oz1", "oz2", "oz3" };

            RestResponse response = ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);
        }

        [Test]
        public void AddOneZipCode_Return201()
        {
            var zipCodes = new List<string> { "ozoz" };

            RestResponse response = ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created),
                $"Status code 201 not returned." +
                $"Expected status code 201 (Created), but got {response.StatusCode}. " +
                $"Response content: {response.Content}");
        }

        [Test]
        public void AddOneZipCode_ZipCodeAdded()
        {
            var zipCodes = new List<string> { "ozoz" };

            RestResponse response = ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);
            RestResponse response2 = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            Assert.That(response2.Content, Does.Contain("ozoz"),
                $"The zip code 'ozoz' was not added. Response content: {response2.Content}");
        }

        [Test]
        public void AddSeveralZipCodes_Return201()
        {
            var zipCodes = new List<string> { "ozoz1", "ozoz2", "ozoz3" };

            RestResponse response = ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created),
                "Status code 201 not returned." +
                $"Expected status code 201 (Created), but got {response.StatusCode}. " +
                $"Response content: {response.Content}");
        }

        [Test]
        public void AddSeveralZipCodes_ZipCodesAdded()
        {
            var zipCodes = new List<string> { "ozoz1", "ozoz2", "ozoz3" };

            RestResponse response = ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);
            RestResponse response2 = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            foreach (var expected in zipCodes)
            {
                Assert.That(response2.Content, Does.Contain(expected),
                    $"The zip codes '{zipCodes}' were not added." +
                    $"Response content: {response2.Content}");
            }
        }

        [Test]
        [AllureIssue("BUG_GetZip_1")]
        public void GetZipCodes_Return200()
        {
            RestResponse response = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
                "Status code 200 not returned." +
                $"Expected status code 200 (OK), but got {response.StatusCode}. " +
                $"Response content: {response.Content}");
        }

        [Test]
        public void GetZipCodes_GetAllAvailableZipCodes()
        {
            RestResponse response = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            var expectedStrings = new[] { "oz", "oz1", "oz2", "oz3" };

            foreach (var expected in expectedStrings)
            {
                Assert.That(response.Content, Does.Contain(expected),
                $"Not all available zip codes found. Response content: {response.Content}");
            }
        }

        [Test]
        public void AddDuplicateZipCodes_Return201()
        {
            var zipCodes = new List<string> { "oz1", "oz2" };

            RestResponse response = ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created),
                "Status code 201 not returned." +
                $"Expected status code 201 (Created), but got {response.StatusCode}. " +
                $"Response content: {response.Content}");
        }

        [Test]
        public void AddDuplicateZipCodes_ZipCodesAdded()
        {
            var zipCodes = new List<string> { "oz3" };

            RestResponse response = ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);
            RestResponse response2 = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            Assert.That(response2.Content, Does.Contain("oz3"),
                $"The zip code 'oz3' was not added. Response content: {response2.Content}");
        }

        [Test]
        [AllureIssue("BUG_AddDuplicateZip_1")]
        public void AddDuplicateZipCodes_NoDuplicatesCreated()
        {
            var zipCodes = new List<string> { "oz1", "oz2" };

            RestResponse response = ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);
            RestResponse response2 = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            var zipCodes2 = System.Text.Json.JsonSerializer.Deserialize<List<string>>(response2.Content);

            Assert.That(zipCodes2.Distinct().Count(), Is.EqualTo(zipCodes2.Count),
                $"Zip code list contains duplicates. Response content: {response2.Content}");
        }
    }
}
