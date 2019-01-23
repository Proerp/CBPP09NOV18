using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Commons.ViewModels;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface IBinLocationSelectListBuilder : IViewModelSelectListBuilder<BinLocationViewModel>
    {
    }

    public class BinLocationSelectListBuilder : IBinLocationSelectListBuilder
    {
        private readonly IBinTypeSelectListBuilder binTypeSelectListBuilder;
        private readonly IBinTypeRepository binTypeRepository;

        public BinLocationSelectListBuilder(IBinTypeSelectListBuilder binTypeSelectListBuilder, IBinTypeRepository binTypeRepository)
        {
            this.binTypeSelectListBuilder = binTypeSelectListBuilder;
            this.binTypeRepository = binTypeRepository;
        }

        public virtual void BuildSelectLists(BinLocationViewModel binLocationViewModel)
        {
            binLocationViewModel.BinTypeSelectList = this.binTypeSelectListBuilder.BuildSelectListItemsForBinTypes(this.binTypeRepository.GetAllBinTypes());
        }
    }

}