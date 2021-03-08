using WebApplication1.Data.AbstractClasses;

namespace WebApplication1.Data.Models
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
