using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class FinishedProductService : GenericWithViewDetailService<FinishedProduct, FinishedProductDetail, FinishedProductViewDetail, FinishedProductDTO, FinishedProductPrimitiveDTO, FinishedProductDetailDTO>, IFinishedProductService
    {
        public FinishedProductService(IFinishedProductRepository finishedProductRepository)
            : base(finishedProductRepository, "FinishedProductPostSaveValidate", "FinishedProductSaveRelative", "FinishedProductToggleApproved", null, null, "GetFinishedProductViewDetails")
        {
        }

        public override ICollection<FinishedProductViewDetail> GetViewDetails(int finishedProductID)
        {
            throw new System.ArgumentException("Invalid call GetViewDetails(id). Use GetFinishedProductViewDetails instead.", "FinishProduct Service");
        }

        public ICollection<FinishedProductViewDetail> GetFinishedProductViewDetails(int finishedProductID, int locationID, int firmOrderID, bool isReadonly)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("FinishedProductID", finishedProductID), new ObjectParameter("LocationID", locationID), new ObjectParameter("FirmOrderID", firmOrderID), new ObjectParameter("IsReadonly", isReadonly) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(FinishedProductDTO finishedProductDTO)
        {
            finishedProductDTO.FinishedProductViewDetails.RemoveAll(x => (x.Quantity == 0 && x.QuantityFailure == 0 && x.QuantityExcess == 0 && x.QuantityShortage == 0 && x.Swarfs == 0));
            return base.Save(finishedProductDTO);
        }
    }
}
