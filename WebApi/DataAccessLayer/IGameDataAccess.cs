using Microsoft.AspNetCore.Mvc;
using WebApi.ModelLayer;

namespace WebApi.DataAccessLayer
{
    public interface IGameDataAccess
    {
        public bool CreateGame(Game game);
        public Game FindGameFromId(int gameId);
        public IEnumerable<Game> GetAllGames();
        public bool UpdateAllGameDetails(Game game);
        public Game GetGameFileById(int gameId);
        public bool UpdateGameFile(Game game);
        //public FileResult Download(int gameId);
        public bool DeleteGame(int id);
        public IEnumerable<Game> GetGamesByDeveloperId(int developerId);


    }
}
