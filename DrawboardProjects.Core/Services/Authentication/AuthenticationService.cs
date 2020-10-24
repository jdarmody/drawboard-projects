using DrawboardProjects.Core.Config;
using DrawboardProjects.Core.Models.Token;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DrawboardProjects.Core.Services.Authentication
{
    /// <summary>
    /// Service for authenticating the user.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        readonly string baseAddress;
        readonly string apiVersion;

        string authUriBase => baseAddress + $"api/v{apiVersion}/auth/";

        HttpClient client;

        public AuthenticationService()
        {
            var config = new AppConfig();
            baseAddress = config.Api.BaseAddress;
            apiVersion = config.Api.Version;

            client = new HttpClient();
            client.BaseAddress = new Uri(authUriBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(Constants.MEDIA_TYPE_APPLICATION_JSON));
            // Didn't require the subscription key, but normally would.
            //client.DefaultRequestHeaders.Add(Constants.OCP_APIM_SUBSCRIPTION_KEY, config.Api.SubscriptionKey);
        }

        public AuthenticationToken CurrentToken { get; private set; }

        /// <summary>
        /// Returns True if Authenticated and the <see cref="CurrentToken"/> hasn't expired.
        /// </summary>
        public bool IsAuthenticated => CurrentToken != null &&
            !string.IsNullOrWhiteSpace(CurrentToken.AccessToken) &&
            DateTime.Now < CurrentToken.ExpiresOnUtc.ToLocalTime();

        /// <summary>
        /// Asynchronous login. Recommend to use <see cref="Login(string, string, CancellationToken)"/> in order
        /// to control the amount of time to wait or to allow the user to cancel.
        /// </summary>
        /// <exception cref="HttpResponseException">If a response is returned from an unsuccessful request.</exception>
        /// <returns>An <see cref="AuthenticationToken\"/> if successful, which is also stored in <see cref="CurrentToken"/>.</returns>
        public async Task<AuthenticationToken> Login(string username, string password)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException(nameof(username));

            string json = JsonConvert.SerializeObject(new
            {
                username,
                password
            });
            var content = new StringContent(json, Encoding.UTF8, Constants.MEDIA_TYPE_APPLICATION_JSON);
            HttpResponseMessage response = await client.PostAsync("login", content);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                CurrentToken = JsonConvert.DeserializeObject<AuthenticationToken>(jsonResponse);
                return CurrentToken;
            }
            else // Failure
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }

        /// <summary>
        /// Asynchronous login  with <paramref name="cancellationToken"/>.
        /// </summary>
        /// <exception cref="HttpResponseException">If a response is returned from an unsuccessful request.</exception>
        /// <returns>An <see cref="AuthenticationToken\"/> if successful, which is also stored in <see cref="CurrentToken"/>.</returns>
        public Task<AuthenticationToken> Login(string username, string password, CancellationToken cancellationToken)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException(nameof(username));

            //TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs out the user and clears the <see cref="CurrentToken"/>.
        /// </summary>
        public Task Logout()
        {
            if (CurrentToken != null)
            {
                // Clear the current token for now
                CurrentToken = null;
                //TODO: save previous user/token
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Refreshes the <see cref="CurrentToken"/> with the server to allow continued use of the API.
        /// The <see cref="CurrentToken"/> will be updated.
        /// </summary>
        /// <returns>The refreshed <see cref="AuthenticationToken"/>.</returns>
        public Task<AuthenticationToken> Refresh()
        {
            //TODO: use the current token
            throw new NotImplementedException();
        }

        /// <summary>
        /// Refreshes the <see cref="CurrentToken"/> with the server to allow continued use of the API.
        /// The <see cref="CurrentToken"/> will be updated.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The refreshed <see cref="AuthenticationToken"/>.</returns>
        public Task<AuthenticationToken> Refresh(CancellationToken cancellationToken)
        {
            //TODO: use the current token
            throw new NotImplementedException();
        }
    }
}
