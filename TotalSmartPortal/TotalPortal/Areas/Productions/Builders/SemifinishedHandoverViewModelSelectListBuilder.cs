using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{    
    public interface ISemifinishedHandoverViewModelSelectListBuilder : IViewModelSelectListBuilder<SemifinishedHandoverViewModel>
    {
    }

    public class SemifinishedHandoverViewModelSelectListBuilder : A01ViewModelSelectListBuilder<SemifinishedHandoverViewModel>, ISemifinishedHandoverViewModelSelectListBuilder
    {
        private readonly IShiftSelectListBuilder shiftSelectListBuilder;
        private readonly IShiftRepository shiftRepository;

        public SemifinishedHandoverViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.shiftSelectListBuilder = shiftSelectListBuilder;
            this.shiftRepository = shiftRepository;
        }      
    }
}