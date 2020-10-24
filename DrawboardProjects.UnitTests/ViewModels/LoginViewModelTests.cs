using DrawboardProjects.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawboardProjects.UnitTests.ViewModels
{
    [TestClass]
    public class LoginViewModelTests : ViewModelTest
    {
        [TestMethod]
        public async Task Successful_Login()
        {
            base.Setup(); // from MvxIoCSupportingTest

            // Arrange
            var vm = Ioc.IoCConstruct<LoginViewModel>();
            vm.Username = VALID_USERNAME;
            vm.Password = VALID_PASSWORD;
            // Act
            await vm.LoginCommand.ExecuteAsync();
            // Assert
            Assert.IsTrue(vm.IsValid, nameof(vm.IsValid) + " should be true");
            Assert.IsTrue(MockAuthenticationService.Object.IsAuthenticated, "Authentication service isn't authenticated");
        }

        [TestMethod]
        public async Task Login_With_Invalid_Credentials()
        {
            base.Setup(); // from MvxIoCSupportingTest

            // Arrange
            var vm = Ioc.IoCConstruct<LoginViewModel>();
            vm.Username = "invalid";
            vm.Password = "invalid";
            // Act
            await vm.LoginCommand.ExecuteAsync();
            // Assert
            Assert.IsTrue(vm.IsValid, nameof(vm.IsValid) + " should be true");
            Assert.IsFalse(MockAuthenticationService.Object.IsAuthenticated, "Authentication service should not be authenticated");
        }

        [TestMethod]
        public async Task Attempt_Login_With_No_Credentials()
        {
            base.Setup(); // from MvxIoCSupportingTest

            // Arrange
            var vm = Ioc.IoCConstruct<LoginViewModel>();
            vm.Username = null;
            vm.Password = null;
            // Act
            await vm.LoginCommand.ExecuteAsync();
            // Assert
            Assert.IsFalse(MockAuthenticationService.Object.IsAuthenticated, "Authentication service should not be authenticated");
            Assert.IsFalse(vm.IsValid, nameof(vm.IsValid) + " should be false");
        }

        [TestMethod]
        public async Task Attempt_Login_With_No_Username()
        {
            base.Setup(); // from MvxIoCSupportingTest

            // Arrange
            var vm = Ioc.IoCConstruct<LoginViewModel>();
            vm.Username = null;
            vm.Password = VALID_PASSWORD;
            // Act
            await vm.LoginCommand.ExecuteAsync();
            // Assert
            Assert.IsFalse(vm.IsValid, nameof(vm.IsValid) + " should be false");
            Assert.IsTrue(!string.IsNullOrEmpty(vm.ValidationErrors[nameof(vm.Username)]), $"{nameof(vm.ValidationErrors)} does not set an error message for {nameof(vm.Username)}");
            Assert.IsFalse(MockAuthenticationService.Object.IsAuthenticated, "Authentication service should not be authenticated");
        }

        [TestMethod]
        public async Task Attempt_Login_With_No_Password()
        {
            base.Setup(); // from MvxIoCSupportingTest

            // Arrange
            var vm = Ioc.IoCConstruct<LoginViewModel>();
            vm.Username = VALID_USERNAME;
            vm.Password = null;
            // Act
            await vm.LoginCommand.ExecuteAsync();
            // Assert
            Assert.IsFalse(vm.IsValid, nameof(vm.IsValid) + " should be false");
            Assert.IsTrue(!string.IsNullOrEmpty(vm.ValidationErrors[nameof(vm.Password)]), $"{nameof(vm.ValidationErrors)} does not set an error message for {nameof(vm.Password)}");
            Assert.IsFalse(MockAuthenticationService.Object.IsAuthenticated, "Authentication service should not be authenticated");
        }
    }
}
