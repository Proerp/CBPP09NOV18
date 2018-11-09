using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;
using TotalDAL.Repositories.Inventories;
using TotalBase.Enums;

namespace TotalService.Inventories
{
    public class TransferOrderService<TDto, TPrimitiveDto, TDtoDetail> : GenericWithViewDetailService<TransferOrder, TransferOrderDetail, TransferOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail>, ITransferOrderService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, ITransferOrderDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public TransferOrderService(ITransferOrderRepository transferOrderRepository)
            : base(transferOrderRepository, null, null, "TransferOrderToggleApproved", "TransferOrderToggleVoid", "TransferOrderToggleVoidDetail", "GetTransferOrderViewDetails")
        {
        }

        public override ICollection<TransferOrderViewDetail> GetViewDetails(int transferOrderID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("TransferOrderID", transferOrderID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(TDto dto)
        {
            dto.TransferOrderViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(dto);
        }

    }




    public class MaterialTransferOrderService : TransferOrderService<TransferOrderDTO<TOOptionMaterial>, TransferOrderPrimitiveDTO<TOOptionMaterial>, TransferOrderDetailDTO>, IMaterialTransferOrderService
    {
        public MaterialTransferOrderService(ITransferOrderRepository transferOrderRepository)
            : base(transferOrderRepository) { }
    }
    public class ItemTransferOrderService : TransferOrderService<TransferOrderDTO<TOOptionItem>, TransferOrderPrimitiveDTO<TOOptionItem>, TransferOrderDetailDTO>, IItemTransferOrderService
    {
        public ItemTransferOrderService(ITransferOrderRepository transferOrderRepository)
            : base(transferOrderRepository) { }
    }
    public class ProductTransferOrderService : TransferOrderService<TransferOrderDTO<TOOptionProduct>, TransferOrderPrimitiveDTO<TOOptionProduct>, TransferOrderDetailDTO>, IProductTransferOrderService
    {
        public ProductTransferOrderService(ITransferOrderRepository transferOrderRepository)
            : base(transferOrderRepository) { }
    }    
}
