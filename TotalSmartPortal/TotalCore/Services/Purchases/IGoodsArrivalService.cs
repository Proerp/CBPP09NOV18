using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Purchases;

namespace TotalCore.Services.Purchases
{
    public interface IGoodsArrivalService<TDto, TPrimitiveDto, TDtoDetail> : IGenericWithViewDetailService<GoodsArrival, GoodsArrivalDetail, GoodsArrivalViewDetail, TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
    }

    public interface IMaterialArrivalService : IGoodsArrivalService<GoodsArrivalDTO<GoodsArrivalOptionMaterial>, GoodsArrivalPrimitiveDTO<GoodsArrivalOptionMaterial>, GoodsArrivalDetailDTO>
    { }
    public interface IItemArrivalService : IGoodsArrivalService<GoodsArrivalDTO<GoodsArrivalOptionItem>, GoodsArrivalPrimitiveDTO<GoodsArrivalOptionItem>, GoodsArrivalDetailDTO>
    { }
    public interface IProductArrivalService : IGoodsArrivalService<GoodsArrivalDTO<GoodsArrivalOptionProduct>, GoodsArrivalPrimitiveDTO<GoodsArrivalOptionProduct>, GoodsArrivalDetailDTO>
    { }
}
