namespace ClientApp.ModelLayer
{
    public class Event
    {
        #region Properties
        public int EventID { get; set; }
        public int GameID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        #endregion

        public Event()
        {

        }

        public Event(int eventId, int gameId, string name, string description, DateTime startDate, DateTime endDate, int capacity)
        {
            EventID = eventId;
            GameID = gameId;
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Capacity = capacity;
        }

        public Event(int gameId, string name, string description, DateTime startDate, DateTime endDate, int capacity)
        {
            GameID = gameId;
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Capacity = capacity;
        }

    }
}
