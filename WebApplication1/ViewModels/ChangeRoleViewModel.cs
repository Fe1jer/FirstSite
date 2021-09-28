﻿using System.Collections.Generic;
using InternetShop.Data.Models;

namespace InternetShop.ViewModels
{
    public class ChangeRoleViewModel
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public Role UserRole { get; set; }
        public IEnumerable<Role> AllRoles { get; set; }
    }
}
