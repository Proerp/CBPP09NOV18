using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Commons
{
    public interface IMoldBaseDTO
    {
        int MoldID { get; set; }
        [Display(Name = "Khuôn")]
        [UIHint("AutoCompletes/MoldBase")]
        [Required(ErrorMessage = "Vui lòng nhập khuôn")]
        string Code { get; set; }
        string Name { get; set; }
        decimal Quantity { get; set; }
    }

    public class MoldBaseDTO : BaseDTO, IMoldBaseDTO
    {
        public int MoldID { get; set; }
        [Display(Name = "Khuôn")]
        [Required(ErrorMessage = "Vui lòng nhập khuôn")]
        public string Code { get; set; }
        [Display(Name = "Tên khuôn")]
        [Required(ErrorMessage = "Vui lòng nhập tên khuôn")]
        public string Name { get; set; }
        [Display(Name = "Số sản phẩm/ khuôn")]
        [Range(1, 100, ErrorMessage = "Vui lòng nhập số SP/ khuôn")]
        public decimal Quantity { get; set; }
    }

    public class MoldPrimitiveDTO : MoldBaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Mold; } }

        public int GetID() { return this.MoldID; }
        public void SetID(int id) { this.MoldID = id; }

        public string Reference { get { return this.Code; } }
        public string OfficialCode { get { return this.Code; } }

        [Display(Name = "Chiều rộng")]
        [Range(1, 10000, ErrorMessage = "Vui lòng nhập chiều rộng")]
        public decimal MoldWidth { get; set; }
        [Display(Name = "Chiều dài")]
        [Range(1, 10000, ErrorMessage = "Vui lòng nhập chiều dài")]
        public decimal MoldLength { get; set; }
        [Display(Name = "Chiều rộng sản phẩm")]
        [Range(1, 10000, ErrorMessage = "Vui lòng nhập chiều rộng SP")]
        public decimal ItemWidth { get; set; }
        [Display(Name = "Chiều dài sản phẩm")]
        [Range(1, 10000, ErrorMessage = "Vui lòng nhập chiều dài SP")]
        public decimal ItemLength { get; set; }
        [Display(Name = "Chiều cao sản phẩm")]
        [Range(1, 10000, ErrorMessage = "Vui lòng nhập chiều cao SP")]
        public decimal ItemHigh { get; set; }

        public override int PreparedPersonID { get { return 1; } }
    }


    public class MoldDTO : MoldPrimitiveDTO
    {
    }

}
