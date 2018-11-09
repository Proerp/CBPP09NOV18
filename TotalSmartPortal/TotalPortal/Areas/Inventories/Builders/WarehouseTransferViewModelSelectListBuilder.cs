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
        public WarehouseTransferViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {         
        }
       
    }








    public interface IMaterialTransferViewModelSelectListBuilder : IWarehouseTransferViewModelSelectListBuilder<MaterialTransferViewModel>
    {
    }
    public class MaterialTransferViewModelSelectListBuilder : WarehouseTransferViewModelSelectListBuilder<MaterialTransferViewModel>, IMaterialTransferViewModelSelectListBuilder
    {
        public MaterialTransferViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }


    public interface IItemTransferViewModelSelectListBuilder : IWarehouseTransferViewModelSelectListBuilder<ItemTransferViewModel>
    {
    }
    public class ItemTransferViewModelSelectListBuilder : WarehouseTransferViewModelSelectListBuilder<ItemTransferViewModel>, IItemTransferViewModelSelectListBuilder
    {
        public ItemTransferViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }


    public interface IProductTransferViewModelSelectListBuilder : IWarehouseTransferViewModelSelectListBuilder<ProductTransferViewModel>
    {
    }
    public class ProductTransferViewModelSelectListBuilder : WarehouseTransferViewModelSelectListBuilder<ProductTransferViewModel>, IProductTransferViewModelSelectListBuilder
    {
        public ProductTransferViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }
}