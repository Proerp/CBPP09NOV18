using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IBarcodeAPIRepository : IGenericAPIRepository
    {
        IList<BarcodeBasic> GetBarcodeBasics(string searchText);
        IList<BarcodeJournal> GetBarcodeJournals(string barcode);
    }
}

