using System.Web.Mvc;
using System.Collections.Generic;

using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{   
    public interface IWarehouseTransferViewModelSelectListBuilder<TWarehouseTransferViewModel> : IViewModelSelectListBuilder<TWarehouseTransferViewModel>
        where TWarehouseTransferViewModel : IWarehouseTransferViewModel
    {
    }

    public class WarehouseTransferViewModelSelectListBuilder<TWarehouseTransferViewModel> : A01ViewModelSelectListBuilder<TWarehouseTransferViewModel>, IWarehouseTransferViewModelSelectListBuilder<TWarehouseTransferViewModel>
        where TWarehouseTransferViewModel : IWarehouseTransferViewModel
    {
        private readonly IShiftSelectListBuilder shiftSelectListBuilder;
        private readonly IShiftRepository shiftRepository;

        public WarehouseTransferViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.shiftSelectListBuilder = shiftSelectListBuilder;
            this.shiftRepository = shiftRepository;
        }

        public override void BuildSelectLists(TWarehouseTransferViewModel warehouseTransferViewModel)
        {
            base.BuildSelectLists(warehouseTransferViewModel);
            warehouseTransferViewModel.ShiftSelectList = this.shiftSelectListBuilder.BuildSelectListItemsForShifts(this.shiftRepository.GetAllShifts());
        }
       
    }








    public interface IMaterialTransferViewModelSelectListBuilder : IWarehouseTransferViewModelSelectListBuilder<MaterialTransferViewModel>
    {
    }
    public class MaterialTransferViewModelSelectListBuilder : WarehouseTransferViewModelSelectListBuilder<MaterialTransferViewModel>, IMaterialTransferViewModelSelectListBuilder
    {
        public MaterialTransferViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, shiftSelectListBuilder, shiftRepository)
        { }
    }


    public interface IItemTransferViewModelSelectListBuilder : IWarehouseTransferViewModelSelectListBuilder<ItemTransferViewModel>
    {
    }
    public class ItemTransferViewModelSelectListBuilder : WarehouseTransferViewModelSelectListBuilder<ItemTransferViewModel>, IItemTransferViewModelSelectListBuilder
    {
        public ItemTransferViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, shiftSelectListBuilder, shiftRepository)
        { }
    }


    public interface IProductTransferViewModelSelectListBuilder : IWarehouseTransferViewModelSelectListBuilder<ProductTransferViewModel>
    {
    }
    public class ProductTransferViewModelSelectListBuilder : WarehouseTransferViewModelSelectListBuilder<ProductTransferViewModel>, IProductTransferViewModelSelectListBuilder
    {
        public ProductTransferViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, shiftSelectListBuilder, shiftRepository)
        { }
    }
}