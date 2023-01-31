using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IO;
using System.Security.Principal;
using WebApi.DataAccessLayer;
using WebApi.ModelLayer;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GameController : ControllerBase
    {
        #region Properties
        const string baseURI = "api/v1/game";
        private IGameDataAccess DataAccessLayer { get; set; }
        #endregion

        #region Constructor
        public GameController(IGameDataAccess dataAccessLayer)
        {
            DataAccessLayer = dataAccessLayer;
        }
        #endregion

        #region RESTful CRUD methods
        [HttpGet]
        [Authorize(Roles = "Maintenance,User")]
        public ActionResult<IEnumerable<Game>> GetAll()
        {
			return Ok(DataAccessLayer.GetAllGames());
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Maintenance,User")]
        public ActionResult<Game> GetGameUsingId(int id)
        {
            Game game = DataAccessLayer.FindGameFromId(id);
            if (game == null)
            {
                return NotFound();  //returns 404
            }
            return Ok(game); //returns 200 + account JSON as body
        }

        [HttpGet]
        [Route("client/{developerId}")]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<IEnumerable<Game>> GetGamesByDeveloperId(int developerId)
        {
           return Ok(DataAccessLayer.GetGamesByDeveloperId(developerId));
        }

        [HttpGet]
		[Route("file/{gameId}")]
        [Authorize(Roles = "Maintenance,User")]
        public ActionResult<Game> GetGameFileById(int gameId)
        {
            Game gameFile = DataAccessLayer.GetGameFileById(gameId);
            return Ok(gameFile);
        }

        [HttpPut]
        [Authorize(Roles = "Maintenance")]
        public ActionResult UpdateAllGameDetails(Game game)
        {
            if (!DataAccessLayer.UpdateAllGameDetails(game))
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<Game> AddGame(Game game)
        {
            DataAccessLayer.CreateGame(game);
            return Created($"{baseURI}/{game.GameID}", game);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Maintenance")]
        public ActionResult DeleteGame(int id)
        {
            if (!DataAccessLayer.DeleteGame(id))
            {
                return NotFound();  //returns 404
            }
            return Ok();    //returns 200
        }

        

        /*[HttpPut]
        [Route("update/gamefile")]
        public ActionResult UpdateGameFile(Game game)
        {
            if (!DataAccessLayer.UpdateGameFile(game))
            {
                return NotFound();
            }
            return Ok();
        }*/
        #endregion
    }
}