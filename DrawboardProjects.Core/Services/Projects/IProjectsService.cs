using DrawboardProjects.Core.Models.Projects;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DrawboardProjects.Core.Services.Projects
{
    /// <summary>
    /// Service for <see cref="Project"/> related operations.
    /// </summary>
    public interface IProjectsService
    {
        /// <summary>
        /// Retrieve all the projects for the current authenticated user.
        /// </summary>
        Task<IEnumerable<Project>> GetProjects();
        /// <summary>
        /// Retrieve all the projects for the current authenticated user.
        /// </summary>
        Task<IEnumerable<Project>> GetProjects(CancellationToken cancellationToken);
        /// <summary>
        /// Get a <see cref="Project"/> Logo.
        /// </summary>
        /// <param name="projectId"><see cref="Project.Id"/></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>A <see cref="Stream"/> containing the bitmap</returns>
        Task<Stream> GetProjectLogo(string projectId, int width, int height);
        /// <summary>
        /// Get a <see cref="Project"/> Background image
        /// </summary>
        /// <param name="projectId"><see cref="Project.Id"/></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>A <see cref="Stream"/> containing the bitmap</returns>
        Task<Stream> GetProjectBackground(string projectId, int width, int height);
    }
}
