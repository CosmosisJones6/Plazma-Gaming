using System.Data.SqlClient;
using WebApi.ModelLayer;

namespace WebApi.DataAccessLayer
{
    public class SaleDataAccess : ISaleDataAccess
    {
        SqlConnection connection;
        public SaleDataAccess(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }
        public bool CreateSale(Sale sale)
        {
            string commandText = "INSERT INTO Sale (GameID, Email, Date, SalesPrice) VALUES (@gameid, @email, @date, @salesprice)";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@gameid", sale.GameID);
                command.Parameters.AddWithValue("@email", sale.Email);
                command.Parameters.AddWithValue("@date", sale.Date);
                command.Parameters.AddWithValue("@salesprice", sale.SalesPrice);
                

                try
                {
                    command.ExecuteScalar();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception($"Exception while trying to insert Sale object. The exception was: '{ex.Message}'", ex);
                }
            }
        }
        // we don't actually use this in the client app (because of the advanced datagrid view), but the api provides this functionality
        public IEnumerable<Sale> SalesInPeriod(DateTime startTime, DateTime endTime)
        {
            string commandText = "SELECT * FROM Sale WHERE Date BETWEEN @starttime AND @endtime";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@starttime", startTime);
                command.Parameters.AddWithValue("@endtime", endTime);

                try
                {
                    List<Sale> sales = new List<Sale>();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        sales.Add(DataReaderRowToSale(reader));
                    }
                    return sales;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to get sales info between two given times. The exception was: '{ex.Message}'", ex);
                }
            }
        }
        // we don't actually use this in the client app (because of the advanced datagrid view), but the api provides this functionality
        public IEnumerable<Sale> SalesByGame(int gameId)
        {
            string commandText = "SELECT * FROM Sale WHERE GameID = @gameid";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@gameid", gameId);

                try
                {
                    List<Sale> sales = new List<Sale>();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        sales.Add(DataReaderRowToSale(reader));
                    }
                    return sales;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to get sales info by one game. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public bool DeleteSale(Guid gamekey)
        {
            string commandText = "DELETE FROM Sale WHERE GameKey = @gamekey";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@gamekey", gamekey);

                try
                {
                    return command.ExecuteNonQuery() == 1;

                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to delete a sale. The exception was: '{ex.Message}'", ex);
                }
            }
        }
        // we don't actually use this in the client app (because of the advanced datagrid view), but the api provides this functionality
        public Sale FindSaleFromGameKey(Guid gameKey)
        {
            string commandText = "SELECT * FROM Sale WHERE GameKey = @gamekey";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@gamekey", gameKey);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return DataReaderRowToSale(reader);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to find the specific sale. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public IEnumerable<Sale> GetAllSales()
        {
            string commandText = "SELECT * FROM Sale";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);

                try
                {
                    List<Sale> sales = new List<Sale>();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        sales.Add(DataReaderRowToSale(reader));
                    }
                    return sales;

                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to read all rows from the Sale table. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        #region Helper Methods
        protected Sale DataReaderRowToSale(SqlDataReader reader)
        {
            Sale sale = new Sale();
            sale.GameKey = (Guid)reader["GameKey"];
            sale.GameID = (int)reader["GameID"];
            sale.Email = (string)reader["Email"];
            sale.Date = (DateTime)reader["Date"];
            sale.SalesPrice = Convert.ToSingle(reader["SalesPrice"]);

            return sale;
        }
        #endregion
    }
}
