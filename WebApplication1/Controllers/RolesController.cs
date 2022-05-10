using InternetShop.Data.Models;
using InternetShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace InternetShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public RolesController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index() => View(_roleManager.Roles.ToList());

        public IActionResult UserList()
        {
            var usersWithRoles = _userManager.Users.ToList().Select(p => new UsersInRoleViewModel()
            {
                UserId = p.Id,
                Username = p.Name,
                Email = p.Email,
                Role = string.Join(", ", _userManager.GetRolesAsync(p).Result.OrderBy(p => p)),
                Img = p.Img,
                IsValid = p.EmailConfirmed != false || p.LockoutEnd > DateTime.Now
            }).ToList();
            usersWithRoles = usersWithRoles.OrderBy(p => p.Role).ToList();
            return View(usersWithRoles);
        }

        public async Task<IActionResult> Edit(string userId)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles;
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles.ToList()
                };
                return View(model);
            }

            return NotFound();
        }

        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (roles.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "Роли пользователя отсутствуют");
                    var allRoles = _roleManager.Roles;
                    ChangeRoleViewModel model = new ChangeRoleViewModel
                    {
                        UserId = user.Id,
                        UserEmail = user.Email,
                        UserRoles = roles,
                        AllRoles = allRoles.ToList()
                    };
                    return View(model);
                }
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);
                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }

        public async Task<IActionResult> Delete(int userId)
        {
            User user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    }
}
