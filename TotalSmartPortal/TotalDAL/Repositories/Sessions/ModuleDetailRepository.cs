using TotalCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TotalModel.Models;
using TotalCore.Repositories.Sessions;

namespace TotalDAL.Repositories.Sessions
{    
    public class ModuleDetailRepository : IModuleDetailRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public ModuleDetailRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IQueryable<ModuleDetail> GetAllModuleDetails()
        {
            return this.totalSmartPortalEntities.ModuleDetails;
        }

        public IQueryable<ModuleDetail> GetModuleDetailByModuleID(int moduleID)
        {
            return this.totalSmartPortalEntities.ModuleDetails.Where(x => x.ModuleID == moduleID && x.InActive == 0);
        }

        public ModuleDetail GetModuleDetailByID(int taskID)
        {
            return this.totalSmartPortalEntities.ModuleDetails.SingleOrDefault(x => x.TaskID == taskID);
        }

        public void AddModuleDetail(ModuleDetail moduleDetail)
        {
            this.totalSmartPortalEntities.ModuleDetails.Add(moduleDetail);
        }

        public void Add(ModuleDetail moduleDetail)
        {
            this.totalSmartPortalEntities.ModuleDetails.Add(moduleDetail);
        }

        public void Remove(ModuleDetail moduleDetail)
        {
            this.totalSmartPortalEntities.ModuleDetails.Remove(moduleDetail);
        }

        public void SaveChanges()
        {
            this.totalSmartPortalEntities.SaveChanges();
        }

    }
}
