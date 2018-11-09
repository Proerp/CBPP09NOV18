using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Inventories;

namespace TotalDAL.Repositories.Inventories
{
    public class MaterialIssueRepository : GenericWithDetailRepository<MaterialIssue, MaterialIssueDetail>, IMaterialIssueRepository
    {
        public MaterialIssueRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "MaterialIssueEditable", "MaterialIssueApproved")
        {
        }
    }








    public class MaterialIssueAPIRepository : GenericAPIRepository, IMaterialIssueAPIRepository
    {
        public MaterialIssueAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetMaterialIssueIndexes")
        {
        }

        public IEnumerable<MaterialIssuePendingFirmOrder> GetFirmOrders(int? locationID, int? firmOrderID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<MaterialIssuePendingFirmOrder> pendingFirmOrders = base.TotalSmartPortalEntities.GetMaterialIssuePendingFirmOrders(locationID, firmOrderID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingFirmOrders;
        }

        public IEnumerable<MaterialIssuePendingFirmOrderMaterial> GetPendingFirmOrderMaterials(int? locationID, int? materialIssueID, int? firmOrderID, int? warehouseID, string goodsReceiptDetailIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<MaterialIssuePendingFirmOrderMaterial> pendingFirmOrderDetails = base.TotalSmartPortalEntities.GetMaterialIssuePendingFirmOrderMaterials(locationID, materialIssueID, firmOrderID, warehouseID, goodsReceiptDetailIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingFirmOrderDetails;
        }

    }


}
