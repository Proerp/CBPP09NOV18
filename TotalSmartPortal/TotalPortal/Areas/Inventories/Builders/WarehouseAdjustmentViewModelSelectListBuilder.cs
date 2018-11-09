using System.Web.Mvc;
using System.Collections.Generic;

using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IWarehouseAdjustmentViewModelSelectListBuilder<TWarehouseAdjustmentViewModel> : IViewModelSelectListBuilder<TWarehouseAdjustmentViewModel>
        where TWarehouseAdjustmentViewModel : IWarehouseAdjustmentViewModel
    {
    }

    public class WarehouseAdjustmentViewModelSelectListBuilder<TWarehouseAdjustmentViewModel> : A01ViewModelSelectListBuilder<TWarehouseAdjustmentViewModel>, IWarehouseAdjustmentViewModelSelectListBuilder<TWarehouseAdjustmentViewModel>
        where TWarehouseAdjustmentViewModel : IWarehouseAdjustmentViewModel
    {
        private readonly IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder;
        private readonly IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository;

        public WarehouseAdjustmentViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder, IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.warehouseAdjustmentTypeSelectListBuilder = warehouseAdjustmentTypeSelectListBuilder;
            this.warehouseAdjustmentTypeRepository = warehouseAdjustmentTypeRepository;
        }

        public override void BuildSelectLists(TWarehouseAdjustmentViewModel warehouseAdjustmentViewModel)
        {
            base.BuildSelectLists(warehouseAdjustmentViewModel);
            warehouseAdjustmentViewModel.WarehouseAdjustmentTypeSelectList = this.warehouseAdjustmentTypeSelectListBuilder.BuildSelectListItemsForWarehouseAdjustmentTypes(this.warehouseAdjustmentTypeRepository.GetAllWarehouseAdjustmentTypes());
        }
    }








    public interface IOtherMaterialReceiptViewModelSelectListBuilder : IWarehouseAdjustmentViewModelSelectListBuilder<OtherMaterialReceiptViewModel>
    {
    }
    public class OtherMaterialReceiptViewModelSelectListBuilder : WarehouseAdjustmentViewModelSelectListBuilder<OtherMaterialReceiptViewModel>, IOtherMaterialReceiptViewModelSelectListBuilder
    {
        public OtherMaterialReceiptViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder, IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, warehouseAdjustmentTypeSelectListBuilder, warehouseAdjustmentTypeRepository)
        { }
    }


    public interface IOtherMaterialIssueViewModelSelectListBuilder : IWarehouseAdjustmentViewModelSelectListBuilder<OtherMaterialIssueViewModel>
    {
    }
    public class OtherMaterialIssueViewModelSelectListBuilder : WarehouseAdjustmentViewModelSelectListBuilder<OtherMaterialIssueViewModel>, IOtherMaterialIssueViewModelSelectListBuilder
    {
        public OtherMaterialIssueViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder, IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, warehouseAdjustmentTypeSelectListBuilder, warehouseAdjustmentTypeRepository)
        {}
    }


    public interface IMaterialAdjustmentViewModelSelectListBuilder : IWarehouseAdjustmentViewModelSelectListBuilder<MaterialAdjustmentViewModel>
    {
    }
    public class MaterialAdjustmentViewModelSelectListBuilder : WarehouseAdjustmentViewModelSelectListBuilder<MaterialAdjustmentViewModel>, IMaterialAdjustmentViewModelSelectListBuilder
    {
        public MaterialAdjustmentViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder, IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, warehouseAdjustmentTypeSelectListBuilder, warehouseAdjustmentTypeRepository)
        { }
    }





    public interface IOtherItemReceiptViewModelSelectListBuilder : IWarehouseAdjustmentViewModelSelectListBuilder<OtherItemReceiptViewModel>
    {
    }
    public class OtherItemReceiptViewModelSelectListBuilder : WarehouseAdjustmentViewModelSelectListBuilder<OtherItemReceiptViewModel>, IOtherItemReceiptViewModelSelectListBuilder
    {
        public OtherItemReceiptViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder, IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, warehouseAdjustmentTypeSelectListBuilder, warehouseAdjustmentTypeRepository)
        { }
    }


    public interface IOtherItemIssueViewModelSelectListBuilder : IWarehouseAdjustmentViewModelSelectListBuilder<OtherItemIssueViewModel>
    {
    }
    public class OtherItemIssueViewModelSelectListBuilder : WarehouseAdjustmentViewModelSelectListBuilder<OtherItemIssueViewModel>, IOtherItemIssueViewModelSelectListBuilder
    {
        public OtherItemIssueViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder, IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, warehouseAdjustmentTypeSelectListBuilder, warehouseAdjustmentTypeRepository)
        { }
    }


    public interface IItemAdjustmentViewModelSelectListBuilder : IWarehouseAdjustmentViewModelSelectListBuilder<ItemAdjustmentViewModel>
    {
    }
    public class ItemAdjustmentViewModelSelectListBuilder : WarehouseAdjustmentViewModelSelectListBuilder<ItemAdjustmentViewModel>, IItemAdjustmentViewModelSelectListBuilder
    {
        public ItemAdjustmentViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder, IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, warehouseAdjustmentTypeSelectListBuilder, warehouseAdjustmentTypeRepository)
        { }
    }





    public interface IOtherProductReceiptViewModelSelectListBuilder : IWarehouseAdjustmentViewModelSelectListBuilder<OtherProductReceiptViewModel>
    {
    }
    public class OtherProductReceiptViewModelSelectListBuilder : WarehouseAdjustmentViewModelSelectListBuilder<OtherProductReceiptViewModel>, IOtherProductReceiptViewModelSelectListBuilder
    {
        public OtherProductReceiptViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder, IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, warehouseAdjustmentTypeSelectListBuilder, warehouseAdjustmentTypeRepository)
        { }
    }


    public interface IOtherProductIssueViewModelSelectListBuilder : IWarehouseAdjustmentViewModelSelectListBuilder<OtherProductIssueViewModel>
    {
    }
    public class OtherProductIssueViewModelSelectListBuilder : WarehouseAdjustmentViewModelSelectListBuilder<OtherProductIssueViewModel>, IOtherProductIssueViewModelSelectListBuilder
    {
        public OtherProductIssueViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder, IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, warehouseAdjustmentTypeSelectListBuilder, warehouseAdjustmentTypeRepository)
        { }
    }


    public interface IProductAdjustmentViewModelSelectListBuilder : IWarehouseAdjustmentViewModelSelectListBuilder<ProductAdjustmentViewModel>
    {
    }
    public class ProductAdjustmentViewModelSelectListBuilder : WarehouseAdjustmentViewModelSelectListBuilder<ProductAdjustmentViewModel>, IProductAdjustmentViewModelSelectListBuilder
    {
        public ProductAdjustmentViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IWarehouseAdjustmentTypeSelectListBuilder warehouseAdjustmentTypeSelectListBuilder, IWarehouseAdjustmentTypeRepository warehouseAdjustmentTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, warehouseAdjustmentTypeSelectListBuilder, warehouseAdjustmentTypeRepository)
        { }
    }

}