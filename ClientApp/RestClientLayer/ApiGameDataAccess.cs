using ClientApp.ModelLayer;
using Newtonsoft.Json;
using RestSharp;

namespace ClientApp.RestClientLayer
{
    internal class ApiGameDataAccess
    {
        public string BaseUri { get; private set; }

        private RestClient RestClient { get; set; }

        public ApiGameDataAccess(string baseUri)
        {
            BaseUri = baseUri;
            RestClient = new RestClient(baseUri);
            RestClient.AddDefaultHeader("Authorization", Token.Value);
        }
        //using different serialization than Restsharp already has because of the byte array serialization problem with Restsharp
        public bool CreateGame(Game game)
        {
            var request = new RestRequest();
            // Serialize the object to JSON using a different library
            var json = JsonConvert.SerializeObject(game);
            // Add the JSON string to the request body
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            return RestClient.Post<bool>(request).Data;
        }
        public void UpdateGame(Game g)
        {
            var request = new RestRequest();
            // Serialize the object to JSON using a different library
            var json = JsonConvert.SerializeObject(g);
            // Add the JSON string to the request body
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            var response = RestClient.Put<bool>(request);

        }

        public IEnumerable<Game> GetGamesByDeveloperId(int developerId)
        {
            var request = new RestRequest("client/" + developerId.ToString());
            var response = RestClient.Execute<IEnumerable<Game>>(request);
            return response.Data;
        }

        public IEnumerable<Game> GetAllGames()
        {
            var request = new RestRequest();
            var response = RestClient.Execute<IEnumerable<Game>>(request);
            return response.Data;
        }

        public Game GetGameUsingId(int gameId)
        {
            var request = new RestRequest(gameId.ToString());
            var response = RestClient.Execute<Game>(request);
            return response.Data;
        }

        public Game GetGameFileByGameId(int gameId)
        {
            var request = new RestRequest("file/" + gameId.ToString());
            var response = RestClient.Execute<Game>(request);
            var keyResponse = JsonConvert.DeserializeObject<Game>(response.Content);
            return keyResponse;
        }

        public void DeleteGame(int id)
        {
            var request = new RestRequest(id.ToString());
            request.AddJsonBody(id);
            var response = RestClient.Delete<bool>(request);
        }
    }
}
