using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using WebApplication1.Data.Models;

namespace WebApplication1.ViewModels
{
    public class ChangeRoleViewModel
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public Role UserRole { get; set; }
        public IEnumerable<Role> AllRoles { get; set; }
    }
}
