using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Linq.Expressions;
using WebApi.ModelLayer;

namespace WebApi.DataAccessLayer
{
    public class EventMemberDataAccess : IEventMemberDataAccess
    {
        SqlConnection connection;
        public EventMemberDataAccess(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }
        #region CRUD Methods
        public IEnumerable<int> GetEventIdListByMemberId(int memberid)
        {
            string commandText = "SELECT EventID FROM EventMember WHERE MemberID = @memberid";
            using (connection)
            {

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@memberid", memberid);

                try
                {
                    List<int> eventIdList = new List<int>();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        eventIdList.Add((int)reader["EventID"]);
                    }
                    return eventIdList;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to find the event list by member id with the '{memberid}'. The exception was: '{ex.Message}'", ex);
                }
            }
        }

        public IEnumerable<int> GetMemberIdListByEventId(int eventId)
        {
            string commandText = "SELECT MemberID FROM EventMember WHERE EventID = @eventid";
            using (connection)
            {

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@eventid", eventId);

                try
                {
                    List<int> memberIdList = new List<int>();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        memberIdList.Add((int)reader["MemberID"]);
                    }
                    return memberIdList;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to find the member list in event with the '{eventId}'. The exception was: '{ex.Message}'", ex);
                }
            }
        }
        //concurrency issue -> transaction, if we only have one available space in the event capacity who should come first if they want to join at the same time
        public bool JoinEvent(EventMember eventMember)
        {
            SqlTransaction transaction;
            int eventCapacity;
            int eventParticipantNumber;
            //get capacity of event
            string getCapacity = "SELECT Capacity FROM Event WHERE EventID = @eventid";
            //get the number of already joined members
            string getCurrentParticipantNumber = "SELECT Count(*) FROM EventMember WHERE EventID = @eventid";
            //if possible add the member to participants
            string makeJoinEvent = "INSERT INTO EventMember (EventID, MemberID) VALUES (@eventid, @memberid)";
            using (transaction = connection.BeginTransaction(System.Data.IsolationLevel.RepeatableRead))
            {
                SqlCommand commandGetCapacity = new SqlCommand(getCapacity, connection, transaction);
                commandGetCapacity.Parameters.AddWithValue("@eventid", eventMember.EventID);

                SqlCommand numberOfParticipants = new SqlCommand(getCurrentParticipantNumber, connection, transaction);
                numberOfParticipants.Parameters.AddWithValue("@eventid", eventMember.EventID);

                SqlCommand commandCreateAttendance = new SqlCommand(makeJoinEvent, connection, transaction);
                commandCreateAttendance.Parameters.AddWithValue("@eventid", eventMember.EventID);
                commandCreateAttendance.Parameters.AddWithValue("@memberid", eventMember.MemberID);


                try
                {
                    try
                    {
                        SqlDataReader readerCapacity = commandGetCapacity.ExecuteReader();
                        readerCapacity.Read();
                        eventCapacity = (int)readerCapacity["Capacity"];
                        readerCapacity.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    try
                    {
                        SqlDataReader readerParticipant = numberOfParticipants.ExecuteReader();
                        readerParticipant.Read();
                        eventParticipantNumber = (int)readerParticipant.GetInt32(0);
                        readerParticipant.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                   // check if we have space to add a new participant
                    if (eventParticipantNumber >= eventCapacity)
                    {
                        //if not roll back
                        transaction.Rollback();
                        return false;
                    }
                    // if yes add participant and commit
                    commandCreateAttendance.ExecuteScalar();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    //if transaction breaks at the exact moment when rolling back we can see from this try catch below
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {

                        throw new Exception(e.Message);
                    }
                    return false;
                }
            }
        }

        public bool RemoveMemberFromEvent(EventMember eventMember)
        {
            string commandText = "DELETE FROM EventMember WHERE MemberID = @memberid AND EventID = @eventid";
            using (connection)
            {

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@memberid", eventMember.MemberID);
                command.Parameters.AddWithValue("@eventid", eventMember.EventID);

                try
                {
                    return command.ExecuteNonQuery() == 1;

                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception while trying to remove participant in event. The exception was: '{ex.Message}'", ex);
                }
            }
        }
        #endregion

        #region Helper Methods
        protected EventMember DataReaderRowToEventMember(SqlDataReader reader)
        {
            EventMember eventMember = new EventMember();
            eventMember.EventID = (int)reader["EventID"];
            eventMember.MemberID = (int)reader["MemberID"];


            return eventMember;
        }
        #endregion
    }
}
