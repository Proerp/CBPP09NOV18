using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Inventories;

namespace TotalCore.Services.Inventories
{
    public interface IWarehouseAdjustmentService<TDto, TPrimitiveDto, TDtoDetail> : IGenericWithViewDetailService<WarehouseAdjustment, WarehouseAdjustmentDetail, WarehouseAdjustmentViewDetail, TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
    }

    public interface IOtherMaterialReceiptService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMtlRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlRct>, WarehouseAdjustmentDetailDTO>
    { }
    public interface IOtherMaterialIssueService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMtlIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlIss>, WarehouseAdjustmentDetailDTO>
    { }
    public interface IMaterialAdjustmentService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMtlAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlAdj>, WarehouseAdjustmentDetailDTO>
    { }

    public interface IOtherItemReceiptService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionItmRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionItmRct>, WarehouseAdjustmentDetailDTO>
    { }
    public interface IOtherItemIssueService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionItmIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionItmIss>, WarehouseAdjustmentDetailDTO>
    { }
    public interface IItemAdjustmentService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionItmAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionItmAdj>, WarehouseAdjustmentDetailDTO>
    { }

    public interface IOtherProductReceiptService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionPrdRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdRct>, WarehouseAdjustmentDetailDTO>
    { }
    public interface IOtherProductIssueService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionPrdIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdIss>, WarehouseAdjustmentDetailDTO>
    { }
    public interface IProductAdjustmentService : IWarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionPrdAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdAdj>, WarehouseAdjustmentDetailDTO>
    { }
}

