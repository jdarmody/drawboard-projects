using DrawboardProjects.Core.Models.Projects;
using DrawboardProjects.Core.Services;
using DrawboardProjects.Core.Services.Authentication;
using DrawboardProjects.Core.Services.Projects;
using DrawboardProjects.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrawboardProjects.UnitTests.ViewModels
{
    [TestClass]
    public class ProjectListViewModelTests : ViewModelTest
    {
        Mock<IProjectsService> mockProjectService;
        IEnumerable<Project> mockProjects;

        protected override void AdditionalSetup()
        {
            base.AdditionalSetup();

            SetupMockProjectsService();
        }

        [TestMethod]
        public async Task Initialize_Without_Authentication()
        {
            base.Setup(); // from MvxIoCSupportingTest

            // Arrange
            var viewModel = Ioc.IoCConstruct<ProjectListViewModel>();
            // Act
            await viewModel.Initialize();
            // Assert
            Assert.IsTrue(viewModel.Projects == null || viewModel.Projects.Count < 1, "Projects must be empty");
            Assert.IsTrue(string.IsNullOrEmpty(viewModel.UserDisplayName), $"{nameof(viewModel.UserDisplayName)} must not be set");
            Assert.AreEqual(viewModel.Title, "Projects (0)", $"{nameof(viewModel.Title)} not correct");
            Assert.IsTrue(viewModel.NoProjectsMessageVisible, $"{nameof(viewModel.NoProjectsMessageVisible)} must be true");
        }

        [TestMethod]
        public async Task Initialize_After_Authentication()
        {
            base.Setup(); // from MvxIoCSupportingTest

            // Arrange
            var viewModel = Ioc.IoCConstruct<ProjectListViewModel>();
            var token = await MockAuthenticationService.Object.Login(VALID_USERNAME, VALID_PASSWORD);
            // Act
            await viewModel.Initialize();
            // Assert
            Assert.IsNotNull(viewModel.Projects, $"{nameof(viewModel.Projects)} should not be NULL");
            Assert.AreEqual(viewModel.Projects.Count, mockProjects.Count(), "Unexpected project count");
            Assert.IsTrue(!string.IsNullOrEmpty(viewModel.UserDisplayName), $"{nameof(viewModel.UserDisplayName)} must be set");
            Assert.AreEqual(viewModel.Title, $"Projects ({mockProjects.Count()})", $"{nameof(viewModel.Title)} not correct");
            Assert.IsFalse(viewModel.NoProjectsMessageVisible, $"{nameof(viewModel.NoProjectsMessageVisible)} must be false");
        }

        [TestMethod]
        public async Task Logout_Without_Authentication()
        {
            base.Setup(); // from MvxIoCSupportingTest

            // Arrange
            var viewModel = Ioc.IoCConstruct<ProjectListViewModel>();
            await viewModel.Initialize();
            // Act
            await viewModel.LogoutCommand.ExecuteAsync();
            // Assert
            Assert.IsFalse(MockAuthenticationService.Object.IsAuthenticated, $"{nameof(IAuthenticationService.IsAuthenticated)} must be false"); 
        }

        [TestMethod]
        public async Task Logout_After_Authenticated()
        {
            base.Setup(); // from MvxIoCSupportingTest

            // Arrange
            await MockAuthenticationService.Object.Login(VALID_USERNAME, VALID_PASSWORD);
            var viewModel = Ioc.IoCConstruct<ProjectListViewModel>();
            await viewModel.Initialize();
            // Act
            await viewModel.LogoutCommand.ExecuteAsync();
            // Assert
            Assert.IsFalse(MockAuthenticationService.Object.IsAuthenticated, $"{nameof(IAuthenticationService.IsAuthenticated)} must be false");
        }

        [TestMethod]
        public async Task Select_First_Project()
        {
            base.Setup(); // from MvxIoCSupportingTest

            // Arrange
            var viewModel = Ioc.IoCConstruct<ProjectListViewModel>();
            await MockAuthenticationService.Object.Login(VALID_USERNAME, VALID_PASSWORD);
            await viewModel.Initialize();
            // Act
            viewModel.SelectedProject = viewModel.Projects.First();
            // Assert
            Assert.AreEqual(viewModel.SelectedProject, viewModel.Projects.First());
        }

        void SetupMockProjectsService()
        {
            mockProjectService = new Mock<IProjectsService>();
            // Setup successful GetProjects call which requires pre-authentication
            mockProjectService
                .When(() => MockAuthenticationService.Object.IsAuthenticated)
                .Setup(svc => svc.GetProjects())
                .Callback(() =>
                {
                    mockProjects = GetMockProjects(6);
                })
                .Returns(() => Task.FromResult(mockProjects));
            // Setup failure scenario of calling GetProjects while not authenticated
            mockProjectService
                .When(() => !MockAuthenticationService.Object.IsAuthenticated)
                .Setup(svc => svc.GetProjects())
                .Throws(new NotAuthenticatedException());
            // Setup successful call of GetProjectLogo which required pre-authentication
            mockProjectService
                .When(() => MockAuthenticationService.Object.IsAuthenticated)
                .Setup(svc => svc.GetProjectLogo(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(System.IO.Stream.Null));
            // Setup failure scenario of calling GetProjectLogo while not authenticated
            mockProjectService
                .When(() => !MockAuthenticationService.Object.IsAuthenticated)
                .Setup(svc => svc.GetProjectLogo(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new NotAuthenticatedException());
            // Setup IProjectsService as Singleton for dependency injection into the view model
            Ioc.RegisterSingleton<IProjectsService>(mockProjectService.Object);
        }

        /// <summary>
        /// Get simple collection of mock projects
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IEnumerable<Project> GetMockProjects(int count)
        {
            List<Project> projects = new List<Project>();
            for (int i = 0; i < count; i++)
            {
                projects.Add(new Project
                {
                    Name = $"P{i}",
                    Description = $"Description for P{i}",
                    Id = i.ToString(),
                    //TODO: set more fields
                }); ;
            }
            return projects;
        }

        //TODO: setup specific mock dialog service for handling the logout tests
        //void SetupMockDialogService()
        //{
        //    var mock = new Mock<IDialogService>();
        //    mock.Setup(svc => svc.ShowMessageDialogAsync(It.IsAny<string>(), It.IsAny<string>(),
        //        commands: It.Is(cmds => cmds.)))

        //    Ioc.RegisterSingleton<IDialogService>(mock.Object);
        //}
    }
}
