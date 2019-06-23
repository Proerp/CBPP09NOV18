using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;
using TotalDTO.Helpers;
using TotalDTO.Commons;

namespace TotalDTO.Purchases
{
    public class LabPrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public override bool EditAfterUnApprove { get { return false; } }
        public override string VoidWarning { get { return "Quarantine"; } }
        public override string ApproveWarning { get { return "PASS"; } }
        public override string UnApproveWarning { get { return "CHƯA PASS"; } }

        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Lab; } }

        public int GetID() { return this.LabID; }
        public void SetID(int id) { this.LabID = id; }

        public int LabID { get; set; }

        [Display(Name = "Lab code")]
        public string Code { get; set; }

        [Display(Name = "Phiếu nhập hàng")]
        public string GoodsArrivalReference { get; set; }

        [Display(Name = "Lab cont")]
        public string SealCodes { get; set; }
        [Display(Name = "Số lô")]
        public string BatchCodes { get; set; }

        [Display(Name = "Mã hàng")]
        public string CommodityCodes { get; set; }
        [Display(Name = "Tên hàng")]
        public string CommodityNames { get; set; }

        [Display(Name = "Số lượng kiện")]
        public decimal TotalPackages { get; set; }

        [Display(Name = "Khối lượng")]
        public decimal TotalQuantity { get; set; }


        public override Nullable<int> VoidTypeID { get { return (this.VoidType != null ? this.VoidType.VoidTypeID : null); } }
        [UIHint("AutoCompletes/VoidType")]
        public VoidTypeBaseDTO VoidType { get; set; }

        public bool Holdable { get; set; }
        public bool Releasable { get; set; }
        public virtual bool Hold { get; set; }
    }

    public class LabDTO : LabPrimitiveDTO
    {
    }
}