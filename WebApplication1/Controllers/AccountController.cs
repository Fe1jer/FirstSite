using InternetShop.Data.Models;
using InternetShop.Data.Services;
using InternetShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace InternetShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
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

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Name = model.Name, Gender = model.Gender, Img = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScY-9qDVs2yQiXkeEHGQfvxEPLWHh-o53ZuQ&usqp=CAU" };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "user");
                if (result.Succeeded)
                {
                    // генерация токена для пользователя
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                    return View("ForgotPasswordConfirmation");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // проверяем, подтвержден ли email
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                        return View(model);
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
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
                User user = await _userManager.GetUserAsync(HttpContext.User);
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

                }
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Patronymic = model.Patronymic;
                user.Gender = model.Gender;
                user.PhoneNumber = model.PhoneNumber;
                user.DateOfBirth = model.DateOfBirth;
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);

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
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // пользователь с данным email может отсутствовать в бд
                    // тем не менее мы выводим стандартное сообщение, чтобы скрыть 
                    // наличие или отсутствие пользователя в бд
                    return View("ForgotPasswordConfirmation");
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(model.Email, "Reset Password",
                    $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>link</a>");

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ResetPasswordConfirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
                return View("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                ModelState.AddModelError(string.Empty, "Использован недействительный, подделанный или просроченный код.");
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Использован недействительный, подделанный или просроченный код.");
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View("Error");
        }

        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
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
