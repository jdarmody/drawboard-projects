using MvvmCross.ViewModels;

namespace DrawboardProjects.Core.PresentationHints
{
    /// <summary>
    /// A custom <see cref="MvxPresentationHint"/> to clear the navigation back stack.
    /// </summary>
    /// <remarks>Use in View Models with the 
    /// <see cref="MvvmCross.Navigation.IMvxNavigationService.ChangePresentation(MvxPresentationHint, System.Threading.CancellationToken)"/>
    /// </remarks>
    public class ClearBackstackHint : MvxPresentationHint
    {
    }
}
