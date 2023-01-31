namespace ClientApp.ModelLayer
{
    public class Game
    {
        //empty
        public Game()
        {

        }
        public Game(string fileName, byte[] fileContent)
        {
            FileName = fileName;
            FileContent = fileContent;
        }
        // full for game table
        public Game(int gameId, int developerID, string title, string description, int yearOfRelease, string specifications, string type, float price)
        {
            GameID = gameId;
            DeveloperID = developerID;
            Title = title;
            Description = description;
            YearOfRelease = yearOfRelease;
            Specifications = specifications;
            Type = type;
            Price = price;
        }
        //without game ID for game table
        public Game(int developerID, string title, string description, int yearOfRelease, string specifications, string type, float price)
        {
            DeveloperID = developerID;
            Title = title;
            Description = description;
            YearOfRelease = yearOfRelease;
            Specifications = specifications;
            Type = type;
            Price = price;
        }
        // full for both tables
        public Game(int gameId, int developerID, string title, string description, int yearOfRelease, string specifications, string type, float price, string fileName, byte[] fileContent)
        {
            GameID = gameId;
            DeveloperID = developerID;
            Title = title;
            Description = description;
            YearOfRelease = yearOfRelease;
            Specifications = specifications;
            Type = type;
            Price = price;
            FileName = fileName;
            FileContent = fileContent;
        }
        // without game ID for both tables
        public Game(int developerID, string title, string description, int yearOfRelease, string specifications, string type, float price, string fileName, byte[] fileContent)
        {
            DeveloperID = developerID;
            Title = title;
            Description = description;
            YearOfRelease = yearOfRelease;
            Specifications = specifications;
            Type = type;
            Price = price;
            FileName = fileName;
            FileContent = fileContent;
        }


        #region Properties
        public int GameID { get; set; }
        public int DeveloperID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int YearOfRelease { get; set; }
        public string Specifications { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        #endregion

    }
}
