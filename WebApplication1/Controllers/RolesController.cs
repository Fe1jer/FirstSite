﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _rolesRepository;

        public RolesController(IUserRepository IUolesRepository, IRoleRepository IRolesRepository)
        {
            _rolesRepository = IRolesRepository;
            _userRepository = IUolesRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _rolesRepository.GetRolesAsync(new RoleSpecification().SortByName()));
        }

        public async Task<IActionResult> UserList(string search)
        {
            if (search != null)
            {
                return View(await _userRepository.GetUsersAsync(new UserSpecification().IncludeRole().WhereEmail(search).SortByRole()));
            }
            return View(await _userRepository.GetUsersAsync(new UserSpecification().IncludeRole().SortByRole()));
        }

        public async Task<IActionResult> Edit(int userId)
        {
            User user = await _userRepository.GetUserAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRole = user.Role;
                IEnumerable<Role> allRoles = await _rolesRepository.GetRolesAsync(new RoleSpecification().SortByName());
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
            User user = await _userRepository.GetUserAsync(userId);
            if (user != null)
            {
                Role role = await _rolesRepository.GetRoleAsync(nameRole);
                user.Role = role;
                await _userRepository.UpdateUserAsync(user);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    }
}
