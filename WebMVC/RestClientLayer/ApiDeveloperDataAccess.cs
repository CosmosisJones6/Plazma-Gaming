using System;
using NuGet.Common;
using RestSharp;
using WebMVC.Models;
using Microsoft.Extensions.Configuration;
using Token = WebMVC.Models.Token;

namespace WebMVC.RestClientLayer
{
	public class ApiDeveloperDataAccess
	{
		public string BaseUri { get; private set; }
		public RestClient RestClient { get; set; }

		public ApiDeveloperDataAccess(string baseUri)
		{
            BaseUri = baseUri;
			RestClient = new(BaseUri);
			RestClient.AddDefaultHeader("Authorization", Token.Value);
        }

		public IEnumerable<Developer> GetAllDevelopers()
		{
			var response = RestClient.Execute<IEnumerable<Developer>>(new RestRequest());
			return response.Data;
		}
	}
}

