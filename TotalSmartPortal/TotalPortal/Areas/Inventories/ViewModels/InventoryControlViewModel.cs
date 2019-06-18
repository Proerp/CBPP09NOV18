namespace TotalPortal.Areas.Inventories.ViewModels
{
    public class InventoryControlViewModel
    {
        public int? CommodityID { get; set; }
        public string CommodityCode { get; set; }

        public int LocationID { get; set; }
        public string LocationCode { get; set; }
    }
}