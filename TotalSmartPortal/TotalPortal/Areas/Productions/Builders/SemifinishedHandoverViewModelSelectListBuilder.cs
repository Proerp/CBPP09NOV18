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
        
        public SemifinishedHandoverViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }      
    }
}