using Newtonsoft.Json;
using System;

namespace DrawboardProjects.Core.Models
{
    /// <summary>
    /// A <see cref="User"/> that can collaborate on a <see cref="Projects.Project"/>.
    /// </summary>
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("permissions")]
        public string Permissions { get; set; }
        [JsonProperty("companyName")]
        public string CompanyName { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("dateJoined")]
        public string DateJoined { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("userAlias")]
        public string UserAlias { get; set; }
        [JsonProperty("activationId")]
        public Guid ActivationId { get; set; }
        [JsonProperty("isOptInForCommunication")]
        public bool IsOptInForCommunication { get; set; }
        [JsonProperty("accountActivated")]
        public bool AccountActivated { get; set; }
    }
}
