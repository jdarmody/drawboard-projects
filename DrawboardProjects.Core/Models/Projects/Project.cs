using Newtonsoft.Json;
using System;

namespace DrawboardProjects.Core.Models.Projects
{
    /// <summary>
    /// A Drawboard Project
    /// </summary>
    public class Project
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; } //TODO: Could be an enum, but no documentation on what the values mean
        [JsonProperty("permissions")]
        public string Permissions { get; set; }
        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }
        [JsonProperty("createdDateUtc")]
        public DateTime? CreatedDateUtc { get; set; }
        [JsonProperty("deletedOn")]
        public DateTime? DeletedOn { get; set; }
        [JsonProperty("owner")]
        public User Owner { get; set; }
        [JsonProperty("drawingCount")]
        public int DrawingCount { get; set; }
        [JsonProperty("documentCount")]
        public int DocumentCount { get; set; }
        [JsonProperty("userCount")]
        public int UserCount { get; set; }
        [JsonProperty("issuesCount")]
        public int IssuesCount { get; set; }
        [JsonProperty("organizationId")]
        public string OrganizationId { get; set; }
    }
}
