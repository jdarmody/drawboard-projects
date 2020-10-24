using DrawboardProjects.Core.PresentationHints;
using DrawboardProjects.Core.Services;
using DrawboardProjects.Core.Services.Authentication;
using DrawboardProjects.Core.Services.Projects;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmDialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace DrawboardProjects.Core.ViewModels
{
    /// <summary>
    /// View Model for accessing a user's projects.
    /// </summary>
    public class ProjectListViewModel : NavigationViewModelBase
    {
        private IProjectsService _projectsService;
        IAuthenticationService _authenticationService;

        public ProjectListViewModel(IAuthenticationService authenticationService,
            IProjectsService projectsService,
            IDialogService dialogService,
            IMvxLogProvider logProvider,
            IMvxNavigationService navigationService)
            : base(dialogService, logProvider, navigationService)
        {
            _authenticationService = authenticationService;
            _projectsService = projectsService;
        }

        /// <summary>
        /// Called after the native navigation has take place
        /// </summary>
        /// <returns></returns>
        public override async Task Initialize()
        {
            // After navigation is complete. Let's load the projects
            await RefreshProjects();
        }

        /// <summary>
        /// Displays the current user First and Last name.
        /// </summary>
        public string UserDisplayName => _authenticationService.CurrentToken == null ? string.Empty :
            $"{_authenticationService.CurrentToken?.FirstName} " +
            $"{_authenticationService.CurrentToken?.LastName}";

        public string Title => $"Projects ({(Projects?.Count > 0 ? Projects.Count : 0)})";

        /// <summary>
        /// Retrieve the projects with the server and sets <see cref="Projects"/>
        /// </summary>
        /// <returns></returns>
        async Task RefreshProjects()
        {
            try
            {
                IsBusy = true;
                // Get the projects from the server
                var projects = await _projectsService.GetProjects();
                if (projects != null)
                {
                    // Success. Update Projects using ProjectListItemViewModel instances.
                    Projects = new MvxObservableCollection<ProjectListItemViewModel>(projects
                            .Select(p => new ProjectListItemViewModel(LogProvider, _projectsService, p)));
                }
            }
            catch (NotAuthenticatedException nae)
            {
                // Must have had the login session expire. Ideally this is managed in a central place.
                Log.Warn(nae, "Couldn't refresh projects as not authenticated");
                //TODO: Attempt to call _authenticationService.Refresh();

                // For now signing out...
                await Logout();
                // Alert the user, show the login view and clear the stack.
                await DialogService.ShowMessageDialogAsync("You have been logged out.");
            }
            catch (HttpResponseException httpRespEx)
            {
                Log.Warn(httpRespEx, "Failed to get projects");
                if(httpRespEx.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    httpRespEx.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    // Unauthorized. Log them out.
                    await Logout();
                    // Alert the user, show the login view and clear the stack.
                    await DialogService.ShowMessageDialogAsync("You have been logged out.");
                }
                else // Some other problem
                {
                    await DialogService.ShowMessageDialogAsync("Failed to get your projects.");
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to get projects");
                await DialogService.ShowMessageDialogAsync("An unexpected problem occurred while getting your projects.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private MvxObservableCollection<ProjectListItemViewModel> _projects;
        public MvxObservableCollection<ProjectListItemViewModel> Projects 
        { 
            get => _projects; 
            set => SetProperty(ref _projects, value, () =>
            {
                RaisePropertyChanged(nameof(Title));
                RaisePropertyChanged(nameof(NoProjectsMessageVisible));
            });
        }

        private ProjectListItemViewModel _selectedProject;
        public ProjectListItemViewModel SelectedProject
        {
            get => _selectedProject;
            set => SetProperty(ref _selectedProject, value, async () =>
            {
                if (value != null)
                {
                    // Simple feedback to the user after selecting a project
                    await DialogService.ShowMessageDialogAsync($"You have select project {value.Name}");
                    //TODO: Instead, navigate to a Project view.
                }
            });
        }

        //TODO: could create a converter to be used in the XAML instead of having this property.
        public bool NoProjectsMessageVisible => Projects == null || Projects.Count < 1;

        private IMvxAsyncCommand _logoutCommand;
        public IMvxAsyncCommand LogoutCommand
        {
            get
            {
                _logoutCommand = _logoutCommand ?? new MvxAsyncCommand(UserTriggeredLogout);
                return _logoutCommand;
            }
        }

        /// <summary>
        /// Logout triggered by the user.
        /// </summary>
        async Task UserTriggeredLogout()
        {
            try
            {
                var logoutOption = new UICommand { Label = "Logout" };
                var result = await DialogService.ShowMessageDialogAsync("Are you sure you wish to logout?", null,
                    new[]
                    {
                        logoutOption,
                        new UICommand { Label = "Close" }
                    }, 1, 1);
                if (result == logoutOption)
                {
                    await Logout();
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Unexpected problem with logging out.");
            }
        }

        /// <summary>
        /// Logout and show the login view(model).
        /// </summary>
        async Task Logout()
        {
            try
            {
                await _authenticationService.Logout();
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(RefreshProjects)}: Failed call logout after handling NotAuthenticatedException");
            }
            await NavigationService.Navigate<LoginViewModel>();
            // Clear back stack
            await NavigationService.ChangePresentation(new ClearBackstackHint());
        }
    }
}
