using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IWarehouseAdjustmentRepository : IGenericWithDetailRepository<WarehouseAdjustment, WarehouseAdjustmentDetail>
    {
    }

    public interface IWarehouseAdjustmentAPIRepository : IGenericAPIRepository
    {
    }
}
