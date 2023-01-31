using System;
using System.Linq;
using RestSharp;
using WebMVC.Models;

namespace WebMVC.RestClientLayer
{
	public class ApiEventDataAccess
	{
		public string BaseUri { get; private set; }
		public RestClient RestClient { get; set; }

		public ApiEventDataAccess(string baseUri)
		{
			BaseUri = baseUri;
			RestClient = new(BaseUri);
            RestClient.AddDefaultHeader("Authorization", Token.Value);
        }

		public IEnumerable<Event> GetAllEvents()
		{
			var response = RestClient.Execute<IEnumerable<Event>>(new RestRequest());
			return response.Data;
		}

		public Event GetUpcomingEvent()
		{
			var response = RestClient.Execute<Event>(new RestRequest("upcoming"));
			return response.Data;
		}
	}
}

