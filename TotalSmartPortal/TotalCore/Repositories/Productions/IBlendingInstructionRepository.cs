using System.Collections.Generic;
using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IBlendingInstructionRepository : IGenericWithDetailRepository<BlendingInstruction, BlendingInstructionDetail>
    {
        void SetBlendingInstructionSymbologies(int? blendingInstructionID, string code, string symbologies);
    }

    public interface IBlendingInstructionAPIRepository : IGenericAPIRepository
    {
        IEnumerable<BlendingInstructionRunning> GetRunnings(int? locationID);
        IEnumerable<BlendingInstructionLog> GetBlendingInstructionLogs(int? blendingInstructionID);
    }
}