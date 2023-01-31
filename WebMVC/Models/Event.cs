using System;
namespace WebMVC.Models
{
	public class Event
	{
		public int EventID { get; set; }
		public int GameID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int Capacity { get; set; }
	}
}

