using RestApiCSharp.ConstantsTestingGeneral;
using RestSharp;
using System.Net;

namespace RestApiCSharp.Tests
{
    public class Test1 : BaseApiTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Order(1)]
        public void AddOneZipCode_Return201()
        {
            var zipCodes = new List<string> { "ozoz" };

            RestResponse response = ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test, Order(2)]
        public void AddOneZipCode_ZipCodeAdded()
        {
            RestResponse response = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            Assert.That(response.Content, Does.Contain("ozoz"));
        }

        [Test, Order(3)]
        public void AddSeveralZipCodes_Return201()
        {
            var zipCodes = new List<string> { "ozoz1", "ozoz2", "ozoz3" };

            RestResponse response = ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test, Order(4)]
        public void AddSeveralZipCodes_ZipCodesAdded()
        {
            var zipCodes = new List<string> { "ozoz1", "ozoz2", "ozoz3" };

            RestResponse response = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            foreach (var expected in zipCodes)
            {
                Assert.That(response.Content, Does.Contain(expected));
            }
        }

        [Test, Order(5)]
        public void GetZipCodes_Return200()
        {
            RestResponse response = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test, Order(6)]
        public void GetZipCodes_GetAllAvailableZipCodes()
        {
            RestResponse response = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            var expectedStrings = new[] { "ozoz", "ozoz1", "ozoz2", "ozoz3" };

            foreach (var expected in expectedStrings)
            {
                Assert.That(response.Content, Does.Contain(expected));
            }
        }

        [Test, Order(7)]
        public void AddDuplicateZipCodes_Return201()
        {
            var zipCodes = new List<string> { "ozoz1", "ozoz2", "ozoz4" };

            RestResponse response = ApiClientInstance.ExpandZipCodes(ConstantsTesting.WriteScope, zipCodes);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test, Order(8)]
        public void AddDuplicateZipCodes_ZipCodesAdded()
        {
            RestResponse response = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            Assert.That(response.Content, Does.Contain("ozoz4"));
        }

        [Test, Order(9)]
        public void AddDuplicateZipCodes_NoDuplicatesCreated()
        {
            RestResponse response = ApiClientInstance.GetZipCodes(ConstantsTesting.ReadScope);

            var zipCodes = System.Text.Json.JsonSerializer.Deserialize<List<string>>(response.Content);

            Assert.That(zipCodes.Distinct().Count(), Is.EqualTo(zipCodes.Count));
        }
    }
}
