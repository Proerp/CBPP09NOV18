using System.Web.Mvc;
using System.Collections.Generic;

using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Commons.ViewModels;
using TotalPortal.ViewModels.Helpers;
using TotalBase.Enums;


namespace TotalPortal.Areas.Commons.Builders
{
    public interface ICommoditySelectListBuilder<TCommodityBuilderModel> : IViewModelSelectListBuilder<TCommodityBuilderModel>
        where TCommodityBuilderModel : ICommodityViewModel
    {
    }

    public class CommoditySelectListBuilder<TCommodityBuilderModel> : ICommoditySelectListBuilder<TCommodityBuilderModel>
        where TCommodityBuilderModel : ICommodityViewModel
    {
        private readonly ICommodityBrandSelectListBuilder commodityBrandSelectListBuilder;
        private readonly ICommodityBrandRepository commodityBrandRepository;

        private readonly ICommodityCategorySelectListBuilder commodityCategorySelectListBuilder;
        private readonly ICommodityCategoryRepository commodityCategoryRepository;

        private readonly ICommodityTypeSelectListBuilder commodityTypeSelectListBuilder;
        private readonly ICommodityTypeRepository commodityTypeRepository;


        private readonly ICommodityClassSelectListBuilder commodityClassSelectListBuilder;
        private readonly ICommodityClassRepository commodityClassRepository;

        private readonly ICommodityLineSelectListBuilder commodityLineSelectListBuilder;
        private readonly ICommodityLineRepository commodityLineRepository;

        public CommoditySelectListBuilder(ICommodityBrandSelectListBuilder commodityBrandSelectListBuilder, ICommodityBrandRepository commodityBrandRepository, ICommodityCategorySelectListBuilder commodityCategorySelectListBuilder, ICommodityCategoryRepository commodityCategoryRepository, ICommodityTypeSelectListBuilder commodityTypeSelectListBuilder, ICommodityTypeRepository commodityTypeRepository, ICommodityClassSelectListBuilder commodityClassSelectListBuilder, ICommodityClassRepository commodityClassRepository, ICommodityLineSelectListBuilder commodityLineSelectListBuilder, ICommodityLineRepository commodityLineRepository)
        {
            this.commodityBrandSelectListBuilder = commodityBrandSelectListBuilder;
            this.commodityBrandRepository = commodityBrandRepository;

            this.commodityCategorySelectListBuilder = commodityCategorySelectListBuilder;
            this.commodityCategoryRepository = commodityCategoryRepository;

            this.commodityTypeSelectListBuilder = commodityTypeSelectListBuilder;
            this.commodityTypeRepository = commodityTypeRepository;

            this.commodityClassSelectListBuilder = commodityClassSelectListBuilder;
            this.commodityClassRepository = commodityClassRepository;

            this.commodityLineSelectListBuilder = commodityLineSelectListBuilder;
            this.commodityLineRepository = commodityLineRepository;
        }

        public virtual void BuildSelectLists(TCommodityBuilderModel commodityViewModel)
        {
            commodityViewModel.CommodityBrandSelectList = this.commodityBrandSelectListBuilder.BuildSelectListItemsForCommodityBrands(this.commodityBrandRepository.GetAllCommodityBrands());
            commodityViewModel.CommodityCategorySelectList = this.commodityCategorySelectListBuilder.BuildSelectListItemsForCommodityCategorys(this.commodityCategoryRepository.GetAllCommodityCategories(commodityViewModel.IsItem || commodityViewModel.IsProduct ? ((int)GlobalEnums.CommodityTypeID.Products).ToString() : ((int)GlobalEnums.CommodityTypeID.Materials).ToString()));
            commodityViewModel.CommodityClassSelectList = this.commodityClassSelectListBuilder.BuildSelectListItemsForCommodityClasss(this.commodityClassRepository.GetAllCommodityClasses());
            commodityViewModel.CommodityLineSelectList = this.commodityLineSelectListBuilder.BuildSelectListItemsForCommodityLines(this.commodityLineRepository.GetAllCommodityLines());
        }
    }


