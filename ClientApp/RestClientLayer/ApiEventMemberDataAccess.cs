using RestSharp;
using ClientApp.ModelLayer;

namespace ClientApp.RestClientLayer
{
    internal class ApiEventMemberDataAccess
    {
        public string BaseUri { get; private set; }

        private RestClient RestClient { get; set; }

        public ApiEventMemberDataAccess(string baseUri)
        {
            BaseUri = baseUri;
            RestClient = new RestClient(baseUri);
            RestClient.AddDefaultHeader("Authorization", Token.Value);
        }


        public bool RemoveMemberFromEvent(EventMember em)
        {
            var request = new RestRequest();
            request.AddJsonBody(em);
            var response = RestClient.Delete<bool>(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error deleting event member with id {em.MemberID}. Message was {response.StatusDescription}");
            }
            return response.Data;
        }

        public IEnumerable<int> FindMembersFromEventId(int eventId)
        {
            var request = new RestRequest("event/"+eventId.ToString());
            var response = RestClient.Execute<IEnumerable<int>>(request).Data;
            return response;
        }
    }
}
