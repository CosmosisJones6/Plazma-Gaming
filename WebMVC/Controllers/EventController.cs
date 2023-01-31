using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMVC.RestClientLayer;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class EventController : Controller
    {
        // Uses 3 data access obejcts, the Event and Member are needed for functionality, whereas the EventMember accesses the connection table
        ApiEventDataAccess _dataAccessEvent = new("https://localhost:7023/api/v1/Event");
		ApiMemberDataAccess _dataAccessMember = new("https://localhost:7023/api/v1/Member");
		ApiEventMemberDataAccess _dataAccessEventMember = new("https://localhost:7023/api/v1/EventMember");

		// GET: Event
        // Returns a detail view of a single event after taking an Event object as parameter
		[HttpGet]
        public ActionResult Index()
        {
            return View(_dataAccessEvent.GetUpcomingEvent());
        }

		[HttpGet]
        public string GetMemberAmount(int eventId)
		{
            return _dataAccessEventMember.GetMemberAmount(eventId);
		}

        [HttpPost]
        public bool JoinEvent([FromBody] EventJoinData data)
		{
            Member member;
            try
            {
                member = (Member)_dataAccessMember.GetAllMembers().First(m => m.Email == data.MemberEmail && m.Name == data.MemberName); //tries to find a member with matching email and name
            }
            catch(Exception ex) //we don't have a member with the given email address and name
			{
                _dataAccessMember.CreateMember(data.MemberEmail, data.MemberName); //create Member
				member = _dataAccessMember.GetAllMembers().First(m => m.Email == data.MemberEmail && m.Name == data.MemberName); //get Member again, now that it exists
			}
            EventMember eventMember = new(data.EventId, member.MemberId);
            return _dataAccessEventMember.JoinEvent(eventMember);
		}

        // Class for creating objects from fetched data
        public class EventJoinData
		{
			public int EventId { get; private set; }
            public string MemberName { get; private set; }
            public string MemberEmail { get; private set; }
            public EventJoinData(int eventId, string memberEmail, string memberName)
			{
                EventId = eventId;
                MemberEmail = memberEmail;
                MemberName = memberName;
			}
		}
    }
}