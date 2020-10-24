using MvvmCross.ViewModels;

namespace DrawboardProjects.Core.PresentationHints
{
    /// <summary>
    /// A custom <see cref="MvxPresentationHint"/> to pop the previous view in the navigation back stack.
    /// </summary>
    /// <remarks>Use in View Models with the 
    /// <see cref="MvvmCross.Navigation.IMvxNavigationService.ChangePresentation(MvxPresentationHint, System.Threading.CancellationToken)"/>
    /// </remarks>
    public class PopBackstackHint : MvxPresentationHint
    {
    }
}
