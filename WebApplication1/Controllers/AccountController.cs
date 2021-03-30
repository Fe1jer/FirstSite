using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _hasher;
        private readonly IRoleRepository _roles;
        private readonly IWebHostEnvironment _appEnvironment;
        //private readonly UserManager<User> _userManager;

        public AccountController(IUserRepository users, IPasswordHasher hasher, IRoleRepository roles, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
        {
            _users = users;
            _hasher = hasher;
            _roles = roles;
            _appEnvironment = appEnvironment;
            //_userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Title = "Регистрация";
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.Title = "Регистрация";
            if (ModelState.IsValid)
            {
                User user = await _users.GetUserAsync(model.Email);

                if (user == null)
                {
                    // добавляем пользователя в бд
                    user = new User { Email = model.Email, Password = _hasher.HashPassword(model.Password), Gender = model.Gender, Name = model.Name, Img = "data:image/png;base64,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" };
                    Role userRole = await _roles.GetRoleAsync("user");

                    if (userRole != null)
                        user.Role = userRole;
                    await _users.AddUserAsync(user);
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Пользователь с данной почтой уже существует");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Title = "Вход";
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewBag.Title = "Вход";
            if (ModelState.IsValid)
            {
                User user = await _users.GetUserAsync(model.Email);

                if (user != null && _hasher.VerifyHashedPassword(user.Password, model.Password))
                {
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            ViewBag.Title = "Профиль";
            User user = await _users.GetUserAsync(User.Identity.Name);
            ProfileViewModel model = new ProfileViewModel
            {
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Avatar = user.Img
            };

            return View(model);
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            ViewBag.Title = "Профиль";
            if (ModelState.IsValid)
            {
                User user = await _users.GetUserAsync(User.Identity.Name);

                user.Email = model.Email;
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Patronymic = model.Patronymic;
                user.Gender = model.Gender;
                user.PhoneNumber = model.PhoneNumber;
                user.DateOfBirth = model.DateOfBirth;
                if (model.Img != null)
                {
                    // путь к папке Files
                    string path = "/img/UsersAvatar/" + user.Id + ".jpg";
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await model.Img.CopyToAsync(fileStream);
                    }
                    user.Img = path;

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await Authenticate(user);
                }
                user.Role = await _roles.GetRoleAsync(user.RoleId);
                await _users.UpdateUser(user);

                return RedirectToAction("Profile");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // пользователь с данным email может отсутствовать в бд
                    // тем не менее мы выводим стандартное сообщение, чтобы скрыть 
                    // наличие или отсутствие пользователя в бд
                    return View("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(model.Email, "Reset Password",
                    $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>link</a>");
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

/*        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // пользователь с данным email может отсутствовать в бд
                    // тем не менее мы выводим стандартное сообщение, чтобы скрыть 
                    // наличие или отсутствие пользователя в бд
                    return View("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(model.Email, "Reset Password",
                    $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>link</a>");
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }*/

        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name),
                new Claim("Avatar", user.Img)
            };
            // создаем объект ClaimsIdentity
            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsIdentity id = new ClaimsIdentity(claims, "Cookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
