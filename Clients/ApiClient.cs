using RestApiCSharp.Authentication;
using RestApiCSharp.Models;
using RestSharp;

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

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User object cannot be null.");
            }

            request.AddJsonBody(user);

            try
            {
                var response = _client.Post(request);

                if (!response.IsSuccessful)
                {
                    throw new HttpRequestException(
                        $"Request failed with status code {response.StatusCode}. Response content: {response.Content}");
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request failed with exception: {ex.Message}");

                return new RestResponse
                {
                    Content = ex.Message
                };
            }
        }

        public void CreateUsersList(string scope, List<User> users)
        {
            foreach (var user in users)
            {
                CreateUsers(scope, user); 
            }
        }

        public RestResponse GetUsers(string scope, List<(string name, string value)>? parameters = null)
        {
            SetClientScope(scope);

            var request = new RestRequest("/users", Method.Get);

            AddParameters(request, parameters);

            return _client.Get(request);
        }

        public RestRequest AddParameters(RestRequest request, List<(string name, string value)>? parameters = null)
        {
            if (parameters == null || parameters.Count == 0) return request;

            foreach (var parameter in parameters)
            {
                request.AddQueryParameter(parameter.name, parameter.value);
            }

            return request;
        }

        public RestResponse UpdateUsersPatch(string scope, UserUpdate user)
        {
            SetClientScope(scope);

            var request = new RestRequest("/users", Method.Patch);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User object cannot be null.");
            }

            request.AddJsonBody(user);

            try
            {
                var response = _client.Patch(request);

                if (!response.IsSuccessful)
                {
                    throw new HttpRequestException(
                        $"Request failed with status code {response.StatusCode}. Response content: {response.Content}");
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request failed with exception: {ex.Message}");

                return new RestResponse
                {
                    Content = ex.Message
                };
            }
        }

        public RestResponse UpdateUsersPut(string scope, UserUpdate user)
        {
            SetClientScope(scope);

            var request = new RestRequest("/users", Method.Put);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User object cannot be null.");
            }

            request.AddJsonBody(user);

            try
            {
                var response = _client.Put(request);

                if (!response.IsSuccessful)
                {
                    throw new HttpRequestException(
                        $"Request failed with status code {response.StatusCode}. Response content: {response.Content}");
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request failed with exception: {ex.Message}");

                return new RestResponse
                {
                    Content = ex.Message
                };
            }
        }

        public RestResponse DeleteUser(string scope, User user)
        {
            SetClientScope(scope);

            var request = new RestRequest("/users", Method.Delete);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User object cannot be null.");
            }

            request.AddJsonBody(user);

            try
            {
                var response = _client.Delete(request);

                if (!response.IsSuccessful)
                {
                    throw new HttpRequestException(
                        $"Request failed with status code {response.StatusCode}. Response content: {response.Content}");
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request failed with exception: {ex.Message}");

                return new RestResponse
                {
                    Content = ex.Message
                };
            }
        }
    }
}
