using System;
namespace WebMVC.Models
{
	public class EventMember
	{
		public int EventId { get; set; }
		public int MemberId { get; set; }

		public EventMember()
		{

		}

		public EventMember(int eventId, int memberId)
		{
			EventId = eventId;
			MemberId = memberId;
		}
		public EventMember(int eventId)
		{
			EventId = eventId;
		}
	}
}

