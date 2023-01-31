using WebApi.ModelLayer;

namespace WebApi.DataAccessLayer
{
    public interface ISaleDataAccess
    {
        public bool CreateSale(Sale sale);
        public Sale FindSaleFromGameKey(Guid gameKey);
        public IEnumerable<Sale> GetAllSales();
        public IEnumerable<Sale> SalesByGame(int gameId);
        public IEnumerable<Sale> SalesInPeriod(DateTime startTime, DateTime endTime);
        public bool DeleteSale(Guid gamekey);
    }
}
