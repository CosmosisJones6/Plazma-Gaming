using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApi.DataAccessLayer;
using WebApi.ModelLayer;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    
    public class EventController : ControllerBase
    {
        #region Properties
        const string baseURI = "api/v1/event";
        private IEventDataAccess DataAccessLayer { get; set; }

        #endregion

        #region Constructor
        public EventController(IEventDataAccess dataAccessLayer)
        {
            DataAccessLayer = dataAccessLayer;
        }
        #endregion

        #region RESTful CRUD methods
        [HttpGet]
        [Authorize(Roles = "Maintenance,User")]
        public ActionResult<IEnumerable<Event>> GetAllEvents()
        {
            return Ok(DataAccessLayer.GetAllEvents());
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<Event> GetEventUsingId(int id)
        {
            Event e = DataAccessLayer.FindEventFromId(id);
            if (e == null)
            {
                return NotFound();  //returns 404
            }
            return Ok(e); //returns 200 + account JSON as body
        }

        [HttpPost]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<Event> AddEvent(Event e)
        {
            DataAccessLayer.CreateEvent(e);
            return Created($"{baseURI}/{e.EventID}", e);
        }

        [HttpDelete]
        [Authorize(Roles = "Maintenance")]
        public ActionResult DeleteEvent(Event e)
        {
            if (!DataAccessLayer.DeleteEvent(e))
            {
                return NotFound();  //returns 404
            }
            return Ok();    //returns 200
        }

        [HttpPut]
        [Authorize(Roles = "Maintenance")]
        public ActionResult UpdateEvent(Event e)
        {
            if (!DataAccessLayer.UpdateEvent(e))
            {
                return NotFound();
            }
            return Ok();
        }

		[HttpGet]
		[Route("upcoming")]
        [Authorize(Roles = "Maintenance,User")]
        public ActionResult<Event> GetUpcomingEvent()
		{
            return Ok(DataAccessLayer.FindUpcomingEvent());
		}
        #endregion
    }
}
