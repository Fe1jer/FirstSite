using InternetShop.Controllers;
using InternetShop.Data.Models;
using InternetShop.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class AccountControllerTest
    {
        AccountController AccountController { get; set; }

        public AccountControllerTest()
        {
            List<User> _users = new List<User>()
            {
                new User { Id=1, Email="Tom", PasswordHash="123"},
                new User { Id=2, Name="Alice"},
                new User { Id=3, Name="Sam"},
                new User { Id=4, Name="Kate"}
            };

            var _userManager = MockUserManager(_users).Object;
            // Arrange
            var signInManagerMock = MockSignInManager(_userManager);
            var mockWebHost = new Mock<IWebHostEnvironment>();
            Mock<IUrlHelper> urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(x => x.IsLocalUrl(It.IsAny<string>())).Returns(true);// Mocj Content method of Url Helper
            urlHelperMock.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns("callbackUrl");
            //urlHelperMock.Setup(x => UrlHelperExtensions.Action(It.IsAny<IUrlHelper>(),It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>())).Returns("http://www.google.com");// Mocj Content method of Url Helper
            AccountController = new AccountController(_userManager, signInManagerMock.Object, mockWebHost.Object) { Url = urlHelperMock.Object };
        }

        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(ls[0]);
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.IsEmailConfirmedAsync(It.IsAny<TUser>())).ReturnsAsync(true);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }

        public static Mock<SignInManager<User>> MockSignInManager(UserManager<User> _userManager)
        {
            var signInManagerMock = new Mock<SignInManager<User>>(_userManager, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);

            signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            return signInManagerMock;
        }

        [Fact]
        public async Task LoginReturnsViewResultRedirect()
        {
            // Arrange
            LoginViewModel newUser = new LoginViewModel() { Email = "Tom", Password = "123", RememberMe = false, ReturnUrl = "http://www.google.com" };
            // Act
            var result = await AccountController.Login(newUser);
            // Arrange
            var redirectToActionResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("http://www.google.com", redirectToActionResult.Url);
        }

        [Fact]
        public async Task LoginReturnsViewResultRedirectToActionIndexHome()
        {
            // Arrange
            LoginViewModel newUser = new LoginViewModel() { Email = "Tom", Password = "123", RememberMe = false, ReturnUrl = null };
            // Act
            var result = await AccountController.Login(newUser);
            // Arrange
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Theory]
        [InlineData("Неправильный логин и (или) пароль")]
        [InlineData("Вы не подтвердили свой email")]
        public async Task LoginReturnsViewResultWithUserModelAsync(string modelError)
        {
            // Arrange
            AccountController.ModelState.AddModelError("", modelError);
            LoginViewModel newUser = new LoginViewModel();
            // Act
            var result = await AccountController.Login(newUser);
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newUser, viewResult.Model);
        }

        [Fact]
        public async Task RegisterReturnsViewResultWithUserModelAsync()
        {
            // Arrange
            AccountController.ModelState.AddModelError("", "Неверно введены данные");
            RegisterViewModel newUser = new RegisterViewModel();
            // Act
            var result = await AccountController.Register(newUser);
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newUser, viewResult.Model);
        }

        [Fact]
        public async Task RegisterReturnsViewResultRedirectToActionIndexHome()
        {
            // Arrange
            RegisterViewModel newUser = new RegisterViewModel() { Email = "Tom", Password = "123", PasswordConfirm = "123" };
            // Act
            var result = await AccountController.Register(newUser);
            // Arrange
            var redirectToActionResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("RegisterConfirmation", redirectToActionResult.ViewName);
        }
    }
}
