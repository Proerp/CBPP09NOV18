using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Commons
{
    public interface IBarcodeBaseDTO
    {
        int BarcodeID { get; set; }
        [Display(Name = "Mã vạch")]
        [Required(ErrorMessage = "Vui lòng nhập mã vạch")]
        string Code { get; set; }
    }

    public class BarcodeBaseDTO : BaseDTO, IBarcodeBaseDTO
    {
        public int BarcodeID { get; set; }

        [Display(Name = "Mã vạch")]
        [Required(ErrorMessage = "Vui lòng nhập mã vạch")]
        public string Code { get; set; }
    }

    public class SearchBarcodeDTO
    {
        [Display(Name = "Mã vạch")]
        [UIHint("AutoCompletes/BarcodeBase")]
        public BarcodeBaseDTO BarcodeBase { get; set; }
    }
}
