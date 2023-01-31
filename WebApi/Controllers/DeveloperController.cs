using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataAccessLayer;
using WebApi.ModelLayer;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DeveloperController : ControllerBase
    {
        #region Properties
        private IDeveloperDataAccess DataAccessLayer { get; set; }
        const string baseURI = "api/v1/developer";
        #endregion

        #region Constructor
        public DeveloperController(IDeveloperDataAccess dataAccessLayer)
        {
            DataAccessLayer = dataAccessLayer;
        }
        #endregion

        #region RESTful CRUD methods
        [HttpGet]
        [Authorize(Roles = "Maintenance,User")]
        public ActionResult<IEnumerable<Developer>> GetAllDevelopers()
        {
            var returnValue = DataAccessLayer.GetAllDevelopers();
            return Ok(returnValue);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<Developer> GetDeveloperUsingId(int id)
        {
            Developer developer = DataAccessLayer.FindDeveloperFromId(id);
            if (developer == null)
            {
                return NotFound();  //returns 404
            }
            return Ok(developer); //returns 200 + account JSON as body
        }

        [HttpPost]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<Developer> AddDeveloper(Developer developer)
        {
            DataAccessLayer.CreateDeveloper(developer);
            return Created($"{baseURI}/{developer.DeveloperID}", developer);
        }

        [HttpDelete]
        [Authorize(Roles = "Maintenance")]
        public ActionResult DeleteDeveloper(Developer developer)
        {
            if (!DataAccessLayer.DeleteDeveloper(developer))
            {
                return NotFound();  //returns 404
            }
            return Ok();    //returns 200
        }

        [HttpPut]
        [Authorize(Roles = "Maintenance")]
        public ActionResult UpdateDeveloper(Developer developer)
        {
            if (!DataAccessLayer.UpdateDeveloper(developer))
            {
                return NotFound();
            }
            return Ok();
        }
        #endregion
    }
}
