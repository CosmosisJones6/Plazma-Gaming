using RestSharp;
using ClientApp.ModelLayer;

namespace ClientApp.RestClientLayer
{
    internal class ApiSaleDataAccess
    {
        public string BaseUri { get; private set; }

        private RestClient RestClient { get; set; }

        public ApiSaleDataAccess(string baseUri)
        {
            BaseUri = baseUri;
            RestClient = new RestClient(baseUri);
            RestClient.AddDefaultHeader("Authorization", Token.Value);
        }

        public List<Sale> GetAllSales()
        {
            var request = new RestRequest();
            var response = RestClient.Execute<List<Sale>>(request);
            return response.Data;
        }
    }
}
