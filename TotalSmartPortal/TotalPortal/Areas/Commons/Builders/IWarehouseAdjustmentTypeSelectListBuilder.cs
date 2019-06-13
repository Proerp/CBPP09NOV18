using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;


namespace TotalPortal.Areas.Commons.Builders
{

    public interface IWarehouseAdjustmentTypeSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForWarehouseAdjustmentTypes(IEnumerable<WarehouseAdjustmentType> WarehouseAdjustmentTypes);
    }

    public class WarehouseAdjustmentTypeSelectListBuilder : IWarehouseAdjustmentTypeSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForWarehouseAdjustmentTypes(IEnumerable<WarehouseAdjustmentType> WarehouseAdjustmentTypes)
        {
            return WarehouseAdjustmentTypes.Where(w => w.Enabled).Select(pt => new SelectListItem { Text = pt.Name, Value = pt.WarehouseAdjustmentTypeID.ToString() }).ToList();
        }
    }
}