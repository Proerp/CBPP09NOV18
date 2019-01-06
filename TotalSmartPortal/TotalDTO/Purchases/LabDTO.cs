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
    }

    public class LabDTO : LabPrimitiveDTO
    {
    }
}