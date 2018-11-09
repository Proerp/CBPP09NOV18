using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class WarehouseRepository : GenericRepository<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities)
        {
        }

        public IList<Warehouse> GetWarehouses(int customerID, string searchText, string warehouseTaskIDList)
        {
            List<Warehouse> warehouses = this.TotalSmartPortalEntities.GetWarehouses(customerID, searchText, warehouseTaskIDList).ToList();

            return warehouses;
        }
    }
}