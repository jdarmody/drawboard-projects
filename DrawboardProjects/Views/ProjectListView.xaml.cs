using MvvmCross.Platforms.Uap.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace DrawboardProjects.Views
{
    // Example of strict assignment to the View Model
    //[MvxViewFor(typeof(ProjectListViewModel))]
    public sealed partial class ProjectListView : MvxWindowsPage
    {
        public ProjectListView()
        {
            this.InitializeComponent();
        }
    }
}
