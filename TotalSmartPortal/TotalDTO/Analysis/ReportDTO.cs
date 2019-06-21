using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;


namespace TotalDTO.Analysis
{
    public class ReportPrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Report; } }

        public int GetID() { return this.ReportID; }
        public void SetID(int id) { this.ReportID = id; }

        public int ReportID { get; set; }
        [Display(Name = "Báo cáo")]
        public string ReportName { get; set; }


        [Display(Name = "Ngày báo cáo")]
        public DateTime ReportFromDate { get; set; }
        [Display(Name = "Đến")]
        public DateTime ReportToDate { get; set; }

    }

    public class ReportDTO : ReportPrimitiveDTO
    {
    }
}
