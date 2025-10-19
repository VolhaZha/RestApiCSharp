using RestSharp;
using RestApiCSharp.Authentication;

namespace RestApiCSharp.Clients
{
    public class ApiClient

    {
        private static ApiClient? _instance;

        private readonly string _baseUrl;
        private readonly string _clientId;
        private readonly string _clientSecret;

        private ApiClient (string baseUrl, string clientId, string clientSecret)
        {
            _baseUrl = baseUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public static void Initialize(string baseUrl, string clientId, string clientSecret)
        {
            if (_instance == null)
                _instance = new ApiClient(baseUrl, clientId, clientSecret);
        }

        public static ApiClient GetInstance ()
        {
            if (_instance == null)
                throw new InvalidOperationException("ApiClient not initialized.");
            return _instance;
        }

        public RestClient CreateClientWithScope(string scope)
        {
            return new RestClient(new RestClientOptions(_baseUrl)
            {
                Authenticator = new Authenticator(_baseUrl, _clientId, _clientSecret, scope)
            });
        }

        public static RestClient GetClientWithScope(string scope)
        {
            var client = ApiClient.GetInstance();
            return client.CreateClientWithScope(scope);
        }
    }
}
