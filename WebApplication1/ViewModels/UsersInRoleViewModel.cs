namespace InternetShop.ViewModels
{
    public class UsersInRoleViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Img { get; set; }
        public bool IsValid { get; set; }
    }
}
