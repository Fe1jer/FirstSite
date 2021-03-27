using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private readonly IUserRepository _users;
        private readonly IRoleRepository _roles;

        public RolesController(IUserRepository users, IRoleRepository roles)
        {
            _roles = roles;
            _users = users;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Список ролей";
            return View(await _roles.GetRolesAsync(new RoleSpecification().SortByName()));
        }

        public async Task<IActionResult> UserList(string search)
        {
            ViewBag.Title = "Список авторизованных пользователей";
            if (search != null)
            {
                return View(await _users.GetUsersAsync(new UserSpecification().IncludeRole().WhereEmail(search).SortByRole()));
            }
            return View(await _users.GetUsersAsync(new UserSpecification().IncludeRole().SortByRole()));
        }

        public async Task<IActionResult> Edit(int userId)
        {
            ViewBag.Title = "Выбор роли пользователя";
            // получаем пользователя
            User user = await _users.GetUserAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRole = user.Role;
                IEnumerable<Role> allRoles = await _roles.GetRolesAsync(new RoleSpecification().SortByName());
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRole = userRole,
                    AllRoles = allRoles
                };
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int userId, string nameRole)
        {
            ViewBag.Title = "Выбор роли пользователя";
            // получаем пользователя
            User user = await _users.GetUserAsync(userId);
            if (user != null)
            {
                Role role = await _roles.GetRoleAsync(nameRole);
                user.Role = role;
                await _users.UpdateUserRole(user);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    }
}
