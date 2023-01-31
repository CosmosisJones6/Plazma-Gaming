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

	public class GameController : Controller
    {

        ApiGameDataAccess _dataAccess = new("https://localhost:7023/api/v1/game");

        // GET: Game
        // Returns a lsit view of games after taking an IEnmureable<Game> as parameter
        public ActionResult Index()
        {
            return View(_dataAccess.GetAllGames());
        }

        // GET: Game/Details/5
        // Returns a detail view of a game after taking a Game object as parameter
        public ActionResult Details(int id)
        {
            return View(_dataAccess.GetGameUsingId(id));
        }

        // Called when a game is bought or download link is clicked on the website
        [HttpGet]
        public FileResult DownloadGame(int gameId)
		{
            Game gameToDownload = _dataAccess.GetGameFileById(gameId);
            return File(gameToDownload.FileContent, System.Net.Mime.MediaTypeNames.Application.Octet, gameToDownload.FileName);
		}
    }
}