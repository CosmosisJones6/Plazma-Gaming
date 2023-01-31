using System;
using System.Security.Principal;
using System.Text.Json;
using RestSharp;
using RestSharp.Serializers;
using WebMVC.Models;

namespace WebMVC.RestClientLayer
{
	public class ApiSaleDataAccess
	{
		public string BaseUri { get; private set; }
		public RestClient RestClient { get; set; }

		public ApiSaleDataAccess(string baseUri)
		{
			BaseUri = baseUri;
			RestClient = new(BaseUri);
            RestClient.AddDefaultHeader("Authorization", Token.Value);
        }

		public string CreateSale(Sale sale)
		{
			string json = JsonSerializer.Serialize(sale);
			var request = new RestRequest("", Method.Post);
			request.AddStringBody(json, DataFormat.Json);
			return RestClient.Post<Sale>(request).GameKey.ToString();
		}
	}
}

