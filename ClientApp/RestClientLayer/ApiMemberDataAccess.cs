using RestSharp;
using ClientApp.ModelLayer;

namespace ClientApp.RestClientLayer
{
    internal class ApiMemberDataAccess
    {
        public string BaseUri { get; private set; }

        private RestClient RestClient { get; set; }

        public ApiMemberDataAccess(string baseUri)
        {
            BaseUri = baseUri;
            RestClient = new RestClient(baseUri);
            RestClient.AddDefaultHeader("Authorization", Token.Value);
        }

        public Member FindMemberFromId(int memberId)
        {
            var request = new RestRequest(memberId.ToString());
            var response = RestClient.Execute<Member>(request).Data;
            return response;
        }

        public IEnumerable<Member> GetAllMembers()
        {
            var request = new RestRequest();
            var response = RestClient.Execute<IEnumerable<Member>>(request).Data;
            return response;
        }
    }
}
