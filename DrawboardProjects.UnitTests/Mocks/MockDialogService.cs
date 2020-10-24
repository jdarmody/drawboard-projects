using MvvmDialogs;
using MvvmDialogs.FrameworkPickers.FileOpen;
using MvvmDialogs.FrameworkPickers.FileSave;
using MvvmDialogs.FrameworkPickers.Folder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace DrawboardProjects.UnitTests.Mocks
{
    /// <summary>
    /// Mock for the <see cref="IDialogService"/>
    /// </summary>
    public class MockDialogService : IDialogService
    {
        public IAsyncOperation<IReadOnlyList<StorageFile>> PickMultipleFilesAsync(FileOpenPickerSettings settings)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<StorageFile> PickSaveFileAsync(FileSavePickerSettings settings)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<StorageFile> PickSingleFileAsync(FileOpenPickerSettings settings)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<StorageFolder> PickSingleFolderAsync(FolderPickerSettings settings)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<ContentDialogResult> ShowContentDialogAsync<T>(INotifyPropertyChanged viewModel) where T : ContentDialog
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<ContentDialogResult> ShowContentDialogAsync(INotifyPropertyChanged viewModel)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<ContentDialogResult> ShowCustomContentDialogAsync<T>(INotifyPropertyChanged viewModel) where T : IContentDialog
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<IUICommand> ShowMessageDialogAsync(string content, string title = null, IEnumerable<IUICommand> commands = null, uint? defaultCommandIndex = null, uint? cancelCommandIndex = null, MessageDialogOptions options = MessageDialogOptions.None)
        {
            if(commands != null)
            {
                //TODO: This shouldn't be here. Use Moq to setup required dialog results.
                // Setting up for Logout test
                var logoutCommand = commands.FirstOrDefault(c => c.Label == "Logout");
                if(logoutCommand != null)
                {
                    return Task.FromResult(logoutCommand).AsAsyncOperation();
                }
            }
            return Task.FromResult((IUICommand)new UICommand("Mock")).AsAsyncOperation();
        }
    }
}
