namespace DrawboardProjects.Core.Config.Sections
{
    /// <summary>
    /// Unsecure storage of username and password. <b>Use only for testing purposes.</b>
    /// </summary>
    public class AuthenticationForTesting
    {
        public string Username { get; set; }
        public string PlainTextPassword { get; set; }
    }
}
