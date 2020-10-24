using MvvmCross.IoC;
using MvvmDialogs;
using System.Threading.Tasks;

namespace DrawboardProjects.Core
{
    /// <summary>
    /// Responsible for registering custom objects on the IoC container and starting ViewModels and business logic.
    /// </summary>
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        /// <summary>
        /// Breaking change in v6: This method is called on a background thread. Use
        /// Startup for any UI bound actions
        /// </summary>
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            MvxIoCProvider.Instance.RegisterSingleton<IDialogService>(new DialogService());

            RegisterCustomAppStart<AppStart>();
        }

        /// <summary>
        /// Do any UI bound startup actions here
        /// </summary>
        public override async Task Startup()
        {
            await base.Startup();
        }

        /// <summary>
        /// If the application is restarted (eg primary activity on Android 
        /// can be restarted) this method will be called before Startup
        /// is called again
        /// </summary>
        public override void Reset()
        {
            base.Reset();
        }
    }
}
