using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roles;
        private readonly IWebHostEnvironment _appEnvironment;

        public AccountController(IUserRepository IUserRepository, IRoleRepository roles, IWebHostEnvironment appEnvironment)
        {
            _userRepository = IUserRepository;
            _roles = roles;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Settings()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userRepository.GetUserAsync(model.Email);

                if (user == null)
                {
                    // добавляем пользователя в бд
                    user = _userRepository.CreateUser(model);

                    Role userRole = await _roles.GetByNameAsync("user");

                    if (userRole != null)
                    {
                        user.Role = userRole;
                    }

                    await _userRepository.AddAsync(user);
                    var code = HmacService.CreatePasswordResetHmacCode(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
                    try
                    {
                        EmailService emailService = new EmailService();
                        await emailService.SendEmailAsync(model.Email, "Регистрация",
                            $"Для подтверждения почты пройдите по ссылке: <a href='{callbackUrl}'>link</a>");
                    }
                    catch
                    {
                        return RedirectToAction("SenderMailError", "Error");
                    }

                    return View("RegisterConfirmation");
                }
                else if (user.EmailConfirmed == false)
                {
                    if (user.LockoutEnd >= DateTime.Now)
                    {
                        ModelState.AddModelError("", "Подтвердите регистрацию на почте.");
                    }
                    else
                    {
                        user.LockoutEnd = DateTime.Now.AddMinutes(5);
                        await _userRepository.UpdateAsync(user);

                        return View("RegisterConfirmation");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с данной почтой уже существует");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userRepository.GetUserAsync(model.Email);
                if (user != null && user.EmailConfirmed == false && user.LockoutEnd < DateTime.Now)
                {
                    await _userRepository.DeleteAsync(user);
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
                else if (user != null && user.EmailConfirmed == false)
                {
                    ModelState.AddModelError("", "Подтвердите регистрацию на почте.");
                }
                else if (user != null && HashingService.VerifyHashedPassword(user.Password, model.Password))
                {
                    await Authenticate(user, model.RememberMe); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            User user = await _userRepository.GetUserAsync(User.Identity.Name);
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
            if (ModelState.IsValid)
            {
                User user = await _userRepository.GetUserAsync(User.Identity.Name);
                if (model.Img != null)
                {
                    // удаляем старый файл из папки Files в каталоге 
                    if (System.IO.File.Exists($"wwwroot{user.Img}"))
                    {
                        System.IO.File.Delete($"wwwroot{user.Img}");
                    }
                    // путь к папке Files
                    string path = "/img/UsersAvatar/" + model.Email + "-" + model.Img.FileName;
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await model.Img.CopyToAsync(fileStream);
                    }
                    user.Img = path;
                    await Authenticate(user, true);
                }
                user.Role = await _roles.GetByIdAsync(user.RoleId);
                await _userRepository.UpdateAsync(user, model);

                return RedirectToAction("Profile");
            }
            model.Avatar = User.FindFirst("Avatar").Value.ToString();
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
                var user = await _userRepository.GetUserAsync(model.Email);
                if (user == null)
                {
                    // пользователь с данным email может отсутствовать в бд
                    // тем не менее мы выводим стандартное сообщение, чтобы скрыть 
                    // наличие или отсутствие пользователя в бд 
                    return View("ForgotPasswordConfirmation");
                }
                if (user.LockoutEnd >= DateTime.Now)
                {
                    ModelState.AddModelError(string.Empty, "Повторное сообщение на почту можно будет отправить позже.");
                }
                else
                {
                    user.LockoutEnd = DateTime.Now.AddMinutes(5);
                    await _userRepository.UpdateAsync(user);
                    var code = HmacService.CreatePasswordResetHmacCode(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
                    try
                    {
                        EmailService emailService = new EmailService();
                        await emailService.SendEmailAsync(model.Email, "Сброс пароля",
                            $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>link</a>");
                    }
                    catch
                    {
                        return RedirectToAction("SenderMailError", "Error");
                    }

                    return View("ForgotPasswordConfirmation");
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userId = null, string code = null)
        {
            return code == null || userId == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserAsync(model.Email);
                if (!HmacService.VerifyPasswordResetHmacCode(model.Code, model.userId))
                {
                    ModelState.AddModelError(string.Empty, "Использован недействительный, подделанный или просроченный код.");
                }
                else
                {
                    user.Password = HashingService.HashPassword(model.Password);
                    await _userRepository.UpdateAsync(user);
                    return View("ResetPasswordConfirmation");
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (!HmacService.VerifyPasswordResetHmacCode(code, userId))
            {
                ModelState.AddModelError(string.Empty, "Использован недействительный, подделанный или просроченный код.");
                return View("Error");
            }
            var user = await _userRepository.GetUserAsync(int.Parse(userId));

            user.EmailConfirmed = true;
            user.LockoutEnd = null;
            await _userRepository.UpdateAsync(user);
            await Authenticate(user, true);
            return RedirectToAction("Index", "Home");
        }

        [ValidateAntiForgeryToken]
        private async Task Authenticate(User user, bool isRemember = false)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name),
                new Claim("Avatar", user.Img),
                new Claim("Name", user.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id),
                new AuthenticationProperties
                {
                    IsPersistent = isRemember
                });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
