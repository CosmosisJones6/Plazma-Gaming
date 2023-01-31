using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApi.DataAccessLayer;
using WebApi.ModelLayer;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EventMemberController : ControllerBase
    {
        #region Properties
        const string baseURI = "api/v1/eventmember";
        private IEventMemberDataAccess DataAccessLayer { get; set; }

        #endregion

        #region Constructor
        public EventMemberController(IEventMemberDataAccess dataAccessLayer)
        {
            DataAccessLayer = dataAccessLayer;
        }
        #endregion

        [HttpGet]
        [Route("event/{eventid}")]
        [Authorize(Roles = "Maintenance,User")]
        public ActionResult<IEnumerable<int>> GetMemberIdListByEventId(int eventId)
        {
            IEnumerable<int> memberId = DataAccessLayer.GetMemberIdListByEventId(eventId);
            if (memberId == null)
            {
                return NotFound();  //returns 404
            }
            return Ok(memberId); //returns 200 + account JSON as body
        }

        [HttpGet]
        [Route("member/{memberid}")]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<IEnumerable<int>> GetEventIdListByMemberId(int memberId)
        {
            IEnumerable<int> eventId = DataAccessLayer.GetEventIdListByMemberId(memberId);
            if (eventId == null)
            {
                return NotFound();  //returns 404
            }
            return Ok(eventId); //returns 200 + account JSON as body
        }

        
        [HttpPost]
        [Authorize(Roles = "Maintenance,User")]
        public ActionResult<EventMember> JoinEvent(EventMember eventMember)
        {
            DataAccessLayer.JoinEvent(eventMember);
            return Created($"{baseURI}/{eventMember.MemberID}", eventMember);
        }

        
        [HttpDelete]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<EventMember> RemoveMemberFromEvent(EventMember eventMember)
        {
            if (!DataAccessLayer.RemoveMemberFromEvent(eventMember))
            {
                return NotFound();  //returns 404
            }
            return Ok();
        }
    }
}
