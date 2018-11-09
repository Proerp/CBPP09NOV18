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
    public class WarehouseTransferService<TDto, TPrimitiveDto, TDtoDetail> : GenericWithViewDetailService<WarehouseTransfer, WarehouseTransferDetail, WarehouseTransferViewDetail, TDto, TPrimitiveDto, TDtoDetail>, IWarehouseTransferService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, IWarehouseTransferDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public WarehouseTransferService(IWarehouseTransferRepository warehouseTransferRepository)
            : base(warehouseTransferRepository, "WarehouseTransferPostSaveValidate", "WarehouseTransferSaveRelative", "WarehouseTransferToggleApproved", "WarehouseTransferToggleVoid", "WarehouseTransferToggleVoidDetail", "GetWarehouseTransferViewDetails")
        {
        }

        public override ICollection<WarehouseTransferViewDetail> GetViewDetails(int warehouseTransferID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("WarehouseTransferID", warehouseTransferID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(TDto dto)
        {
            dto.WarehouseTransferViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(dto);
        }

    }




    public class MaterialTransferService : WarehouseTransferService<WarehouseTransferDTO<WTOptionMaterial>, WarehouseTransferPrimitiveDTO<WTOptionMaterial>, WarehouseTransferDetailDTO>, IMaterialTransferService
    {
        public MaterialTransferService(IWarehouseTransferRepository warehouseTransferRepository)
            : base(warehouseTransferRepository) { }
    }
    public class ItemTransferService : WarehouseTransferService<WarehouseTransferDTO<WTOptionItem>, WarehouseTransferPrimitiveDTO<WTOptionItem>, WarehouseTransferDetailDTO>, IItemTransferService
    {
        public ItemTransferService(IWarehouseTransferRepository warehouseTransferRepository)
            : base(warehouseTransferRepository) { }
    }
    public class ProductTransferService : WarehouseTransferService<WarehouseTransferDTO<WTOptionProduct>, WarehouseTransferPrimitiveDTO<WTOptionProduct>, WarehouseTransferDetailDTO>, IProductTransferService
    {
        public ProductTransferService(IWarehouseTransferRepository warehouseTransferRepository)
            : base(warehouseTransferRepository) { }
    }   
}
