using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApi.DataAccessLayer;
using WebApi.ModelLayer;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MemberController : ControllerBase
    {
        #region Properties
        const string baseURI = "api/v1/member";
        private IMemberDataAccess DataAccessLayer { get; set; }

        #endregion

        #region Constructor
        public MemberController(IMemberDataAccess dataAccessLayer)
        {
            DataAccessLayer = dataAccessLayer;
        }
        #endregion

        #region RESTful CRUD methods
        [HttpGet]
        [Authorize(Roles = "Maintenance,User")]
        public ActionResult<IEnumerable<Member>> GetAllMembers()
        {
            return Ok(DataAccessLayer.GetAllMembers());
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<Member> GetMemberUsingId(int id)
        {
            Member member = DataAccessLayer.FindMemberFromId(id);
            if (member == null)
            {
                return NotFound();  //returns 404
            }
            return Ok(member); //returns 200 + account JSON as body
        }

        [HttpPost]
        [Authorize(Roles = "Maintenance,User")]
        public ActionResult<Member> AddMember(Member member)
        {
            DataAccessLayer.CreateMember(member);
            return Created($"{baseURI}/{member.MemberID}", member);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Maintenance")]
        public ActionResult DeleteMember(int memberId)
        {
            if (!DataAccessLayer.DeleteMember(memberId))
            {
                return NotFound();  //returns 404
            }
            return Ok();    //returns 200
        }

        [HttpPut]
        [Authorize(Roles = "Maintenance")]
        public ActionResult UpdateMember(Member member)
        {
            if (!DataAccessLayer.UpdateMember(member))
            {
                return NotFound();
            }
            return Ok();
        }
        #endregion
    }
}
