using System;
using System.ComponentModel.DataAnnotations;

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

    public class BinLocationDTO
    {
    }
}
