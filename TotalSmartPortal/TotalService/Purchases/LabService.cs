using System;

using TotalModel.Models;
using TotalDTO.Purchases;
using TotalCore.Repositories.Purchases;
using TotalCore.Services.Purchases;

namespace TotalService.Purchases
{
    public class LabService : GenericService<Lab, LabDTO, LabPrimitiveDTO>, ILabService
    {
        public LabService(ILabRepository labRepository)
            : base(labRepository)
        {
        }
    }
}
