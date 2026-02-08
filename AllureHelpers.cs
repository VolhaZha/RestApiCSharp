using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using Newtonsoft.Json;
using RestSharp;

namespace RestApiCSharp
{
    public static class AllureHelpers
    {
        [AllureStep("Attach Request Payload to Report")]
        public static void AttachRequestContentToReport(RestRequest request)
        {
            var fileName = "requestContent.json";
            var requestParameters = request.Parameters
                .Where(p => p.Type == ParameterType.RequestBody || p.Type == ParameterType.GetOrPost)
                .Select(p => p.Value)
                .FirstOrDefault();

            if (requestParameters != null)
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(requestParameters));
                AllureApi.AddAttachment($"Request Payload to {request.Resource}", "application/json",
                    Path.Combine(TestContext.CurrentContext.TestDirectory, fileName));
            }
        }

        [AllureStep("Attach Response to Report")]
        public static void AttachResponseContentToReport(RestResponse response)
        {
            var fileName = "responseContent.json";

            File.WriteAllText(fileName, response.Content);
            AllureApi.AddAttachment("Response Content", "application/json",
                Path.Combine(TestContext.CurrentContext.TestDirectory, fileName));
        }
    }
}
