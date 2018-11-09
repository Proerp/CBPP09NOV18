using System;
using System.ComponentModel.DataAnnotations;

namespace TotalDTO.Commons
{
    public interface IProductionLineBaseDTO
    {
        int ProductionLineID { get; set; }
        [Display(Name = "Mã số máy")]
        [UIHint("AutoCompletes/ProductionLineBase")]
        [Required(ErrorMessage = "Vui lòng nhập mã số máy")]
        string Code { get; set; }
        string Name { get; set; }
    }

    public class ProductionLineBaseDTO : BaseDTO, IProductionLineBaseDTO
    {
        public int ProductionLineID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class ProductionLineDTO
    {
    }
}
