using DrawboardProjects.Core.PresentationHints;
using MvvmCross.Platforms.Uap.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace DrawboardProjects
{
    /// <summary>
    /// Handles back stack presentation hints
    /// </summary>
    public class BackStackHintHandler
    {
        private readonly Frame _frame;

        public BackStackHintHandler(IMvxWindowsFrame rootFrame)
        {
            _frame = (Frame)rootFrame.UnderlyingControl;
        }

        public Task<bool> HandleClearBackstackHint(ClearBackstackHint clearBackstackHint)
        {
            _frame.BackStack.Clear();
            UpdateBackButtonVisibility();
            return Task.FromResult(true);
        }

        public Task<bool> HandlePopBackstackHint(PopBackstackHint clearBackstackHint)
        {
            if (_frame.CanGoBack)
            {
                _frame.BackStack.RemoveAt(_frame.BackStackDepth - 1);
                UpdateBackButtonVisibility();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        private void UpdateBackButtonVisibility()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                _frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }
    }
}
