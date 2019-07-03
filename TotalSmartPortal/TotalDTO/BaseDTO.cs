using System;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO
{
    public interface IBaseDTO : IBaseModel, IAccessControlAttribute
    {
        string Reference { get; set; }

        int ApproverID { get; set; }
        [Display(Name = "Diễn giải")]
        string Description { get; set; }

        bool GlobalLocked { get; set; }
        bool Editable { get; set; }
        bool Approvable { get; set; }
        bool UnApprovable { get; set; }
        bool Deletable { get; set; }
        bool Voidable { get; set; }
        bool UnVoidable { get; set; }

        bool ShowDiscount { get; set; }

        bool ShowListedPrice { get; set; }
        bool ShowListedGrossPrice { get; set; }


        bool EditAfterApprove { get; }
        bool EditAfterUnApprove { get; }

        bool EditAfterVoid { get; }
        bool EditAfterUnVoid { get; }

        bool PrintAfterClosedSubmit { get; set; }
        GlobalEnums.SubmitTypeOption SubmitTypeOption { get; set; }

        string ApproveWarning { get; }
        string UnApproveWarning { get; }

        string VoidWarning { get; }
        string UnVoidWarning { get; }

        void PerformPresaveRule();

        void PrepareVoidDetail(int? detailID);

        DateTime GlobalFromDate { get; set; }
        DateTime GlobalToDate { get; set; }

        int PrintOptionID { get; set; }
    }

    public abstract class BaseDTO : BaseModel, IAccessControlAttribute 
    {
        protected BaseDTO()
        {
            // apply any DefaultValueAttribute settings to their properties
            var propertyInfos = this.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                if (attributes.Any())
                {
                    var attribute = (DefaultValueAttribute)attributes[0];
                    propertyInfo.SetValue(this, attribute.Value, null);
                }
            }
        }

        
        [Display(Name = "Số phiếu")]
        public virtual string Reference { get; set; }



        public int UserID { get; set; }
        [Required]
        [Display(Name = "Người lập")]
        public virtual int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }


        [Display(Name = "Người duyệt")]
        public int ApproverID { get; set; }


        [Display(Name = "Diễn giải")]
        public string Description { get; set; }
        

        public bool GlobalLocked { get; set; }
        public bool Editable { get; set; }
        public bool Approvable { get; set; }
        public bool UnApprovable { get; set; }
        public bool Deletable { get; set; }
        public bool Voidable { get; set; }
        public bool UnVoidable { get; set; }

        public bool ShowDiscount { get; set; }

        public bool ShowListedPrice { get; set; }
        public bool ShowListedGrossPrice { get; set; }


        public virtual bool EditAfterApprove { get { return false; } }
        public virtual bool EditAfterUnApprove { get { return true; } }

        public virtual bool EditAfterVoid { get { return false; } }
        public virtual bool EditAfterUnVoid { get { return false; } }
        //These properties are used as an implementation preservation of ISimpleViewModel for these ________ViewModel class (Those class ________ViewModel which is BOTH inheritance from this BaseDTO AND implementation of ISimpleViewModel)
        public virtual bool PrintAfterClosedSubmit { get; set; }
        public GlobalEnums.SubmitTypeOption SubmitTypeOption { get; set; }

        public virtual string ApproveWarning { get { return "Duyệt"; } }
        public virtual string UnApproveWarning { get { return "Hủy duyệt"; } }

        public virtual string VoidWarning { get { return "Vô hiệu"; } }
        public virtual string UnVoidWarning { get { return "Phục hồi"; } }
        
        public virtual void PerformPresaveRule() { }

        public virtual void PrepareVoidDetail(int? detailID) { }
        public virtual void PrepareRemarkDetail(int? detailID) { }














        
        [Display(Name = "Lọc dữ liệu từ ngày")]
        public DateTime GlobalFromDate { get; set; }
        [Display(Name = "Lọc dữ liệu đến ngày")]
        public DateTime GlobalToDate { get; set; }

        public virtual int PrintOptionID { get; set; }
    }
}

