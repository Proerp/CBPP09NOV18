using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;
using TotalDTO.Helpers;
using TotalDTO.Commons;

namespace TotalDTO.Purchases
{
    public class PurchaseRequisitionPrimitiveDTO : QuantityDTO<PurchaseRequisitionDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PurchaseRequisition; } }

        public int GetID() { return this.PurchaseRequisitionID; }
        public void SetID(int id) { this.PurchaseRequisitionID = id; }

        public int PurchaseRequisitionID { get; set; }
        
        public virtual Nullable<int> CustomerID { get; set; }        

        [Display(Name = "Số chứng từ")]
        public string Code { get; set; }

        [Display(Name = "Mục đích")]
        public string Purposes { get; set; }

        [Display(Name = "Ngày giao hàng")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = (int)this.CustomerID; });
        }
    }

    public class PurchaseRequisitionDTO : PurchaseRequisitionPrimitiveDTO, IBaseDetailEntity<PurchaseRequisitionDetailDTO>
    {
        public PurchaseRequisitionDTO()
        {
            this.PurchaseRequisitionViewDetails = new List<PurchaseRequisitionDetailDTO>();
        }

        public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> VoidTypeID { get { return (this.VoidType != null ? this.VoidType.VoidTypeID : null); } }
        [UIHint("AutoCompletes/VoidType")]
        public VoidTypeBaseDTO VoidType { get; set; }

        public List<PurchaseRequisitionDetailDTO> PurchaseRequisitionViewDetails { get; set; }
        public List<PurchaseRequisitionDetailDTO> ViewDetails { get { return this.PurchaseRequisitionViewDetails; } set { this.PurchaseRequisitionViewDetails = value; } }

        public ICollection<PurchaseRequisitionDetailDTO> GetDetails() { return this.PurchaseRequisitionViewDetails; }

        protected override IEnumerable<PurchaseRequisitionDetailDTO> DtoDetails() { return this.PurchaseRequisitionViewDetails; }

        public override void PrepareVoidDetail(int? detailID)
        {
            this.ViewDetails.RemoveAll(w => w.PurchaseRequisitionDetailID != detailID);
            if (this.ViewDetails.Count() > 0)
                this.VoidType = new VoidTypeBaseDTO() { VoidTypeID = this.ViewDetails[0].VoidTypeID, Code = this.ViewDetails[0].VoidTypeCode, Name = this.ViewDetails[0].VoidTypeName, VoidClassID = this.ViewDetails[0].VoidClassID };
            base.PrepareVoidDetail(detailID);
        }
    }

}
