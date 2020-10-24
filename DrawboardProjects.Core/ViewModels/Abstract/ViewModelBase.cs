using DrawboardProjects.Core.Common;
using MvvmCross.ViewModels;

namespace DrawboardProjects.Core.ViewModels
{
    /// <summary>
    /// Base for View Models not requiring navigation.
    /// </summary>
    public abstract class ViewModelBase : MvxViewModel
    {
        //TODO: private IMvxLog _log;

        protected ViewModelBase() : base()
        {
            ValidationErrors = new ValidationErrors();
        }

        private bool _isBusy;
        /// <summary>
        /// Use to indicate if the View Model is busy performaing a task.
        /// </summary>
        public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }

        /// <summary>
        /// Used to manage validation error messages.
        /// </summary>
        /// <remarks>Set validation errors in <see cref="ValidateSelf"/>.</remarks>
        public ValidationErrors ValidationErrors { get; }

        /// <summary>
        /// Returns True if there are no validation errors.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Run this to perform validation on the view model.
        /// </summary>
        public void Validate()
        {
            this.ValidationErrors.Clear();
            this.ValidateSelf();
            this.IsValid = this.ValidationErrors.IsValid;
            this.RaisePropertyChanged(nameof(IsValid));
            this.RaisePropertyChanged(nameof(ValidationErrors));
        }

        /// <summary>
        /// Override to add validation to your View Model.
        /// </summary>
        protected virtual void ValidateSelf() { }
    }
}
