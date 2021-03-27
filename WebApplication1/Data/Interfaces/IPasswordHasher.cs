namespace WebApplication1.Data.Interfaces
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);
        public bool VerifyHashedPassword(string hashedPassword, string password);
    }
}
