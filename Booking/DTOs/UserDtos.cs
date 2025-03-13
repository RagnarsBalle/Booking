namespace Room.DTOs
{
    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int RoleID { get; set; } = 3;
    }

    public class UserLoginDto
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}