using RestApiCSharp.Clients;
using RestSharp;
using RestApiCSharp.ConstantsTestingGeneral;

namespace RestApiCSharp.Tests
{
    public abstract class BaseApiTest
    {
        protected RestClient Client { get; private set; }
        protected ApiClient ApiClientInstance { get; private set; }

        protected BaseApiTest()
        {
            ApiClient.Initialize(
                baseUrl: ConstantsTesting.BaseUrl,
                clientId: ConstantsTesting.ClientId,
                clientSecret: ConstantsTesting.ClientSecret
            );

            ApiClientInstance = ApiClient.GetInstance();
        }
    }
}
