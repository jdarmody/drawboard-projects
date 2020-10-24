using MvvmCross.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;

namespace DrawboardProjects.UnitTests.Mocks
{
    /// <summary>
    /// Mock for handling requests for navigation and actions to be run on the UI thread from View Models.
    /// </summary>
    /// <remarks>This class doesn't actualy yet grab the UI thread (yet).</remarks>
    public class MockDispatcher : MvxMainThreadDispatcher, IMvxViewDispatcher
    {
        public readonly List<MvxViewModelRequest> Requests = new List<MvxViewModelRequest>();
        public readonly List<MvxPresentationHint> Hints = new List<MvxPresentationHint>();

        public override bool IsOnMainThread => true;

        public bool RequestMainThreadAction(Action action)
        {
            action();
            return true;
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            Requests.Add(request);
            return true;
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            Hints.Add(hint);
            return true;
        }

        Task<bool> IMvxViewDispatcher.ShowViewModel(MvxViewModelRequest request)
        {
            Requests.Add(request);
            return Task.FromResult(true);
        }

        Task<bool> IMvxViewDispatcher.ChangePresentation(MvxPresentationHint hint)
        {
            ChangePresentation(hint);
            return Task.FromResult(true);
        }

        public async Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
        {
            Exception e = null;
            await Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    e = ex;
                }
            });
            if (!maskExceptions)
                throw e;
        }

        public async Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true)
        {
            Exception e = null;
            await Task.Run(async () =>
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    e = ex;
                }
            });
            if (!maskExceptions)
                throw e;
        }

        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                if (!maskExceptions)
                    throw ex;
                return false;
            }
        }
    }
}
