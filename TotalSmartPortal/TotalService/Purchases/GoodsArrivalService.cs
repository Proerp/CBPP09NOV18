using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Purchases;
using TotalCore.Repositories.Purchases;
using TotalCore.Services.Purchases;


namespace TotalService.Purchases
{
    public class GoodsArrivalService : GenericWithViewDetailService<GoodsArrival, GoodsArrivalDetail, GoodsArrivalViewDetail, GoodsArrivalDTO, GoodsArrivalPrimitiveDTO, GoodsArrivalDetailDTO>, IGoodsArrivalService
    {
        public GoodsArrivalService(IGoodsArrivalRepository goodsArrivalRepository)
            : base(goodsArrivalRepository, "GoodsArrivalPostSaveValidate", "GoodsArrivalSaveRelative", "GoodsArrivalToggleApproved", null, null, "GetGoodsArrivalViewDetails")
        {
        }

        public override ICollection<GoodsArrivalViewDetail> GetViewDetails(int goodsArrivalID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("GoodsArrivalID", goodsArrivalID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(GoodsArrivalDTO goodsArrivalDTO)
        {
            goodsArrivalDTO.GoodsArrivalViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(goodsArrivalDTO);
        }
    }
}
