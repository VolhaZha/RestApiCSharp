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
        private RestClient _client;

        private ApiClient (string baseUrl, string clientId, string clientSecret)
        {
            _baseUrl = baseUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _client = CreateClientWithScope("");
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

        public void SetClientScope(string scope)
        {
            _client = CreateClientWithScope(scope);
        }

        public RestResponse ExpandZipCodes(string scope, List<string> zipCodes)
        {
            SetClientScope(scope);

            var request = new RestRequest("/zip-codes/expand", Method.Post);

            request.AddJsonBody(zipCodes);

            return _client.Post(request);
        }

        public RestResponse GetZipCodes(string scope)
        {
            SetClientScope(scope);

            var request = new RestRequest("/zip-codes", Method.Get);
            return _client.Get(request);
        }

        public RestResponse CreateUsers (string scope, User user)
        {
            SetClientScope(scope);

            var request = new RestRequest("/users", Method.Post);

            request.AddJsonBody(user);

            var response = _client.Post(request);

            if (!response.IsSuccessful)
            {
                Console.WriteLine($"Error: {response.StatusCode}, Response Body: {response.Content}");
            }

            return response;

        }

        public RestResponse GetUsers(string scope)
        {
            SetClientScope(scope);

            var request = new RestRequest("/users", Method.Get);
            return _client.Get(request);
        }
    }
}
