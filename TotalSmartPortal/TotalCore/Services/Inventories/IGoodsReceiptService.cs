using TotalDTO;
using TotalModel;
using TotalModel.Models;
using TotalDTO.Inventories;

namespace TotalCore.Services.Inventories
{
    public interface IGoodsReceiptService<TDto, TPrimitiveDto, TDtoDetail> : IGoodsReceiptBaseService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    { }

    public interface IGoodsReceiptBaseService<TDto, TPrimitiveDto, TDtoDetail> : IGenericWithViewDetailService<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        bool Save(TDto dto, bool useExistingTransaction);
        bool Delete(int id, bool useExistingTransaction);
    }





    public interface IMaterialReceiptBaseService : IGoodsReceiptBaseService<GoodsReceiptDTO<GROptionMaterial>, GoodsReceiptPrimitiveDTO<GROptionMaterial>, GoodsReceiptDetailDTO>
    { }
    public interface IItemReceiptBaseService : IGoodsReceiptBaseService<GoodsReceiptDTO<GROptionItem>, GoodsReceiptPrimitiveDTO<GROptionItem>, GoodsReceiptDetailDTO>
    { }
    public interface IProductReceiptBaseService : IGoodsReceiptBaseService<GoodsReceiptDTO<GROptionProduct>, GoodsReceiptPrimitiveDTO<GROptionProduct>, GoodsReceiptDetailDTO>
    { }



    public interface IMaterialReceiptService : IGoodsReceiptService<GoodsReceiptDTO<GROptionMaterial>, GoodsReceiptPrimitiveDTO<GROptionMaterial>, GoodsReceiptDetailDTO>
    { }
    public interface IItemReceiptService : IGoodsReceiptService<GoodsReceiptDTO<GROptionItem>, GoodsReceiptPrimitiveDTO<GROptionItem>, GoodsReceiptDetailDTO>
    { }
    public interface IProductReceiptService : IGoodsReceiptService<GoodsReceiptDTO<GROptionProduct>, GoodsReceiptPrimitiveDTO<GROptionProduct>, GoodsReceiptDetailDTO>
    { }   

}