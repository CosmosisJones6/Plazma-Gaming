namespace WebApi.ModelLayer
{
    public class EventMember
    {
        public int EventID { get; set; }
        public int MemberID { get; set; }

        public EventMember()
        {

        }
        
        public EventMember(int eventId, int memberId)
        {
            EventID = eventId;
            MemberID = memberId;
        }
        public EventMember(int eventId)
        {
            EventID = eventId;
        }
    }
}
