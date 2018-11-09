using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;

namespace TotalDTO.Purchases
{
    public class PurchaseRequisitionDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.PurchaseRequisitionDetailID; }

        public int PurchaseRequisitionDetailID { get; set; }
        public int PurchaseRequisitionID { get; set; }

        public int CustomerID { get; set; }
        
        //[Display(Name = "Mã CK")]
        [UIHint("AutoCompletes/CommodityBase")]
        public override string CommodityCode { get; set; }

        public string VoidTypeCode { get; set; }
        [Display(Name = "Lý do")]
        [UIHint("AutoCompletes/VoidTypeBase")]
        public string VoidTypeName { get; set; }
        public Nullable<int> VoidClassID { get; set; }

        [UIHint("Quantity")]
        [Range(0, 99999999999, ErrorMessage = "Số lượng không hợp lệ")]
        public override decimal Quantity { get; set; }
    }
}