    public interface IMaterialSelectListBuilder : ICommoditySelectListBuilder<MaterialViewModel> { }
    public class MaterialSelectListBuilder : CommoditySelectListBuilder<MaterialViewModel>, IMaterialSelectListBuilder
    {
        public MaterialSelectListBuilder(ICommodityBrandSelectListBuilder commodityBrandSelectListBuilder, ICommodityBrandRepository commodityBrandRepository, ICommodityCategorySelectListBuilder commodityCategorySelectListBuilder, ICommodityCategoryRepository commodityCategoryRepository, ICommodityTypeSelectListBuilder commodityTypeSelectListBuilder, ICommodityTypeRepository commodityTypeRepository, ICommodityClassSelectListBuilder commodityClassSelectListBuilder, ICommodityClassRepository commodityClassRepository, ICommodityLineSelectListBuilder commodityLineSelectListBuilder, ICommodityLineRepository commodityLineRepository)
            : base(commodityBrandSelectListBuilder, commodityBrandRepository, commodityCategorySelectListBuilder, commodityCategoryRepository, commodityTypeSelectListBuilder, commodityTypeRepository, commodityClassSelectListBuilder, commodityClassRepository, commodityLineSelectListBuilder, commodityLineRepository)
        { }
    }

    public interface IItemSelectListBuilder : ICommoditySelectListBuilder<ItemViewModel> { }
    public class ItemSelectListBuilder : CommoditySelectListBuilder<ItemViewModel>, IItemSelectListBuilder
    {
        public ItemSelectListBuilder(ICommodityBrandSelectListBuilder commodityBrandSelectListBuilder, ICommodityBrandRepository commodityBrandRepository, ICommodityCategorySelectListBuilder commodityCategorySelectListBuilder, ICommodityCategoryRepository commodityCategoryRepository, ICommodityTypeSelectListBuilder commodityTypeSelectListBuilder, ICommodityTypeRepository commodityTypeRepository, ICommodityClassSelectListBuilder commodityClassSelectListBuilder, ICommodityClassRepository commodityClassRepository, ICommodityLineSelectListBuilder commodityLineSelectListBuilder, ICommodityLineRepository commodityLineRepository)
            : base(commodityBrandSelectListBuilder, commodityBrandRepository, commodityCategorySelectListBuilder, commodityCategoryRepository, commodityTypeSelectListBuilder, commodityTypeRepository, commodityClassSelectListBuilder, commodityClassRepository, commodityLineSelectListBuilder, commodityLineRepository)
        { }
    }

    public interface IProductSelectListBuilder : ICommoditySelectListBuilder<ProductViewModel> { }
    public class ProductSelectListBuilder : CommoditySelectListBuilder<ProductViewModel>, IProductSelectListBuilder
    {
        public ProductSelectListBuilder(ICommodityBrandSelectListBuilder commodityBrandSelectListBuilder, ICommodityBrandRepository commodityBrandRepository, ICommodityCategorySelectListBuilder commodityCategorySelectListBuilder, ICommodityCategoryRepository commodityCategoryRepository, ICommodityTypeSelectListBuilder commodityTypeSelectListBuilder, ICommodityTypeRepository commodityTypeRepository, ICommodityClassSelectListBuilder commodityClassSelectListBuilder, ICommodityClassRepository commodityClassRepository, ICommodityLineSelectListBuilder commodityLineSelectListBuilder, ICommodityLineRepository commodityLineRepository)
            : base(commodityBrandSelectListBuilder, commodityBrandRepository, commodityCategorySelectListBuilder, commodityCategoryRepository, commodityTypeSelectListBuilder, commodityTypeRepository, commodityClassSelectListBuilder, commodityClassRepository, commodityLineSelectListBuilder, commodityLineRepository)
        { }
    }
}