using System.ComponentModel.DataAnnotations;
using TotalDTO.Commons;

namespace TotalDTO.Helpers.Interfaces
{    
    public interface IWarehouse
    {
        [Display(Name = "Kho hàng")]        
        WarehouseBaseDTO Warehouse { get; set; }
    }
}
