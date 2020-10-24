using Newtonsoft.Json;
using System;

namespace DrawboardProjects.Core.Models.Token
{
    /// <summary>
    /// An Authentication token acquired after a successful login with the REST API.
    /// </summary>
    public class AuthenticationToken
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("authorizationHeader")]
        public string AuthorizationHeader { get; set; }

        [JsonProperty("expiresOnUtc")]
        public DateTime ExpiresOnUtc { get; set; }

        [JsonProperty("userId")]
        public string UserID { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
}
