using DrawboardProjects.Core.Models.Projects;
using DrawboardProjects.Core.Services;
using DrawboardProjects.Core.Services.Projects;
using DrawboardProjects.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvvmCross.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawboardProjects.UnitTests.ViewModels
{
    [TestClass]
    public class ProjectListItemViewModelTests : ViewModelTest
    {
        Mock<IProjectsService> mockProjectService;

        protected override void AdditionalSetup()
        {
            base.AdditionalSetup();

            SetupMockProjectsService();
        }

        [TestMethod]
        public void Is_Name_Set()
        {
            base.Setup();

            // Arrange
            Project p = new Project
            {
                Description = "Mock description",
                Id = "1",
                Name = "Mock Name",
            };

            // Act
            var vm = new ProjectListItemViewModel(Ioc.Resolve<IMvxLogProvider>(), mockProjectService.Object, p);
            // Assert
            Assert.IsNotNull(vm.Name, nameof(vm.Name) + " should not be null");
            Assert.AreEqual(vm.Name, p.Name, nameof(vm.Name) + " not set to Project Name");
        }

        [TestMethod]
        public void Is_Description_Set()
        {
            base.Setup();

            // Arrange
            Project p = new Project
            {
                Description = "Mock description",
                Id = "1",
                Name = "Mock Name",
            };

            // Act
            var vm = new ProjectListItemViewModel(Ioc.Resolve<IMvxLogProvider>(), mockProjectService.Object, p);
            // Assert
            Assert.IsNotNull(vm.Description, nameof(vm.Description) + " should not be null");
            Assert.AreEqual(vm.Description, p.Description, nameof(vm.Description) + " not set to Project Description");
        }

        [TestMethod]
        public async Task Is_Logo_Set()
        {
            base.Setup();

            // Arrange
            Project p = new Project
            {
                Description = "Mock description",
                Id = "1",
                Name = "Mock Name",
            };
            await MockAuthenticationService.Object.Login(VALID_USERNAME, VALID_PASSWORD);
            var vm = new ProjectListItemViewModel(Ioc.Resolve<IMvxLogProvider>(), mockProjectService.Object, p);

            // Act
            //await vm.LoadLogo();
            // Give a few seconds for the Logo to be set
            await Task.Delay(5000);
            // Assert
            Assert.IsNotNull(vm.Logo, nameof(vm.Logo) + " should not be null");
        }

        [TestMethod]
        public async Task Is_Logo_Not_Set_Without_Authentication()
        {
            base.Setup();

            // Arrange
            Project p = new Project
            {
                Description = "Mock description",
                Id = "1",
                Name = "Mock Name",
            };
            var vm = new ProjectListItemViewModel(Ioc.Resolve<IMvxLogProvider>(), mockProjectService.Object, p);

            // Act
            //await vm.LoadLogo();
            // Give a few seconds for the Logo attempt to set
            await Task.Delay(5000);
            // Assert
            Assert.IsNull(vm.Logo, nameof(vm.Logo) + " should be null");
        }

        void SetupMockProjectsService()
        {
            mockProjectService = new Mock<IProjectsService>();
            // Setup use of GetProjectLogo when authenticated
            mockProjectService
                .When(() => MockAuthenticationService.Object.IsAuthenticated)
                .Setup(svc => svc.GetProjectLogo(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(System.IO.Stream.Null));
            // Setup use of GetProjectLogo when NOT authenticated
            mockProjectService
                .When(() => !MockAuthenticationService.Object.IsAuthenticated)
                .Setup(svc => svc.GetProjectLogo(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new NotAuthenticatedException());
            Ioc.RegisterSingleton<IProjectsService>(mockProjectService.Object);
        }
    }
}
