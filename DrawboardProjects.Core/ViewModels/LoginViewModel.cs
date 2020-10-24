using DrawboardProjects.Core.PresentationHints;
using DrawboardProjects.Core.Services;
using DrawboardProjects.Core.Services.Authentication;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmDialogs;
using System;
using System.Threading.Tasks;

namespace DrawboardProjects.Core.ViewModels
{
    /// <summary>
    /// View Model for Login
    /// </summary>
    public class LoginViewModel : NavigationViewModelBase
    {
        IAuthenticationService _authenticationService;

        public LoginViewModel(IAuthenticationService authenticationService,
            IDialogService dialogService,
            IMvxLogProvider logProvider,
            IMvxNavigationService navigationService)
            : base(dialogService, logProvider, navigationService)
        {
            _authenticationService = authenticationService;
        }

        public override void Prepare()
        {
            base.Prepare();

#if DEBUG
            // For Testing.
            // Grab username and password from config and pre-populate.
            try
            {
                var config = new Config.AppConfig();
                Username = config.AuthenticationForTesting.Username;
                Password = config.AuthenticationForTesting.PlainTextPassword;
            }
            catch (Exception e)
            {
                Log.Error(e, $"Error in {nameof(Prepare)} trying to get config");
            }
#endif
        }

        string _username;
        public string Username { get => _username; set => SetProperty(ref _username, value); }
        string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private IMvxAsyncCommand _loginCommand;
        public IMvxAsyncCommand LoginCommand
        {
            get
            {
                _loginCommand = _loginCommand ?? new MvxAsyncCommand(Login);
                return _loginCommand;
            }
        }

        /// <summary>
        /// Complete the login with validation on the username and password.
        /// </summary>
        async Task Login()
        {
            bool showFailure = false;
            string failureMessage = "Something went wrong. Please try logging in again.";

            try
            {
                // Validation 
                Validate();
                if (!IsValid)
                {
                    // Just return and depend on XAML to bind and show errors
                    return;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, $"Validation error in {nameof(Login)}");
                // Validation somehow failed, but lets continue to try logging in.
            }
            try
            {
                IsBusy = true;
                // Authenticate
                var token = await _authenticationService.Login(Username, Password);
                if (token != null && !string.IsNullOrWhiteSpace(token.AccessToken))
                {
                    // Success. Navigate to the next view(Model).
                    await NavigationService.Navigate<ProjectListViewModel>();
                    // Pop the Login view(Model) off the stack
                    await NavigationService.ChangePresentation(new PopBackstackHint());
                }
                else // Failed, but expect an error normally
                {
                    showFailure = true;
                }
            }
            catch (HttpResponseException httpRespEx)
            {
                showFailure = true;
                Log.WarnException($"{nameof(Login)}", httpRespEx);
                if (httpRespEx.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    failureMessage = "Please check your username and password";
                }
                else if (httpRespEx.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    failureMessage = "Please check your account";
                }
                //TODO: could check other status codes
            }
            catch (System.Net.Http.HttpRequestException httpReqEx)
            {
                showFailure = true;
                Log.WarnException($"{nameof(Login)}", httpReqEx);
                failureMessage = "Please check you are connected to the internet.";
            }
            catch (Exception e)
            {
                showFailure = true;
                Log.Error(e, $"{nameof(Login)}");
                failureMessage = "The was an unexpected problem with logging in. Please try again.";
            }
            finally
            {
                IsBusy = false;
                // Clear the password
                Password = null;
            }
            if(showFailure)
            {
                await DialogService.ShowMessageDialogAsync(failureMessage);
            }
        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this.Username))
            {
                this.ValidationErrors[nameof(Username)] = "Username is required.";
            }
            if (string.IsNullOrWhiteSpace(this.Password))
            {
                this.ValidationErrors[nameof(Password)] = "Password is required.";
            }
        }
    }
}
