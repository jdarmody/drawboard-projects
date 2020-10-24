using DrawboardProjects.Core.Models.Projects;
using DrawboardProjects.Core.Services.Projects;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace DrawboardProjects.Core.ViewModels
{
    /// <summary>
    /// Represents a single project to be used in <see cref="ProjectListViewModel"/>.
    /// </summary>
    public class ProjectListItemViewModel : ViewModelBase
    {
        private Project _project;
        private IProjectsService _projectsService;
        private IMvxLogProvider _logProvider;
        private IMvxLog _log;
        private IMvxLog Log => _log ?? (_log = _logProvider.GetLogFor(GetType()));

        public ProjectListItemViewModel(IMvxLogProvider logProvider, IProjectsService projectsService, Project project)
        {
            _logProvider = logProvider;
            _projectsService = projectsService;
            _project = project;

            LoadLogo();
        }

        public string Name => _project.Name;
        public string Description => _project.Description;

        private BitmapImage _logo;
        public BitmapImage Logo { get => _logo; set => SetProperty(ref _logo, value); }

        /// <summary>
        /// Asynchronously retrieve the project logo image from the server.
        /// </summary>
        Task LoadLogo()
        {
            return Task.Run(async () =>
            {
                try
                {
                    int width = 100, height = 100; //TODO: Could have these public for the View to use
                    using (var stream = await _projectsService.GetProjectLogo(_project.Id, width, height))
                    {
                        // Create a .NET memory stream.
                        using (var memStream = new MemoryStream())
                        {
                            // Convert the stream to the memory stream, because a memory stream supports seeking.
                            await stream.CopyToAsync(memStream);
                            // Set the start position.
                            memStream.Position = 0;
                            // UI Thread needed
                            await InvokeOnMainThreadAsync(() =>
                            {
                                using (var ras = memStream.AsRandomAccessStream())
                                {
                                    // Create a new bitmap image.
                                    var bitmap = new BitmapImage();
                                    // Set the bitmap source to the stream, which is converted to a IRandomAccessStream.
                                    bitmap.SetSource(ras);
                                    // Set the Logo property
                                    Logo = bitmap;
                                }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Warn(ex, $"A problem occurred with loading the logo for project {_project?.Name} ({_project?.Id})");
                }
            });
        }
    }
}
