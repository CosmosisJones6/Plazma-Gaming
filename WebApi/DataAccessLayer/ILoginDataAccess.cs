using WebApi.ModelLayer;

namespace WebApi.DataAccessLayer
{
    public interface ILoginDataAccess
    {
        public bool CreateLogin(Login login);
        public IEnumerable<Login> GetAllLoginInformation();
        public bool DeleteLogin(Login login);
        public bool ValidateLogin(Login incomingLogin);
    }
}
