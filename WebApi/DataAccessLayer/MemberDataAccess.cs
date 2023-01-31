using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using WebApi.ModelLayer;

namespace WebApi.DataAccessLayer
{
    public class MemberDataAccess : IMemberDataAccess
    {
        SqlConnection connection;
        public MemberDataAccess(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }
        #region CRUD Methods
        public bool CreateMember(Member member)
        {
            string commandText = "INSERT INTO Member (Name,Email) VALUES (@name,@email)";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@name", member.Name);
                command.Parameters.AddWithValue("@email", member.Email);
                try
                {
                    command.ExecuteScalar();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception($"Exception while trying to insert Member object. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public bool DeleteMember(int memberId)
        {
            string commandText = "DELETE FROM Member WHERE MemberID = @memberid";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@memberid", memberId);

                try
                {
                    return command.ExecuteNonQuery() == 1;

                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to delete a Member. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public Member FindMemberFromId(int memberId)
        {
            string commandText = "SELECT * FROM Member WHERE MemberID = @memberid";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@memberid", memberId);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return DataReaderRowToMember(reader);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to find the Member with the '{memberId}'. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public IEnumerable<Member> GetAllMembers()
        {
            string commandText = "SELECT * FROM Member";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);

                try
                {
                    List<Member> members = new List<Member>();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        members.Add(DataReaderRowToMember(reader));
                    }
                    return members;

                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to read all rows from the Member table. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public bool UpdateMember(Member member)
        {
            string commandText = "UPDATE Member SET Name=@name, Email=@email WHERE MemberID=@memberid";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@name", member.Name);
                command.Parameters.AddWithValue("@email", member.Email);
                command.Parameters.AddWithValue("@memberid", member.MemberID);

                try
                {
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to update Member. The exception was: '{ex.Message}'", ex);
                }
            }
        }
        #endregion

        #region Helper Methods
        protected Member DataReaderRowToMember(SqlDataReader reader)
        {
            Member member = new Member();
            member.MemberID = (int)reader["MemberID"];
            member.Name = (string)reader["Name"];
            member.Email = (string)reader["Email"];

            return member;
        }
        #endregion
    }
}