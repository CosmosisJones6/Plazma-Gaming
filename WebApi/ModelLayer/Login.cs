using bcrypt = BCrypt.Net;
namespace WebApi.ModelLayer
{
    public class Login
    {
        #region Properties
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool AdminRights { get; set; }
        #endregion
        public Login(string usreName, string password, bool adminRights)
        {
            UserName = usreName;
            Password = password;
            AdminRights = adminRights;
        }
        public Login(string usreName, string password)
        {
            UserName = usreName;
            Password = password;
        }
        public Login()
        {

        }
        private static string GetRandomSalt()
        {  
            return bcrypt.BCrypt.GenerateSalt(12);
        }

        public static string HashPassword(string password)
        {
            return bcrypt.BCrypt.HashPassword(password, GetRandomSalt());
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            return bcrypt.BCrypt.Verify(password, correctHash);
        }
    }
}
