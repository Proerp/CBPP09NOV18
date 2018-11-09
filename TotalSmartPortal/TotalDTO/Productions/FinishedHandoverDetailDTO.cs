using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;
using TotalDTO.Commons;

namespace TotalDTO.Productions
{
    public class FinishedHandoverDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.FinishedHandoverDetailID; }

        public int FinishedHandoverDetailID { get; set; }
        public int FinishedHandoverID { get; set; }
       
        public Nullable<int> FinishedProductID { get; set; }
        public Nullable<int> FinishedProductPackageID { get; set; }

        [Display(Name = "KHSX")]
        [UIHint("StringReadonly")]
        public string FirmOrderReference { get; set; }
        [Display(Name = "Số CT")]
        [UIHint("StringReadonly")]
        public string FirmOrderCode { get; set; }
        
        [Display(Name = "Pallet")]
        [UIHint("StringReadonly")]
        public string SemifinishedProductReferences { get; set; }
        [Display(Name = "Ngày ĐG")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> FinishedProductEntryDate { get; set; }

        public int CustomerID { get; set; }
        [Display(Name = "Mã KH")]
        [UIHint("StringReadonly")]
        public string CustomerCode { get; set; }
        [Display(Name = "Tên KH")]
        [UIHint("StringReadonly")]
        public string CustomerName { get; set; }

        [UIHint("StringReadonly")]
        public override string CommodityCode { get; set; }
        [UIHint("StringReadonly")]
        public override string CommodityName { get; set; }

        [Display(Name = "SL")]
        [UIHint("QuantityReadonly")]
        public override decimal Quantity { get; set; }
    }
}
