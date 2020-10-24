using DrawboardProjects.Core.Config;
using DrawboardProjects.Core.Models.Projects;
using DrawboardProjects.Core.Services.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DrawboardProjects.Core.Services.Projects
{
    /// <summary>
    /// Service for <see cref="Project"/> related operations.
    /// </summary>
    public class ProjectsService : IProjectsService
    {
        readonly string baseAddress;
        readonly string apiVersion;

        string projectsUriBase => baseAddress + $"api/v{apiVersion}/project/";

        IAuthenticationService authenticationService;
        HttpClient client;

        public ProjectsService(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;

            var config = new AppConfig();
            baseAddress = config.Api.BaseAddress;
            apiVersion = config.Api.Version;

            client = new HttpClient();
            client.BaseAddress = new Uri(projectsUriBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(Constants.MEDIA_TYPE_APPLICATION_JSON));
            // Didn't require the subscription key, but normally would.
            //client.DefaultRequestHeaders.Add(Constants.OCP_APIM_SUBSCRIPTION_KEY, config.Api.SubscriptionKey);
            CheckAuthHeader();
        }

        /// <summary>
        /// Retrieve all the projects for the current authenticated user.
        /// </summary>
        public async Task<IEnumerable<Project>> GetProjects()
        {
            CheckAuthentication();

            //var queryString = System.web HttpUtility.ParseQueryString(string.Empty);
            HttpResponseMessage response = await client.GetAsync($"my?api_key={authenticationService.CurrentToken.AccessToken}");
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var projects = JsonConvert.DeserializeObject<IEnumerable<Project>>(jsonResponse);
                return projects;
            }
            else // Failure
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }

        /// <summary>
        /// Retrieve all the projects for the current authenticated user.
        /// </summary>
        public Task<IEnumerable<Project>> GetProjects(CancellationToken cancellationToken)
        {
            //TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a <see cref="Project"/> Background image
        /// </summary>
        /// <param name="projectId"><see cref="Project.Id"/></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>A <see cref="Stream"/> containing the bitmap</returns>
        public async Task<Stream> GetProjectBackground(string projectId, int width, int height)
        {
            CheckAuthentication();

            // Request parameters
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["base64"] = false.ToString();
            queryString["debug"] = false.ToString();

            var uri = $"{projectId}/background/{width}x{height}?" + queryString;
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                // Expect content type to be bmp (Could check)
                return await response.Content.ReadAsStreamAsync();
            }
            else // Failure
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }

        /// <summary>
        /// Get a <see cref="Project"/> Logo.
        /// </summary>
        /// <param name="projectId"><see cref="Project.Id"/></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>A <see cref="Stream"/> containing the bitmap</returns>
        public async Task<Stream> GetProjectLogo(string projectId, int width, int height)
        {
            CheckAuthentication();

            // Request parameters
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["base64"] = false.ToString();
            queryString["debug"] = false.ToString();

            var uri = $"{projectId}/logo/{width}x{height}?" + queryString;
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                // Expect content type to be bmp (Could check)
                return await response.Content.ReadAsStreamAsync();
            }
            else // Failure
            {
                throw new HttpResponseException(response.StatusCode);
            }
        }

        /// <summary>
        /// Use to ensure the user is authenticated and the authentication header is setup with the current token.
        /// </summary>
        void CheckAuthentication()
        {
            if (authenticationService == null) throw new InvalidOperationException("Authentication Service is missing");
            if (!authenticationService.IsAuthenticated) throw new NotAuthenticatedException();
            CheckAuthHeader();
        }
        /// <summary>
        /// Use to setup the client Authentication header with current <see cref="Models.Token.AuthenticationToken"/>
        /// </summary>
        void CheckAuthHeader()
        {
            //TODO: thread safety if service is used as a singleton
            if (!string.IsNullOrWhiteSpace(authenticationService?.CurrentToken?.AccessToken) &&
                (client.DefaultRequestHeaders.Authorization == null ||
                client.DefaultRequestHeaders.Authorization.Scheme != Constants.AUTHORIZATION_SCHEME ||
                client.DefaultRequestHeaders.Authorization.Parameter != authenticationService.CurrentToken.AccessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.AUTHORIZATION_SCHEME,
                    authenticationService.CurrentToken.AccessToken);
            }
        }
    }
}
