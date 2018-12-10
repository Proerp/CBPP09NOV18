using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface IBlendingInstructionViewModelSelectListBuilder : IViewModelSelectListBuilder<BlendingInstructionViewModel>
    {
    }

    public class BlendingInstructionViewModelSelectListBuilder : A01ViewModelSelectListBuilder<BlendingInstructionViewModel>, IBlendingInstructionViewModelSelectListBuilder
    {
        public BlendingInstructionViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }

}