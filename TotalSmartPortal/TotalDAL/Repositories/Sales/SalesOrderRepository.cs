﻿using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Sales;


namespace TotalDAL.Repositories.Sales
{
    public class SalesOrderRepository : GenericWithDetailRepository<SalesOrder, SalesOrderDetail>, ISalesOrderRepository
    {
        public SalesOrderRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "SalesOrderEditable", "SalesOrderApproved", null, "SalesOrderVoidable")
        {
        }
    }








    public class SalesOrderAPIRepository : GenericAPIRepository, ISalesOrderAPIRepository
    {
        public SalesOrderAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetSalesOrderIndexes")
        {
        }
    }


}
