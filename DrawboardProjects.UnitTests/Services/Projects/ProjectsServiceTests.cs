using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrawboardProjects.Core.Services.Authentication;
using DrawboardProjects.Core.Services.Projects;
using DrawboardProjects.Core.Models.Token;
using DrawboardProjects.Core.Services;
using System.IO;
using Windows.UI.Core;
using Windows.Foundation;
using DrawboardProjects.Core.Config;

namespace DrawboardProjects.UnitTests.Services.Projects
{
    [TestClass]
    public class ProjectsServiceTests
    {
        [TestMethod]
        public async Task GetProjects_With_Authenticated_User()
        {
            // Arrange
            //var token = await GetAuthenticationToken();
            ProjectsService svc = new ProjectsService(await GetAuthenticationService());
            // Act
            var projects = await svc.GetProjects();
            // Assert
            Assert.IsNotNull(projects);
        }
        [TestMethod]
        public async Task GetProjects_Without_Authentication()
        {
            // Arrange
            ProjectsService svc = new ProjectsService(new AuthenticationService());
            // Act and Assert
            await Assert.ThrowsExceptionAsync<NotAuthenticatedException>(async () => await svc.GetProjects());
        }
        [TestMethod]
        public async Task GetProjectLogo()
        {
            // Arrange
            ProjectsService svc = new ProjectsService(await GetAuthenticationService());
            // Act
            var projects = await svc.GetProjects();
            if (projects?.Count() > 0)
            {
                var first = projects.First();
                int width = 100, height = 100;
                var stream = await svc.GetProjectLogo(first.Id, width, height);
                // Create a .NET memory stream.
                var memStream = new MemoryStream();
                // Convert the stream to the memory stream, because a memory stream supports seeking.
                await stream.CopyToAsync(memStream);
                // Set the start position.
                memStream.Position = 0;
                // UI Thread needed
                await ExecuteOnUIThreadAsync(() =>
                {
                    // Create a new bitmap image.
                    var bitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                    // Set the bitmap source to the stream, which is converted to a IRandomAccessStream.
                    bitmap.SetSource(memStream.AsRandomAccessStream());
                });
            }
            // Assert
            // TODO
        }
        async Task<IAuthenticationService> GetAuthenticationService()
        {
            var config = new AppConfig();
            string username = config.AuthenticationForTesting.Username;
            string password = config.AuthenticationForTesting.PlainTextPassword;
            AuthenticationService svc = new AuthenticationService();
            await svc.Login(username, password);
            return svc;
        }
        public IAsyncAction ExecuteOnUIThreadAsync(DispatchedHandler action)
        {
            return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, action);
        }
    }
}
