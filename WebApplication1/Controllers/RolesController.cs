using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using InternetShop.Data.Interfaces;
using InternetShop.Data.Models;
using InternetShop.Data.Specifications;
using InternetShop.ViewModels;

namespace InternetShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _rolesRepository;

        public RolesController(IUserRepository IUserRepository, IRoleRepository IRolesRepository)
        {
            _rolesRepository = IRolesRepository;
            _userRepository = IUserRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _rolesRepository.GetAllAsync(new RoleSpecification().SortByName()));
        }

        public async Task<IActionResult> UserList(string search)
        {
            return View(await _userRepository.GetAllAsync(new UserSpecification().IncludeRole().WhereEmail(search).SortByRole()));
        }

        public async Task<IActionResult> Edit(int userId)
        {
            User user = await _userRepository.GetUserAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRole = user.Role;
                IEnumerable<Role> allRoles = await _rolesRepository.GetAllAsync(new RoleSpecification().SortByName());
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

        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Edit(int userId, string nameRole)
        {
            User user = await _userRepository.GetUserAsync(userId);
            if (user != null)
            {
                Role role = await _rolesRepository.GetByNameAsync(nameRole);
                user.Role = role;
                await _userRepository.UpdateAsync(user);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }

        public async Task<IActionResult> Delete(int userId)
        {
            User user = await _userRepository.GetUserAsync(userId);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    }
}
