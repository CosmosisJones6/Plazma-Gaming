using WebApi.ModelLayer;

namespace WebApi.DataAccessLayer
{
    public interface IEventDataAccess
    {
        public bool CreateEvent(Event e);
        public Event FindEventFromId(int eventId);
        public IEnumerable<Event> GetAllEvents();
        public bool UpdateEvent(Event e);
        public bool DeleteEvent(Event e);
        public Event FindUpcomingEvent();
    }
}
