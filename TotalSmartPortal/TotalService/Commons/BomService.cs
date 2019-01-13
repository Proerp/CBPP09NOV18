using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Commons;
using TotalCore.Repositories.Commons;
using TotalCore.Services.Commons;

namespace TotalService.Commons
{
    public class BomService : GenericWithViewDetailService<Bom, BomDetail, BomViewDetail, BomDTO, BomPrimitiveDTO, BomDetailDTO>, IBomService
    {
        public BomService(IBomRepository bomRepository)
            : base(bomRepository, "BomPostSaveValidate", "BomSaveRelative", null, null, null, "GetBomViewDetails")
        {
        }

        public override ICollection<BomViewDetail> GetViewDetails(int bomID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("BomID", bomID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(BomDTO bomDTO)
        {
            bomDTO.BomViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(bomDTO);
        }
    }
}
