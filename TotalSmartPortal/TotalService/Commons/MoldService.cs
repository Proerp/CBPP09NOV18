using TotalModel.Models;
using TotalDTO.Commons;
using TotalCore.Repositories.Commons;
using TotalCore.Services.Commons;

namespace TotalService.Commons
{
    public class MoldService : GenericService<Mold, MoldDTO, MoldPrimitiveDTO>, IMoldService
    {
        public MoldService(IMoldRepository employeeRepository)
            : base(employeeRepository)
        {
        }

    }
}
