using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalBase.Enums;
using TotalDTO.Helpers;

namespace TotalDTO.Productions
{
    public class WorkOrderDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.WorkOrderDetailID; }

        public int WorkOrderDetailID { get; set; }
        public int WorkOrderID { get; set; }

        public int ProductionOrderID { get; set; }
        public int ProductionOrderDetailID { get; set; }

        public int PlannedOrderID { get; set; }
        public int FirmOrderID { get; set; }
        public int FirmOrderMaterialID { get; set; }

        public GlobalEnums.NmvnTaskID NMVNTaskID { get; set; }

        public int BomID { get; set; }
        public int BomDetailID { get; set; }
        [Display(Name = "Trục")]
        [UIHint("StringReadonly")]
        public string LayerCode { get; set; }

        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> WarehouseID { get; set; }

        [UIHint("StringReadonly")]
        public override string CommodityCode { get; set; }     

        [Display(Name = "Tồn kho")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityAvailables { get; set; }

        [UIHint("QuantityReadonly")]
        public override decimal Quantity { get; set; }
    }
}
