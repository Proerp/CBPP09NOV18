using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;


namespace TotalPortal.Areas.Commons.Builders
{ 
    public interface ITransferOrderTypeSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForTransferOrderTypes(IEnumerable<TransferOrderType> TransferOrderTypes);
    }

    public class TransferOrderTypeSelectListBuilder : ITransferOrderTypeSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForTransferOrderTypes(IEnumerable<TransferOrderType> TransferOrderTypes)
        {
            return TransferOrderTypes.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.TransferOrderTypeID.ToString() }).ToList();
        }
    }
}