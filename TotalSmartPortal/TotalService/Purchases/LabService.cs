using System;

using TotalModel.Models;
using TotalDTO.Purchases;
using TotalCore.Repositories.Purchases;
using TotalCore.Services.Purchases;

namespace TotalService.Purchases
{
    public class LabService : GenericService<Lab, LabDTO, LabPrimitiveDTO>, ILabService
    {
        private ILabRepository labRepository;
        public LabService(ILabRepository labRepository)
            : base(labRepository, null, null, "LabToggleApproved", "LabToggleVoid")
        {
            this.labRepository = labRepository;
        }

        public virtual bool Holdable(LabDTO dto)
        {
            if (this.GlobalLocked(dto)) return false;
            if (dto.Hold || !this.GetApprovalPermitted(dto.OrganizationalUnitID)) return false;

            return true; // this.labRepository.GetEditable(dto.GetID());
        }

        public virtual bool Releasable(LabDTO dto)
        {
            if (this.GlobalLocked(dto)) return false;
            if (!dto.Hold || !this.GetUnApprovalPermitted(dto.OrganizationalUnitID)) return false;

            return true; // this.labRepository.GetEditable(dto.GetID());
        }

        public virtual bool ToggleHold(LabDTO dto)
        {
            using (var dbContextTransaction = this.labRepository.BeginTransaction())
            {
                try
                {
                    if ((!dto.Hold && !this.Holdable(dto)) || (dto.Hold && !this.Releasable(dto))) throw new System.ArgumentException("Lỗi " + (dto.Hold ? "release" : "hold"), "Bạn không có quyền hoặc dữ liệu này đã bị khóa.");

                    if (!this.labRepository.ToggleHold(dto.LabID, !dto.Hold)) throw new System.ArgumentException("Lỗi", "Chứng từ không tồn tại hoặc đã " + (dto.Hold ? "relase" : "hold"));

                    this.labRepository.SaveChanges();

                    dbContextTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
