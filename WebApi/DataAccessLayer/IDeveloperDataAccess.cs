using WebApi.ModelLayer;

namespace WebApi.DataAccessLayer
{
    public interface IDeveloperDataAccess
    {
        public bool CreateDeveloper(Developer developer);
        public Developer FindDeveloperFromId(int developerId);
        public IEnumerable<Developer> GetAllDevelopers();
        public bool UpdateDeveloper(Developer developer);
        public bool DeleteDeveloper(Developer developer);
    }
}
