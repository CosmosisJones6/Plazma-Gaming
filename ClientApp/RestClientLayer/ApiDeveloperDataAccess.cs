using ClientApp.ModelLayer;
using Microsoft.Extensions.Configuration;
using RestSharp;


namespace ClientApp.RestClientLayer
{
    public class ApiDeveloperDataAccess
    {
        public string BaseUri { get; private set; }
        private RestClient RestClient { get; set; }

        public ApiDeveloperDataAccess(string baseUri)
        {
            BaseUri = baseUri;
            RestClient = new RestClient(baseUri);
            RestClient.AddDefaultHeader("Authorization", Token.Value);
        }

        public bool CreateDeveloper(Developer developer)
        {
            var request = new RestRequest();
            request.AddJsonBody(developer);
            return RestClient.Post<bool>(request).Data;
        }

        public bool DeleteDeveloper(Developer developer)
        {
            var request = new RestRequest();
            request.AddJsonBody(developer);
            var response = RestClient.Delete<bool>(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error deleting developer with id {developer.DeveloperID}. Message was {response.StatusDescription}");
            }
            return response.Data;
        }

        public Developer FindDeveloperFromId(int developerId)
        {
            var request = new RestRequest(developerId.ToString());
            var response = RestClient.Execute<Developer>(request).Data;
            return response;
        }

        public IEnumerable<Developer> GetAllDevelopers()
        {
            var request = new RestRequest();
            var response = RestClient.Execute<IEnumerable<Developer>>(request);
            return response.Data;
        }

        public bool UpdateDeveloper(Developer developer)
        {
            var request = new RestRequest();
            request.AddJsonBody(developer);
            var response = RestClient.Put<bool>(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error updating developer {developer}. Message was {response.StatusDescription}");
            }
            return response.Data;
        }
    }
}
