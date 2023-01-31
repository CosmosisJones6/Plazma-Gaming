namespace ClientApp.ModelLayer
{
    public class Sale
    {
        #region Properties
        public string GameKey { get; set; }
        public int GameID { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public float SalesPrice { get; set; }
        #endregion
    }
}
