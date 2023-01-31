using System.Data.SqlClient;
using WebApi.ModelLayer;

namespace WebApi.DataAccessLayer
{
    public class DeveloperDataAccess : IDeveloperDataAccess
    {
        SqlConnection connection;

        public DeveloperDataAccess(string connectionString)
        {
            connection = new SqlConnection(connectionString);


        }
        #region CRUD Methods
        public bool CreateDeveloper(Developer developer)
        {
            string commandText = "INSERT INTO Developer (Name,Email,Description) VALUES (@name, @email, @description)";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@name", developer.Name);
                command.Parameters.AddWithValue("@email", developer.Email);
                command.Parameters.AddWithValue("@description", developer.Description);

                try
                {
                    command.ExecuteScalar();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception($"Exception while trying to insert Developer object. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public Developer FindDeveloperFromId(int developerId)
        {
            string commandText = "SELECT * FROM Developer WHERE DeveloperID = @developerid";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@developerid", developerId);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return DataReaderRowToDeveloper(reader);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to find the developer with the '{developerId}'. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public IEnumerable<Developer> GetAllDevelopers()
        {
            string commandText = "SELECT * FROM Developer";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);

                try
                {
                    List<Developer> developer = new List<Developer>();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        developer.Add(DataReaderRowToDeveloper(reader));
                    }
                    return developer;

                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to read all rows from the Developer table. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public bool UpdateDeveloper(Developer developer)
        {
            string commandText = "UPDATE Developer SET Name=@name, Email=@email, Description=@description WHERE DeveloperID=@developerid";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@name", developer.Name);
                command.Parameters.AddWithValue("@email", developer.Email);
                command.Parameters.AddWithValue("@description", developer.Description);
                command.Parameters.AddWithValue("@developerid", developer.DeveloperID);

                try
                {
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to update developer. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public bool DeleteDeveloper(Developer developer)
        {
            string commandText = "DELETE FROM Developer WHERE DeveloperID = @developerid";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@developerid", developer.DeveloperID);

                try
                {
                    return command.ExecuteNonQuery() == 1;

                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to delete a developer. The exception was: '{ex.Message}'", ex);
                }
            }
        }
        #endregion
        #region Helper Methods
        protected Developer DataReaderRowToDeveloper(SqlDataReader reader)
        {
            Developer developer = new Developer();
            developer.DeveloperID = (int)reader["DeveloperID"];
            developer.Name = (string)reader["Name"];
            developer.Email = (string)reader["Email"];
            developer.Description = (string)reader["Description"];
            

            return developer;
        }
        #endregion
    }
}
