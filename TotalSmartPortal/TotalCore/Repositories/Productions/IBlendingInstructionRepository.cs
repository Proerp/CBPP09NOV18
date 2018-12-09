using System.Collections.Generic;
using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IBlendingInstructionRepository : IGenericWithDetailRepository<BlendingInstruction, BlendingInstructionDetail>
    {
    }

    public interface IBlendingInstructionAPIRepository : IGenericAPIRepository
    {
        IEnumerable<BlendingInstructionLog> GetBlendingInstructionLogs(int? blendingInstructionID);
    }
}