using System.Data.SqlClient;
using WebApi.ModelLayer;


namespace WebApi.DataAccessLayer
{
    public class LoginDataAccess : ILoginDataAccess
    {
        SqlConnection connection;
        public LoginDataAccess(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }
        #region CRUD Methods
        public bool CreateLogin(Login login)
        {
            string hashPassword = Login.HashPassword(login.Password);
            string commandText = "INSERT INTO Login (UserName,Hash,AdminRights) VALUES (@username,@hash,@adminrights)";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@username", login.UserName);
                command.Parameters.AddWithValue("@hash", hashPassword);
                command.Parameters.AddWithValue("@adminrights", login.AdminRights);
                try
                {
                    command.ExecuteScalar();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception($"Exception while trying to insert Login object. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public bool DeleteLogin(Login login)
        {
            string commandText = "DELETE FROM Login WHERE UserName = @username";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@username", login.UserName);

                try
                {
                    return command.ExecuteNonQuery() == 1;

                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to delete a Login. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public IEnumerable<Login> GetAllLoginInformation()
        {
            string commandText = "SELECT UserName,Hash,AdminRights FROM Login";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);

                try
                {
                    List<Login> members = new List<Login>();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        members.Add(DataReaderRowToLogin(reader));
                    }
                    return members;

                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to read all rows from the Login table. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        // we don't want to update login information, admin has rights to delete and create new logins but not to change passwords and usernames
        /*public bool UpdateLogin(Login login)
        {
            string commandText = "UPDATE Login SET UserName=@username, Hash=@hash WHERE UserName=@un";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@username", login.UserName);
                command.Parameters.AddWithValue("@hash", login.Password);
                command.Parameters.AddWithValue("@un", login.UserName);

                try
                {
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to update Login. The exception was: '{ex.Message}'", ex);
                }
            }
        }*/
        #endregion

        #region Helper Methods
        protected Login DataReaderRowToLogin(SqlDataReader reader)
        {
            Login login = new Login();
            login.UserName = (string)reader["UserName"];
            login.Password = (string)reader["Hash"];
            login.AdminRights = (bool)reader["AdminRights"];

            return login;
        }

        public bool ValidateLogin(Login incomingLogin)
        {
            IEnumerable<Login> logins = GetAllLoginInformation();
            foreach (Login login in logins)
            {
                if (incomingLogin.UserName.Equals(login.UserName) && Login.ValidatePassword(incomingLogin.Password, login.Password) == true && incomingLogin.AdminRights == login.AdminRights)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
