using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json.Serialization;

namespace RestApiCSharp.Authentication
{
    public class Authenticator : AuthenticatorBase
    {
        record TokenResponse
        {
            [JsonPropertyName("token_type")]
            public string TokenType { get; init; }
            [JsonPropertyName("access_token")]
            public string AccessToken { get; init; }
        }

        readonly string _baseUrl;
        readonly string _clientId;
        readonly string _clientSecret;
        readonly string _scope;

        public Authenticator(string baseUrl, string clientId, string clientSecret, string scope) : base("")
        {
            _baseUrl = baseUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _scope = scope;
        }

        protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
        {
            Token = string.IsNullOrEmpty(Token) ? await GetToken() : Token;
            return new HeaderParameter(KnownHeaders.Authorization, Token);
        }

        async Task<string> GetToken()
        {
            var options = new RestClientOptions(_baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(_clientId, _clientSecret),
            };

            using var client = new RestClient(options);

            var request = new RestRequest("oauth/token")
                .AddParameter("grant_type", "client_credentials")
                .AddParameter("scope", _scope);
            var response = await client.PostAsync<TokenResponse>(request);

            return $"{response!.TokenType} {response!.AccessToken}";
        }
    }
}
