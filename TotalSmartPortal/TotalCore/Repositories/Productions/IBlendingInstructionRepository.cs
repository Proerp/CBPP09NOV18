using System.Collections.Generic;
using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IBlendingInstructionRepository : IGenericWithDetailRepository<BlendingInstruction, BlendingInstructionDetail>
    {
    }

    public interface IBlendingInstructionAPIRepository : IGenericAPIRepository
    {
        IEnumerable<BlendingInstructionRunning> GetRunnings(int? locationID);
        IEnumerable<BlendingInstructionLog> GetBlendingInstructionLogs(int? blendingInstructionID);
    }
}