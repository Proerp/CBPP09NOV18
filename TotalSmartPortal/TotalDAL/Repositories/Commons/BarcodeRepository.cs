using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class BarcodeAPIRepository : GenericAPIRepository, IBarcodeAPIRepository
    {
        public BarcodeAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetBarcodeIndexes")
        {
        }

        public IList<BarcodeBasic> GetBarcodeBasics(string searchText)
        {
            return this.TotalSmartPortalEntities.GetBarcodeBasics(searchText).ToList();
        }

        public IList<BarcodeJournal> GetBarcodeJournals(string barcode)
        {
            return this.TotalSmartPortalEntities.GetBarcodeJournals(barcode).ToList();
        }        
    }
}
