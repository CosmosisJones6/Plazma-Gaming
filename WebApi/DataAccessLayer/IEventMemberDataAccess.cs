using WebApi.ModelLayer;

namespace WebApi.DataAccessLayer
{
    public interface IEventMemberDataAccess
    {
        public bool JoinEvent(EventMember eventMember);
        public IEnumerable<int> GetEventIdListByMemberId(int eventId);
        public IEnumerable<int> GetMemberIdListByEventId(int eventId);
        public bool RemoveMemberFromEvent(EventMember eventMember);
    }
}
