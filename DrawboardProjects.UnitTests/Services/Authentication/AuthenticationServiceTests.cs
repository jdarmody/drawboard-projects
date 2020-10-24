using DrawboardProjects.Core.Config;
using DrawboardProjects.Core.Services;
using DrawboardProjects.Core.Services.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DrawboardProjects.UnitTests.Services.Authentication
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        [TestMethod]
        public async Task Login_With_Empty_Credentials()
        {
            // Arrange
            string username = "";
            string password = "";
            AuthenticationService svc = new AuthenticationService();

            // Act and Assert
            await Assert.ThrowsExceptionAsync<System.ArgumentException>(async () => await svc.Login(username, password));
        }

        [TestMethod]
        public async Task Login_With_Null_Credentials()
        {
            // Arrange
            string username = null;
            string password = null;
            AuthenticationService svc = new AuthenticationService();

            // Act and Assert
            await Assert.ThrowsExceptionAsync<System.ArgumentNullException>(async () => await svc.Login(username, password));
        }

        [TestMethod]
        public async Task Login_With_Valid_Credentials()
        {
            // Arrange
            var config = new AppConfig();
            string username = config.AuthenticationForTesting.Username;
            string password = config.AuthenticationForTesting.PlainTextPassword;
            AuthenticationService svc = new AuthenticationService();

            // Act
            var token = await svc.Login(username, password);

            // Assert
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.AccessToken);
            //TODO: can add more, but then we're testing the API rather than the client....
        }

        [TestMethod]
        public async Task Login_With_Invalid_Credentials()
        {
            // Arrange
            string username = "not going to work";
            string password = "invalid password";
            AuthenticationService svc = new AuthenticationService();

            // Act and Assert
            await Assert.ThrowsExceptionAsync<HttpResponseException>(async () => await svc.Login(username, password));
            //TODO: check status code is 400
        }

        //TODO: test offline, use timeout
    }
}
