using System.Web.Mvc;
using System.Collections.Generic;

using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{   
    public interface ITransferOrderViewModelSelectListBuilder<TTransferOrderViewModel> : IViewModelSelectListBuilder<TTransferOrderViewModel>
        where TTransferOrderViewModel : ITransferOrderViewModel
    {
    }

    public class TransferOrderViewModelSelectListBuilder<TTransferOrderViewModel> : A01ViewModelSelectListBuilder<TTransferOrderViewModel>, ITransferOrderViewModelSelectListBuilder<TTransferOrderViewModel>
        where TTransferOrderViewModel : ITransferOrderViewModel
    {
        private readonly ITransferOrderTypeSelectListBuilder transferOrderTypeSelectListBuilder;
        private readonly ITransferOrderTypeRepository transferOrderTypeRepository;

        public TransferOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, ITransferOrderTypeSelectListBuilder transferOrderTypeSelectListBuilder, ITransferOrderTypeRepository transferOrderTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.transferOrderTypeSelectListBuilder = transferOrderTypeSelectListBuilder;
            this.transferOrderTypeRepository = transferOrderTypeRepository;
        }

        public override void BuildSelectLists(TTransferOrderViewModel transferOrderViewModel)
        {
            base.BuildSelectLists(transferOrderViewModel);
            transferOrderViewModel.TransferOrderTypeSelectList = this.transferOrderTypeSelectListBuilder.BuildSelectListItemsForTransferOrderTypes(this.transferOrderTypeRepository.GetAllTransferOrderTypes());
        }
    }








    public interface IMaterialTransferOrderViewModelSelectListBuilder : ITransferOrderViewModelSelectListBuilder<MaterialTransferOrderViewModel>
    {
    }
    public class MaterialTransferOrderViewModelSelectListBuilder : TransferOrderViewModelSelectListBuilder<MaterialTransferOrderViewModel>, IMaterialTransferOrderViewModelSelectListBuilder
    {
        public MaterialTransferOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, ITransferOrderTypeSelectListBuilder transferOrderTypeSelectListBuilder, ITransferOrderTypeRepository transferOrderTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, transferOrderTypeSelectListBuilder, transferOrderTypeRepository)
        { }
    }

 
    public interface IItemTransferOrderViewModelSelectListBuilder : ITransferOrderViewModelSelectListBuilder<ItemTransferOrderViewModel>
    {
    }
    public class ItemTransferOrderViewModelSelectListBuilder : TransferOrderViewModelSelectListBuilder<ItemTransferOrderViewModel>, IItemTransferOrderViewModelSelectListBuilder
    {
        public ItemTransferOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, ITransferOrderTypeSelectListBuilder transferOrderTypeSelectListBuilder, ITransferOrderTypeRepository transferOrderTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, transferOrderTypeSelectListBuilder, transferOrderTypeRepository)
        { }
    }


     public interface IProductTransferOrderViewModelSelectListBuilder : ITransferOrderViewModelSelectListBuilder<ProductTransferOrderViewModel>
    {
    }
    public class ProductTransferOrderViewModelSelectListBuilder : TransferOrderViewModelSelectListBuilder<ProductTransferOrderViewModel>, IProductTransferOrderViewModelSelectListBuilder
    {
        public ProductTransferOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, ITransferOrderTypeSelectListBuilder transferOrderTypeSelectListBuilder, ITransferOrderTypeRepository transferOrderTypeRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, transferOrderTypeSelectListBuilder, transferOrderTypeRepository)
        { }
    }

  
}