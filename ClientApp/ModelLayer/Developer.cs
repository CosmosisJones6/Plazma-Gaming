namespace ClientApp.ModelLayer
{
    public class Developer
    {
        public Developer()
        {

        }
        public Developer(string name, string email, string description)
        {
            Name = name;
            Email = email;
            Description = description;
        }
        public Developer(int id, string name, string email, string description)
        {
            DeveloperID = id;
            Name = name;
            Email = email;
            Description = description;
        }
        #region Properties
        public int DeveloperID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        #endregion

    }
}
