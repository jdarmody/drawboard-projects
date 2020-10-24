using DrawboardProjects.Core.Models.Token;
using DrawboardProjects.Core.Services.Authentication;
using DrawboardProjects.UnitTests.Mocks;
using Moq;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Core;
using MvvmCross.Navigation;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.Tests;
using MvvmCross.Views;
using MvvmDialogs;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;

namespace DrawboardProjects.UnitTests.ViewModels
{
    /// <summary>
    /// Base class for View Model Tests.
    /// </summary>
    public abstract class ViewModelTest : MvxIoCSupportingTest
    {
        protected const string VALID_USERNAME = "valid";
        protected const string VALID_PASSWORD = "valid";
        protected readonly AuthenticationToken VALID_TOKEN = new AuthenticationToken
        {
            AccessToken = "valid token",
            AuthorizationHeader = "",
            ExpiresOnUtc = DateTime.UtcNow.AddHours(1),
            FirstName = "Test",
            LastName = "Time",
            RefreshToken = "token for refreshing",
            UserID = Guid.NewGuid().ToString()
        };

        protected AuthenticationToken CurrentToken { get; private set; }
        protected Mock<IAuthenticationService> MockAuthenticationService { get; private set; }
        protected MockDispatcher MockDispatcher { get; private set; }

        protected override void AdditionalSetup()
        {
            //MockDispatcher = new MockDispatcher();
            //Ioc.RegisterSingleton<IMvxViewDispatcher>(MockDispatcher);
            var dispatcher = new MvxWindowsMainThreadDispatcher(Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher);
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);

            Ioc.RegisterSingleton<IDialogService>(new MockDialogService());

            // for navigation parsing
            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());

            var navService = new Mock<IMvxNavigationService>();
            Ioc.RegisterSingleton<IMvxNavigationService>(navService.Object);

            // to allow View Model commands to work in unit tests
            MvxSingletonCache.Instance.Settings.AlwaysRaiseInpcOnUserInterfaceThread = false;
            var helper = new MvxUnitTestCommandHelper();
            Ioc.RegisterSingleton<IMvxCommandHelper>(helper);

            SetupMockAuthenticationService();
        }

        /// <summary>
        /// Use to actually run actions on the UI thread
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected IAsyncAction ExecuteOnUIThreadAsync(DispatchedHandler action)
        {
            return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, action);
        }

        /// <summary>
        /// Simple Mock for the <see cref="IAuthenticationService"/>
        /// </summary>
        protected virtual void SetupMockAuthenticationService()
        {
            MockAuthenticationService = new Mock<IAuthenticationService>();
            // Setup the CurrentToken property
            MockAuthenticationService.Setup(svc => svc.CurrentToken)
                .Returns(() => CurrentToken);
            // Setup the IsAuthenticate property
            MockAuthenticationService.Setup(svc => svc.IsAuthenticated)
                .Returns(() => !string.IsNullOrEmpty(CurrentToken?.AccessToken));
            // Setup the Login for a valid usernamd and password
            MockAuthenticationService.Setup(svc => svc.Login(VALID_USERNAME, VALID_USERNAME))
                .Returns(Task.FromResult(VALID_TOKEN))
                .Callback(() => { CurrentToken = VALID_TOKEN; });
            // Setup a successul Logout
            MockAuthenticationService.Setup(svc => svc.Logout())
                .Callback(() => { CurrentToken = null; });
            // Register as a Singleton for Dependecy Injection
            Ioc.RegisterSingleton<IAuthenticationService>(MockAuthenticationService.Object);
        }
    }
}
