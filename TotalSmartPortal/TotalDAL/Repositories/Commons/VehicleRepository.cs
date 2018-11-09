using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public VehicleRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<Vehicle> GetAllVehicles()
        {
            return this.totalSmartPortalEntities.Vehicles.ToList();
        }
    }
}