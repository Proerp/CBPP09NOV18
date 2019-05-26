using TotalDTO.Purchases;
using TotalModel.Models;

namespace TotalCore.Services.Purchases
{
    public interface ILabService : IGenericService<Lab, LabDTO, LabPrimitiveDTO>
    {
        bool Holdable(LabDTO dto);
        bool Releasable(LabDTO dto);
        bool ToggleHold(LabDTO dto);
    }
}
