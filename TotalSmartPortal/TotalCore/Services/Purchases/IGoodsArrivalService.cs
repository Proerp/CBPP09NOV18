using TotalModel.Models;
using TotalDTO.Purchases;
using TotalCore.Services.Helpers;

namespace TotalCore.Services.Purchases
{
    public interface IGoodsArrivalService : IGenericWithViewDetailService<GoodsArrival, GoodsArrivalDetail, GoodsArrivalViewDetail, GoodsArrivalDTO, GoodsArrivalPrimitiveDTO, GoodsArrivalDetailDTO>
    {
    }
}
