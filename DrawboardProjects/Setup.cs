using DrawboardProjects.Core.PresentationHints;
using MvvmCross.Logging;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Presenters;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.ViewModels;
using Serilog;

namespace DrawboardProjects
{
    /// <summary>
    /// Custom Platform Setup used to setup logging and customise the View Presenter.
    /// </summary>
    /// <typeparam name="TApplication"></typeparam>
    public class Setup<TApplication> : MvxWindowsSetup<TApplication>
        where TApplication : class, IMvxApplication, new()
    {
        /// <summary>
        /// Adds our <see cref="BackStackHintHandler"/> to the <see cref="MvxWindowsViewPresenter"/>.
        /// </summary>
        /// <param name="rootFrame"></param>
        /// <returns></returns>
        protected override IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            var viewPresenter = base.CreateViewPresenter(rootFrame);
            var backStackHandler = new BackStackHintHandler(rootFrame);
            viewPresenter.AddPresentationHintHandler<ClearBackstackHint>(backStackHandler.HandleClearBackstackHint);
            viewPresenter.AddPresentationHintHandler<PopBackstackHint>(backStackHandler.HandlePopBackstackHint);
            return viewPresenter;
        }

        public override MvxLogProviderType GetDefaultLogProviderType() => MvxLogProviderType.Serilog;

        protected override IMvxLogProvider CreateLogProvider()
        {
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        //TODO: .WriteTo.File()
                        .CreateLogger();
            return base.CreateLogProvider();
        }
    }
}
