using System.Data.SqlClient;
using WebApi.ModelLayer;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApi.DataAccessLayer
{
    public class EventDataAccess : IEventDataAccess
    {
        SqlConnection connection;
        public EventDataAccess(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }
        #region CRUD Methods
        public bool CreateEvent(Event e)
        {
            string commandText = "INSERT INTO Event (GameID,Name,Description,StartDate,EndDate,Capacity) VALUES (@gameid,@name,@description,@startdate,@enddate,@capacity)";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@gameid", e.GameID);
                command.Parameters.AddWithValue("@name", e.Name);
                command.Parameters.AddWithValue("@description", e.Description);
                command.Parameters.AddWithValue("@startdate", e.StartDate.ToUniversalTime());
                command.Parameters.AddWithValue("@enddate", e.EndDate.ToUniversalTime());
                command.Parameters.AddWithValue("@capacity", e.Capacity);
                try
                {
                    command.ExecuteScalar();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to insert Event object. The exception was: '{ex.Message}'", ex);
                    return false;
                    
                }
            }
        }

        public bool DeleteEvent(Event e)
        {
            string commandText = "DELETE FROM Event WHERE EventID = @eventid";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@eventid", e.EventID);

                try
                {
                    return command.ExecuteNonQuery() == 1;

                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to delete an Event. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public Event FindEventFromId(int eventId)
        {
            string commandText = "SELECT * FROM Event WHERE EventID = @eventid";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@eventid", eventId);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return DataReaderRowToEvent(reader);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to find the Event with the '{eventId}'. The exception was: '{ex.Message}'", ex);
                }
            }
        }

		public Event FindUpcomingEvent()
		{
            string commandText = "SELECT TOP 1 * FROM Event WHERE StartDate = (SELECT MIN(StartDate) FROM Event)";
			using (connection)
			{
				connection.Open();

				SqlCommand command = new SqlCommand(commandText, connection);

				try
				{
					SqlDataReader reader = command.ExecuteReader();
					if (reader.Read())
					{
						return DataReaderRowToEvent(reader);
					}
					else
					{
						return null;
					}
				}
				catch (Exception ex)
				{
					throw new Exception($"Exception while trying to find the upcoming Event. The exception was: '{ex.Message}'", ex);
				}
			}
		}

		public IEnumerable<Event> GetAllEvents()
        {
            string commandText = "SELECT * FROM Event";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);

                try
                {
                    List<Event> events = new List<Event>();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        events.Add(DataReaderRowToEvent(reader));
                    }
                    foreach (Event e in events)
                    {
                        e.StartDate = e.StartDate.ToLocalTime();
                        e.EndDate = e.EndDate.ToLocalTime();
                    }
                    return events;

                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to read all rows from the Event table. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public bool UpdateEvent(Event e)
        {
            string commandText = "UPDATE Event SET Name=@name, Description=@description, StartDate=@startdate, EndDate=@enddate WHERE EventID=@eventid";
            using (connection)
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandText, connection);
                
                command.Parameters.AddWithValue("@name", e.Name);
                command.Parameters.AddWithValue("@description", e.Description);
                command.Parameters.AddWithValue("@startdate", e.StartDate.ToUniversalTime());
                command.Parameters.AddWithValue("@enddate", e.EndDate.ToUniversalTime());
                command.Parameters.AddWithValue("@eventid", e.EventID);

                try
                {
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to update Event. The exception was: '{ex.Message}'", ex);
                }
            }
        }
        #endregion
        #region Helper Methods
        protected Event DataReaderRowToEvent(SqlDataReader reader)
        {
            Event e = new Event();
            e.EventID = (int)reader["EventID"];
            e.GameID = (int)reader["GameID"];
            e.Name = (string)reader["Name"];
            e.Description = (string)reader["Description"];
            e.StartDate = (DateTime)reader["StartDate"];
            e.EndDate = (DateTime)reader["EndDate"];
            e.Capacity = (int)reader["Capacity"];


            return e;
        }
        #endregion
    }
}
