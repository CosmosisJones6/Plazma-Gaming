using ClientApp.ModelLayer;
using Newtonsoft.Json;
using RestSharp;
using System.Reflection;

namespace ClientApp.RestClientLayer
{
    internal class ApiLoginDataAccess
    {
        public string BaseUri { get; private set; }

        private RestClient RestClient { get; set; }

        public ApiLoginDataAccess(string baseUri)
        {
            BaseUri = baseUri;
            RestClient = new RestClient(baseUri);
            RestClient.AddDefaultHeader("Authorization", Token.Value);
        }

        public IEnumerable<Login> GetAllLogins()
        {
            var request = new RestRequest();
            var response = RestClient.Execute<IEnumerable<Login>>(request);
            return response.Data;
        }

        public bool ValidateLogin(Login incomingLogin)
        {
            var request = new RestRequest("validate");
            request.AddJsonBody(incomingLogin);
            var response = RestClient.Post<bool>(request);
            return response.Data;
        }

        public bool AddLogin(Login login)
        {
            var request = new RestRequest();
            request.AddJsonBody(login);
            var response = RestClient.Post<bool>(request);
            return response.Data;
        }

        public bool UpdateLogin(Login login)
        {
            var request = new RestRequest();
            request.AddJsonBody(login);
            var response = RestClient.Put<bool>(request);
            return response.Data;
        }

        public bool DeleteLogin(Login login)
        {
            var request = new RestRequest();
            request.AddJsonBody(login);
            var response = RestClient.Delete<bool>(request);
            return response.Data;
        }
        
    }
}
