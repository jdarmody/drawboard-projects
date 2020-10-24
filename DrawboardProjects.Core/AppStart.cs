using DrawboardProjects.Core.Models.Token;
using DrawboardProjects.Core.Services.Authentication;
using MvvmCross.Exceptions;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawboardProjects.Core
{
	/// <summary>
	/// Custom app start up responsible for making the decision of which ViewModel to present first.
	/// </summary>
	public class AppStart : MvxAppStart
    {
		private readonly IAuthenticationService _authenticationService;

		public AppStart(IMvxApplication application, IMvxNavigationService navigationService, IAuthenticationService authenticationService)
			: base(application, navigationService)
		{
			_authenticationService = authenticationService;
		}

        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
			try
			{
				if (_authenticationService.IsAuthenticated)
				{
					await NavigationService.Navigate<ViewModels.ProjectListViewModel>();
				}
				else
				{
					await NavigationService.Navigate<ViewModels.LoginViewModel>();
				}
			}
			catch (Exception exception)
			{
				throw exception.MvxWrap("Problem navigating to first ViewModel");
			}
		}
    }
}
