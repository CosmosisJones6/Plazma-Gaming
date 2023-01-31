using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApi.DataAccessLayer;
using WebApi.ModelLayer;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LoginController : ControllerBase
    {
        #region Properties
        const string baseURI = "api/v1/login";
        private ILoginDataAccess DataAccessLayer { get; set; }

        #endregion

        #region Constructor
        public LoginController(ILoginDataAccess dataAccessLayer)
        {
            DataAccessLayer = dataAccessLayer;
        }
        #endregion

        #region RESTful CRUD methods

        [HttpGet]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<IEnumerable<Login>> GetAllLoginInformation()
        {
            return Ok(DataAccessLayer.GetAllLoginInformation());
        }

        [HttpPost]
        [Route("validate")]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<bool> ValidateLogin(Login incomingLogin)
        {
            return Ok(DataAccessLayer.ValidateLogin(incomingLogin));
        }

        [HttpPost]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<Login> AddLogin(Login login)
        {
            DataAccessLayer.CreateLogin(login);
            return Created($"{baseURI}/{login.UserName}", login);
        }

        [HttpDelete]
        [Authorize(Roles = "Maintenance")]
        public ActionResult DeleteLogin(Login login)
        {
            if (!DataAccessLayer.DeleteLogin(login))
            {
                return NotFound();  //returns 404
            }
            return Ok();    //returns 200
        }
        #endregion
    }
}
