namespace WebMVC.Models
{
	public class Sale
	{

		public Sale()
		{

		}

		public Sale(Guid gameKey, int gameId, string email, DateTime date, float salesPrice)
		{
			GameKey = gameKey;
			GameID = gameId;
			Email = email;
			Date = date;
			SalesPrice = salesPrice;
		}

		public Sale(int gameId, string email, DateTime date, float salesPrice)
		{
			GameID = gameId;
			Email = email;
			Date = date;
			SalesPrice = salesPrice;
		}

		public Sale(PartialSale ps)
		{
			GameID = ps.GameID;
			Email = ps.Email;
			Date = DateTime.Now;
			SalesPrice = ps.SalesPrice;
		}

		#region Properties
		public Guid GameKey { get; set; }
		public int GameID { get; set; }
		public string Email { get; set; }
		public DateTime Date { get; set; }
		public float SalesPrice { get; set; }
		#endregion
	}
}
