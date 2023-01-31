using ClientApp.ModelLayer;
using RestSharp;

namespace ClientApp.RestClientLayer
{
    internal class ApiEventDataAccess
    {
        public string BaseUri { get; private set; }

        private RestClient RestClient { get; set; }

        public ApiEventDataAccess(string baseUri)
        {
            BaseUri = baseUri;
            RestClient = new RestClient(baseUri);
            RestClient.AddDefaultHeader("Authorization", Token.Value);
        }

        public bool CreateEvent(Event e)
        {
            var request = new RestRequest();
            request.AddJsonBody(e);
            return RestClient.Post<bool>(request).Data;
        }

        public bool DeleteEvent(Event e)
        {
            var request = new RestRequest();
            request.AddJsonBody(e);
            var response = RestClient.Delete<bool>(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error deleting event with id {e.EventID}. Message was {response.StatusDescription}");
            }
            return response.Data;
        }

        public Event FindEventFromId(int eventId)
        {
            var request = new RestRequest(eventId.ToString());
            var response = RestClient.Execute<Event>(request).Data;
            return response;
        }

        public IEnumerable<Event> GetAllEvents()
        {
            var request = new RestRequest();
            var response = RestClient.Execute<IEnumerable<Event>>(request);
            return response.Data;
        }

        public bool UpdateEvent(Event e)
        {
            var request = new RestRequest();
            request.AddJsonBody(e);
            var response = RestClient.Put<bool>(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error updating company {e}. Message was {response.StatusDescription}");
            }
            return response.Data;
        }
    }
}
