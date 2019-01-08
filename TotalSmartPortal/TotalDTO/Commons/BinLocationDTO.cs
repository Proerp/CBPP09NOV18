using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Commons
{
    public interface IBinLocationBaseDTO
    {
        int BinLocationID { get; set; }
        [Display(Name = "Vị trí")]
        [UIHint("AutoCompletes/BinLocationBase")]
        [Required(ErrorMessage = "Vui lòng nhập vị trí")]
        string Code { get; set; }
        string Name { get; set; }
    }

    public class BinLocationBaseDTO : BaseDTO, IBinLocationBaseDTO
    {
        public int BinLocationID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class BinLocationPrimitiveDTO : BinLocationBaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.BinLocation; } }

        public int GetID() { return this.BinLocationID; }
        public void SetID(int id) { this.BinLocationID = id; }

        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }

        public override int PreparedPersonID { get { return 1; } }
    }


    public class BinLocationDTO
    {
    }
}
