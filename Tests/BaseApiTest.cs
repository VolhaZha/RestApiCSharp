using NLog;
using RestApiCSharp.Clients;
using RestApiCSharp.ConstantsTestingGeneral;
using RestSharp;

namespace RestApiCSharp.Tests
{
    public abstract class BaseApiTest
    {
        protected RestClient Client { get; private set; }
        protected ApiClient ApiClientInstance { get; private set; }
        protected static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        protected BaseApiTest()
        {
            Logger.Info("Initializing BaseApiTest");

            ApiClient.Initialize(
                baseUrl: ConstantsTesting.BaseUrl,
                clientId: ConstantsTesting.ClientId,
                clientSecret: ConstantsTesting.ClientSecret
            );

            ApiClientInstance = ApiClient.GetInstance();

            Logger.Info("ApiClient initialized successfully");
        }
    }
}
