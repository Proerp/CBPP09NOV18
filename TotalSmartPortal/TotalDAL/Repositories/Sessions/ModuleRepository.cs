using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

using TotalModel.Models;
using TotalCore.Repositories.Sessions;

namespace TotalDAL.Repositories.Sessions
{
    public class ModuleRepository : BaseRepository, IModuleRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public ModuleRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IQueryable<Module> GetAllModules()
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            var allModules = this.totalSmartPortalEntities.Modules.Where(w => w.InActive == 0);
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;
            return allModules;
        }

        public Module GetModuleByID(int moduleID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            var module = this.totalSmartPortalEntities.Modules.SingleOrDefault(x => x.ModuleID == moduleID);
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;
            return module;
        }

        //Cai nay su dung tam thoi, cho cai menu ma thoi. Cach lam nay amatuer qua!!!!
        public string GetLocationName(int userID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            var organizationalUnitUser = this.totalSmartPortalEntities.OrganizationalUnitUsers.Where(w => w.UserID == userID && !w.InActive).Include(i => i.OrganizationalUnit.Location).First();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;
            return organizationalUnitUser.OrganizationalUnit.Location.OfficialName;
        }
        public int GetLocationID(int userID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            var organizationalUnitUser = this.totalSmartPortalEntities.OrganizationalUnitUsers.Where(w => w.UserID == userID && !w.InActive).Include(i => i.OrganizationalUnit.Location).First();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;
            return organizationalUnitUser.OrganizationalUnit.Location.LocationID;
        }

    }
}
