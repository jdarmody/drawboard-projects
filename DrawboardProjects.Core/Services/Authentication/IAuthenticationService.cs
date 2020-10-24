using DrawboardProjects.Core.Models.Token;
using System.Threading;
using System.Threading.Tasks;

namespace DrawboardProjects.Core.Services.Authentication
{
    /// <summary>
    /// Service for authenticating the user.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Storage of the current token acquired after a successful login.
        /// </summary>
        /// <remarks>This will be NULL if the user is not currently authenticated.</remarks>
        AuthenticationToken CurrentToken { get; }
        /// <summary>
        /// Returns True if Authenticated and the <see cref="CurrentToken"/> hasn't expired.
        /// </summary>
        bool IsAuthenticated { get; }
        /// <summary>
        /// Asynchronous login. Recommend to use <see cref="Login(string, string, CancellationToken)"/> in order
        /// to control the amount of time to wait or to allow the user to cancel.
        /// </summary>
        /// <exception cref="HttpResponseException">If a response is returned from an unsuccessful request.</exception>
        /// <returns>An <see cref="AuthenticationToken\"/> if successful, which is also stored in <see cref="CurrentToken"/>.</returns>
        Task<AuthenticationToken> Login(string username, string password);
        /// <summary>
        /// Asynchronous login  with <paramref name="cancellationToken"/>.
        /// </summary>
        /// <exception cref="HttpResponseException">If a response is returned from an unsuccessful request.</exception>
        /// <returns>An <see cref="AuthenticationToken\"/> if successful, which is also stored in <see cref="CurrentToken"/>.</returns>
        Task<AuthenticationToken> Login(string username, string password, CancellationToken cancellationToken);
        /// <summary>
        /// Logs out the user and clears the <see cref="CurrentToken"/>.
        /// </summary>
        Task Logout();
        /// <summary>
        /// Refreshes the <see cref="CurrentToken"/> with the server to allow continued use of the API.
        /// The <see cref="CurrentToken"/> will be updated.
        /// </summary>
        /// <returns>The refreshed <see cref="AuthenticationToken"/>.</returns>
        Task<AuthenticationToken> Refresh();
        /// <summary>
        /// Refreshes the <see cref="CurrentToken"/> with the server to allow continued use of the API.
        /// The <see cref="CurrentToken"/> will be updated.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The refreshed <see cref="AuthenticationToken"/>.</returns>
        Task<AuthenticationToken> Refresh(CancellationToken cancellationToken);
    }
}
