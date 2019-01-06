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
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Lab; } }

        public int GetID() { return this.LabID; }
        public void SetID(int id) { this.LabID = id; }

        public int LabID { get; set; }

        [Display(Name = "Mã nhân viên")]
        [Required(ErrorMessage = "Vui lòng nhập mã nhân viên")]
        public string Code { get; set; }

        public string GoodsArrivalReference { get; set; }
        
        public string SealCodes { get; set; }
        public string BatchCodes { get; set; }

        public string CommodityCodes { get; set; }
        public string CommodityNames { get; set; }
    }

    public class LabDTO : LabPrimitiveDTO
    {
    }
}