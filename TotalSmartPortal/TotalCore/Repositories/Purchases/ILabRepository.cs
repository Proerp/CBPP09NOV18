using TotalModel.Models;

namespace TotalCore.Repositories.Purchases
{
    public interface ILabRepository : IGenericRepository<Lab>
    {
        bool ToggleHold(int entityID, bool hold);
    }

    public interface ILabAPIRepository : IGenericAPIRepository
    {
    }
}