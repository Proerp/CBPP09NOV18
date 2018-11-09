using System.Linq;

using TotalModel.Models;

namespace TotalCore.Repositories.Sessions
{
    public interface IModuleRepository : IBaseRepository
    {
        IQueryable<Module> GetAllModules();

        Module GetModuleByID(int moduleID);
        string GetLocationName(int userID);
        int GetLocationID(int userID);
    }
}
