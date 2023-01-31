using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMVC.RestClientLayer;
using WebMVC.Models;
using Newtonsoft.Json;

namespace MVC.Controllers
{
	/*
	 * Has no associated views, only used for creating Sale objects when a purchase is made
	*/
	public class SaleController : Controller
	{
		ApiSaleDataAccess _dataAccess = new("https://localhost:7023/api/v1/sale");

		[HttpPost]
		public void CreateSale([FromBody] PartialSale saleJson)
		{
			Sale sale = new(saleJson);
			_dataAccess.CreateSale(sale);
		}
	}

}