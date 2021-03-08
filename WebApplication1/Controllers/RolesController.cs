using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private readonly IAllUser _users;

        public RolesController(IAllUser users)
        {
            _users = users;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Список ролей";
            return View(_users.Roles);
        }

        public IActionResult UserList(string search)
        {
            ViewBag.Title = "Список авторизованных пользователей";
            if (search != null)
            {
                return View(_users.Users.Where(p => p.Email.Contains(search)).ToList());
            }
            return View(_users.Users);
        }

        public async Task<IActionResult> Edit(int userId)
        {
            ViewBag.Title = "Выбор роли пользователя";
            // получаем пользователя
            User user = await _users.User(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = user.Role;
                IEnumerable<Role> allRoles = _users.Roles;
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRole = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int userId, string role)
        {
            ViewBag.Title = "Выбор роли пользователя";
            // получаем пользователя
            User user = await _users.User(userId);
            if (user != null)
            {
                _users.SetUserRole(user, role);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    }
}
