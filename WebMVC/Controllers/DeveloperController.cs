using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMVC.RestClientLayer;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class DeveloperController : Controller
    {
        ApiDeveloperDataAccess _dataAccess = new("https://localhost:7023/api/v1/developer");
        
        // GET: Developer
        // Returns a list view after taking an IEnumerable<Developer> as a parameter
        public ActionResult Index()
        {
            return View(_dataAccess.GetAllDevelopers());
        }
    }
}