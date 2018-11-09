using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalModel
{
    public interface IBaseModel : IValidatableObject
    {
        DateTime? EntryDate { get; set; }
        int LocationID { get; set; }


        bool Approved { get; set; }
        Nullable<System.DateTime> ApprovedDate { get; set; }

        bool InActive { get; set; }
        Nullable<System.DateTime> InActiveDate { get; set; }

        bool InActivePartial { get; set; }
        Nullable<System.DateTime> InActivePartialDate { get; set; }

        Nullable<int> VoidTypeID { get; set; }

        [Display(Name = "Mô tả")]
        string Caption { get; set; }
        [Display(Name = "Ghi chú")]
        string Remarks { get; set; }
    }

    public abstract class BaseModel : IBaseModel
    {
        protected BaseModel() { this.EntryDate = DateTime.Now; }


        [UIHint("DateTimeReadonly")]
        [Display(Name = "Ngày lập")]
        [Required(ErrorMessage = "Vui lòng nhập ngày lập")]
        public DateTime? EntryDate { get; set; }

        public int LocationID { get; set; }

        [Display(Name = "Mô tả")]
        public virtual string Caption { get; set; }
        [Display(Name = "Ghi chú")]
        public virtual string Remarks { get; set; }

        public virtual bool Approved { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        
        public virtual bool InActive { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        
        public bool InActivePartial { get; set; }
        public Nullable<System.DateTime> InActivePartialDate { get; set; }

        public virtual Nullable<int> VoidTypeID { get; set; }

        protected virtual void ShiftSaving(int shiftID)
        {
            TimeSpan midnight = new TimeSpan(0, 0, 0);
            TimeSpan sevenOclock = new TimeSpan(7, 0, 0);
            TimeSpan atNoon = new TimeSpan(12, 0, 0);

            TimeSpan entryTime = ((DateTime)this.EntryDate).TimeOfDay;

            if (shiftID == 1 && entryTime >= midnight && entryTime < sevenOclock)
                this.EntryDate = ((DateTime)this.EntryDate).AddMinutes(-1) - entryTime;
            if (shiftID == 2 && entryTime >= midnight && entryTime < atNoon)
                this.EntryDate = ((DateTime)this.EntryDate).AddMinutes(-1) - entryTime;
        }

        #region Implementation of IValidatableObject

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (false) yield return new ValidationResult("", new[] { "" });
        }

        #endregion
    }
}
