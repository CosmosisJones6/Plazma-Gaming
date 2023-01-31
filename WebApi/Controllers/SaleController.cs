using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApi.DataAccessLayer;
using WebApi.ModelLayer;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SaleController : ControllerBase
    {
        #region Properties
        const string baseURI = "api/v1/sale";
        private ISaleDataAccess DataAccessLayer { get; set; }
        #endregion

        #region Constructor
        public SaleController(ISaleDataAccess dataAccessLayer)
        {
            DataAccessLayer = dataAccessLayer;
        }
        #endregion

        #region RESTful CRUD methods
        [HttpGet]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<IEnumerable<Sale>> GetAll()
        {
            return Ok(DataAccessLayer.GetAllSales());
        }

        [HttpGet]
        [Route("{gamekey}")]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<Sale> GetSaleUsingGameKey(Guid gamekey)
        {
            Sale sale = DataAccessLayer.FindSaleFromGameKey(gamekey);
            if (sale == null)
            {
                return NotFound();  //returns 404
            }
            return Ok(sale); //returns 200 + account JSON as body
        }

        [HttpGet]
        [Route("byGame/{gameid}")]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<Sale> SalesByGame(int gameId)
        {
            IEnumerable<Sale> sales = DataAccessLayer.SalesByGame(gameId);
            if (sales == null)
            {
                return NotFound();  //returns 404
            }
            return Ok(sales); //returns 200 + account JSON as body
        }

        [HttpGet]
        [Route("{starttime}/{endtime}")]
        [Authorize(Roles = "Maintenance")]
        public ActionResult<Sale> SalesInPeriod(DateTime startTime, DateTime endTime)
        {
            IEnumerable<Sale> sales = DataAccessLayer.SalesInPeriod(startTime, endTime);
            if (sales == null)
            {
                return NotFound();  //returns 404
            }
            return Ok(sales); //returns 200 + account JSON as body
        }

        [HttpPost]
        [Authorize(Roles = "Maintenance,User")]
        public ActionResult<Sale> AddSale(Sale sale)
        {
            DataAccessLayer.CreateSale(sale);
            return Created($"{baseURI}/{sale.GameID}", sale);
        }

        [HttpDelete]
        [Route("{gamekey}")]
        [Authorize(Roles = "Maintenance")]
        public ActionResult DeleteSale(Guid gamekey)
        {
            if (!DataAccessLayer.DeleteSale(gamekey))
            {
                return NotFound();  //returns 404
            }
            return Ok();    //returns 200
        }
        #endregion
    }
}