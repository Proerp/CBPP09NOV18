[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TotalPortal.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TotalPortal.App_Start.NinjectWebCommon), "Stop")]

namespace TotalPortal.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;


    using TotalModel.Models;

    using TotalCore.Repositories;
    using TotalCore.Repositories.Generals;
    using TotalCore.Repositories.Commons;
    using TotalCore.Repositories.Sales;
    using TotalCore.Repositories.Purchases;
    using TotalCore.Repositories.Productions;
    using TotalCore.Repositories.Inventories;
    using TotalCore.Repositories.Accounts;
    using TotalCore.Repositories.Sessions;
    using TotalCore.Repositories.Analysis;

    using TotalCore.Services.Commons;
    using TotalCore.Services.Sales;
    using TotalCore.Services.Purchases;
    using TotalCore.Services.Productions;
    using TotalCore.Services.Inventories;
    using TotalCore.Services.Accounts;


    using TotalDAL.Repositories;
    using TotalDAL.Repositories.Generals;
    using TotalDAL.Repositories.Commons;
    using TotalDAL.Repositories.Sales;
    using TotalDAL.Repositories.Purchases;
    using TotalDAL.Repositories.Productions;
    using TotalDAL.Repositories.Inventories;
    using TotalDAL.Repositories.Accounts;
    using TotalDAL.Repositories.Sessions;
    using TotalDAL.Repositories.Analysis;


    using TotalService.Commons;
    using TotalService.Sales;
    using TotalService.Purchases;
    using TotalService.Productions;
    using TotalService.Inventories;
    using TotalService.Accounts;


    using TotalPortal.Areas.Sales.Builders;
    using TotalPortal.Areas.Purchases.Builders;
    using TotalPortal.Areas.Productions.Builders;
    using TotalPortal.Areas.Inventories.Builders;
    using TotalPortal.Areas.Commons.Builders;
    using TotalPortal.Areas.Accounts.Builders;
    
        
        
        



    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();


                kernel.Bind<TotalSmartPortalEntities>().ToSelf().InRequestScope();

                kernel.Bind<IBaseRepository>().To<BaseRepository>();

                kernel.Bind<IModuleRepository>().To<ModuleRepository>();
                kernel.Bind<IModuleDetailRepository>().To<ModuleDetailRepository>();

                kernel.Bind<IReportAPIRepository>().To<ReportAPIRepository>();


                kernel.Bind<IPurchaseRequisitionService>().To<PurchaseRequisitionService>();
                kernel.Bind<IPurchaseRequisitionRepository>().To<PurchaseRequisitionRepository>();
                kernel.Bind<IPurchaseRequisitionAPIRepository>().To<PurchaseRequisitionAPIRepository>();
                kernel.Bind<IPurchaseRequisitionViewModelSelectListBuilder>().To<PurchaseRequisitionViewModelSelectListBuilder>();

                kernel.Bind<IGoodsReceiptRepository>().To<GoodsReceiptRepository>();
                kernel.Bind<IGoodsReceiptAPIRepository>().To<GoodsReceiptAPIRepository>();

                kernel.Bind<IMaterialReceiptService>().To<MaterialReceiptService>();
                kernel.Bind<IMaterialReceiptViewModelSelectListBuilder>().To<MaterialReceiptViewModelSelectListBuilder>();
                kernel.Bind<IItemReceiptService>().To<ItemReceiptService>();
                kernel.Bind<IItemReceiptViewModelSelectListBuilder>().To<ItemReceiptViewModelSelectListBuilder>();
                kernel.Bind<IProductReceiptService>().To<ProductReceiptService>();
                kernel.Bind<IProductReceiptViewModelSelectListBuilder>().To<ProductReceiptViewModelSelectListBuilder>();                



                
                kernel.Bind<IMaterialIssueService>().To<MaterialIssueService>();
                kernel.Bind<IMaterialIssueRepository>().To<MaterialIssueRepository>();
                kernel.Bind<IMaterialIssueAPIRepository>().To<MaterialIssueAPIRepository>();
                kernel.Bind<IMaterialIssueViewModelSelectListBuilder>().To<MaterialIssueViewModelSelectListBuilder>();


                
                kernel.Bind<IWarehouseAdjustmentRepository>().To<WarehouseAdjustmentRepository>();
                kernel.Bind<IWarehouseAdjustmentAPIRepository>().To<WarehouseAdjustmentAPIRepository>();


                kernel.Bind<IOtherMaterialReceiptService>().To<OtherMaterialReceiptService>();
                kernel.Bind<IOtherMaterialReceiptViewModelSelectListBuilder>().To<OtherMaterialReceiptViewModelSelectListBuilder>();
                kernel.Bind<IOtherMaterialIssueService>().To<OtherMaterialIssueService>();
                kernel.Bind<IOtherMaterialIssueViewModelSelectListBuilder>().To<OtherMaterialIssueViewModelSelectListBuilder>();
                kernel.Bind<IMaterialAdjustmentService>().To<MaterialAdjustmentService>();
                kernel.Bind<IMaterialAdjustmentViewModelSelectListBuilder>().To<MaterialAdjustmentViewModelSelectListBuilder>();

                kernel.Bind<IOtherItemReceiptService>().To<OtherItemReceiptService>();
                kernel.Bind<IOtherItemReceiptViewModelSelectListBuilder>().To<OtherItemReceiptViewModelSelectListBuilder>();
                kernel.Bind<IOtherItemIssueService>().To<OtherItemIssueService>();
                kernel.Bind<IOtherItemIssueViewModelSelectListBuilder>().To<OtherItemIssueViewModelSelectListBuilder>();
                kernel.Bind<IItemAdjustmentService>().To<ItemAdjustmentService>();
                kernel.Bind<IItemAdjustmentViewModelSelectListBuilder>().To<ItemAdjustmentViewModelSelectListBuilder>();

                kernel.Bind<IOtherProductReceiptService>().To<OtherProductReceiptService>();
                kernel.Bind<IOtherProductReceiptViewModelSelectListBuilder>().To<OtherProductReceiptViewModelSelectListBuilder>();
                kernel.Bind<IOtherProductIssueService>().To<OtherProductIssueService>();
                kernel.Bind<IOtherProductIssueViewModelSelectListBuilder>().To<OtherProductIssueViewModelSelectListBuilder>();
                kernel.Bind<IProductAdjustmentService>().To<ProductAdjustmentService>();
                kernel.Bind<IProductAdjustmentViewModelSelectListBuilder>().To<ProductAdjustmentViewModelSelectListBuilder>();



                kernel.Bind<ITransferOrderRepository>().To<TransferOrderRepository>();
                kernel.Bind<ITransferOrderAPIRepository>().To<TransferOrderAPIRepository>();

                kernel.Bind<IMaterialTransferOrderService>().To<MaterialTransferOrderService>();
                kernel.Bind<IMaterialTransferOrderViewModelSelectListBuilder>().To<MaterialTransferOrderViewModelSelectListBuilder>();                
                kernel.Bind<IItemTransferOrderService>().To<ItemTransferOrderService>();
                kernel.Bind<IItemTransferOrderViewModelSelectListBuilder>().To<ItemTransferOrderViewModelSelectListBuilder>();                
                kernel.Bind<IProductTransferOrderService>().To<ProductTransferOrderService>();
                kernel.Bind<IProductTransferOrderViewModelSelectListBuilder>().To<ProductTransferOrderViewModelSelectListBuilder>();



                kernel.Bind<IWarehouseTransferRepository>().To<WarehouseTransferRepository>();
                kernel.Bind<IWarehouseTransferAPIRepository>().To<WarehouseTransferAPIRepository>();

                kernel.Bind<IMaterialTransferService>().To<MaterialTransferService>();
                kernel.Bind<IMaterialTransferViewModelSelectListBuilder>().To<MaterialTransferViewModelSelectListBuilder>();
                kernel.Bind<IItemTransferService>().To<ItemTransferService>();
                kernel.Bind<IItemTransferViewModelSelectListBuilder>().To<ItemTransferViewModelSelectListBuilder>();
                kernel.Bind<IProductTransferService>().To<ProductTransferService>();
                kernel.Bind<IProductTransferViewModelSelectListBuilder>().To<ProductTransferViewModelSelectListBuilder>();                





                kernel.Bind<IPlannedOrderService>().To<PlannedOrderService>();
                kernel.Bind<IPlannedOrderRepository>().To<PlannedOrderRepository>();
                kernel.Bind<IPlannedOrderAPIRepository>().To<PlannedOrderAPIRepository>();
                kernel.Bind<IPlannedOrderViewModelSelectListBuilder>().To<PlannedOrderViewModelSelectListBuilder>();

                kernel.Bind<IProductionOrderService>().To<ProductionOrderService>();
                kernel.Bind<IProductionOrderRepository>().To<ProductionOrderRepository>();
                kernel.Bind<IProductionOrderAPIRepository>().To<ProductionOrderAPIRepository>();
                kernel.Bind<IProductionOrderViewModelSelectListBuilder>().To<ProductionOrderViewModelSelectListBuilder>();

                kernel.Bind<ISemifinishedProductService>().To<SemifinishedProductService>();
                kernel.Bind<ISemifinishedProductRepository>().To<SemifinishedProductRepository>();
                kernel.Bind<ISemifinishedProductAPIRepository>().To<SemifinishedProductAPIRepository>();
                kernel.Bind<ISemifinishedProductViewModelSelectListBuilder>().To<SemifinishedProductViewModelSelectListBuilder>();

                kernel.Bind<ISemifinishedHandoverService>().To<SemifinishedHandoverService>();
                kernel.Bind<ISemifinishedHandoverRepository>().To<SemifinishedHandoverRepository>();
                kernel.Bind<ISemifinishedHandoverAPIRepository>().To<SemifinishedHandoverAPIRepository>();
                kernel.Bind<ISemifinishedHandoverViewModelSelectListBuilder>().To<SemifinishedHandoverViewModelSelectListBuilder>();

                kernel.Bind<IFinishedProductService>().To<FinishedProductService>();
                kernel.Bind<IFinishedProductRepository>().To<FinishedProductRepository>();
                kernel.Bind<IFinishedProductAPIRepository>().To<FinishedProductAPIRepository>();
                kernel.Bind<IFinishedProductViewModelSelectListBuilder>().To<FinishedProductViewModelSelectListBuilder>();

                kernel.Bind<IFinishedHandoverService>().To<FinishedHandoverService>();
                kernel.Bind<IFinishedHandoverRepository>().To<FinishedHandoverRepository>();
                kernel.Bind<IFinishedHandoverAPIRepository>().To<FinishedHandoverAPIRepository>();
                kernel.Bind<IFinishedHandoverViewModelSelectListBuilder>().To<FinishedHandoverViewModelSelectListBuilder>();



                kernel.Bind<ISalesOrderService>().To<SalesOrderService>();
                kernel.Bind<ISalesOrderRepository>().To<SalesOrderRepository>();
                kernel.Bind<ISalesOrderAPIRepository>().To<SalesOrderAPIRepository>();
                kernel.Bind<ISalesOrderHelperService>().To<SalesOrderHelperService>();
                kernel.Bind<ISalesOrderViewModelSelectListBuilder>().To<SalesOrderViewModelSelectListBuilder>();


                kernel.Bind<IDeliveryAdviceService>().To<DeliveryAdviceService>();
                kernel.Bind<IDeliveryAdviceRepository>().To<DeliveryAdviceRepository>();
                kernel.Bind<IDeliveryAdviceAPIRepository>().To<DeliveryAdviceAPIRepository>();
                kernel.Bind<IDeliveryAdviceHelperService>().To<DeliveryAdviceHelperService>();
                kernel.Bind<IDeliveryAdviceViewModelSelectListBuilder>().To<DeliveryAdviceViewModelSelectListBuilder>();


                kernel.Bind<ISalesReturnService>().To<SalesReturnService>();
                kernel.Bind<ISalesReturnRepository>().To<SalesReturnRepository>();
                kernel.Bind<ISalesReturnAPIRepository>().To<SalesReturnAPIRepository>();
                kernel.Bind<ISalesReturnViewModelSelectListBuilder>().To<SalesReturnViewModelSelectListBuilder>();


                kernel.Bind<IGoodsIssueService>().To<GoodsIssueService>();
                kernel.Bind<IGoodsIssueRepository>().To<GoodsIssueRepository>();
                kernel.Bind<IGoodsIssueAPIRepository>().To<GoodsIssueAPIRepository>();
                kernel.Bind<IGoodsIssueHelperService>().To<GoodsIssueHelperService>();
                kernel.Bind<IGoodsIssueViewModelSelectListBuilder>().To<GoodsIssueViewModelSelectListBuilder>();

                kernel.Bind<IHandlingUnitService>().To<HandlingUnitService>();
                kernel.Bind<IHandlingUnitRepository>().To<HandlingUnitRepository>();
                kernel.Bind<IHandlingUnitAPIRepository>().To<HandlingUnitAPIRepository>();
                kernel.Bind<IHandlingUnitViewModelSelectListBuilder>().To<HandlingUnitViewModelSelectListBuilder>();

                kernel.Bind<IGoodsDeliveryService>().To<GoodsDeliveryService>();
                kernel.Bind<IGoodsDeliveryRepository>().To<GoodsDeliveryRepository>();
                kernel.Bind<IGoodsDeliveryAPIRepository>().To<GoodsDeliveryAPIRepository>();
                kernel.Bind<IGoodsDeliveryViewModelSelectListBuilder>().To<GoodsDeliveryViewModelSelectListBuilder>();

                kernel.Bind<IAccountInvoiceService>().To<AccountInvoiceService>();
                kernel.Bind<IAccountInvoiceRepository>().To<AccountInvoiceRepository>();
                kernel.Bind<IAccountInvoiceAPIRepository>().To<AccountInvoiceAPIRepository>();
                kernel.Bind<IAccountInvoiceViewModelSelectListBuilder>().To<AccountInvoiceViewModelSelectListBuilder>();

                kernel.Bind<IReceiptService>().To<ReceiptService>();
                kernel.Bind<IReceiptRepository>().To<ReceiptRepository>();
                kernel.Bind<IReceiptAPIRepository>().To<ReceiptAPIRepository>();
                kernel.Bind<IReceiptViewModelSelectListBuilder>().To<ReceiptViewModelSelectListBuilder>();

                kernel.Bind<ICreditNoteService>().To<CreditNoteService>();
                kernel.Bind<ICreditNoteRepository>().To<CreditNoteRepository>();
                kernel.Bind<ICreditNoteAPIRepository>().To<CreditNoteAPIRepository>();
                kernel.Bind<ICreditNoteViewModelSelectListBuilder>().To<CreditNoteViewModelSelectListBuilder>();


                kernel.Bind<ICustomerService>().To<CustomerService>();
                kernel.Bind<ICustomerRepository>().To<CustomerRepository>();
                kernel.Bind<ICustomerAPIRepository>().To<CustomerAPIRepository>();
                kernel.Bind<ICustomerSelectListBuilder>().To<CustomerSelectListBuilder>();


                kernel.Bind<IEmployeeService>().To<EmployeeService>();
                kernel.Bind<IEmployeeRepository>().To<EmployeeRepository>();
                kernel.Bind<IEmployeeAPIRepository>().To<EmployeeAPIRepository>();
                kernel.Bind<IEmployeeSelectListBuilder>().To<EmployeeSelectListBuilder>();


                kernel.Bind<ICommodityPriceService>().To<CommodityPriceService>();
                kernel.Bind<ICommodityPriceRepository>().To<CommodityPriceRepository>();
                kernel.Bind<ICommodityPriceAPIRepository>().To<CommodityPriceAPIRepository>();
                kernel.Bind<ICommodityPriceSelectListBuilder>().To<CommodityPriceSelectListBuilder>();


                kernel.Bind<IPromotionService>().To<PromotionService>();
                kernel.Bind<IPromotionRepository>().To<PromotionRepository>();
                kernel.Bind<IPromotionAPIRepository>().To<PromotionAPIRepository>();
                kernel.Bind<IPromotionViewModelSelectListBuilder>().To<PromotionViewModelSelectListBuilder>();

                kernel.Bind<ISearchAPIRepository>().To<SearchAPIRepository>();                

                kernel.Bind<IInventoryRepository>().To<InventoryRepository>();

                kernel.Bind<IAspNetUserSelectListBuilder>().To<AspNetUserSelectListBuilder>();
                kernel.Bind<IPaymentTermSelectListBuilder>().To<PaymentTermSelectListBuilder>();
                kernel.Bind<IMonetaryAccountSelectListBuilder>().To<MonetaryAccountSelectListBuilder>();
                kernel.Bind<IPriceCategorySelectListBuilder>().To<PriceCategorySelectListBuilder>();
                kernel.Bind<ICustomerCategorySelectListBuilder>().To<CustomerCategorySelectListBuilder>();
                kernel.Bind<ICustomerTypeSelectListBuilder>().To<CustomerTypeSelectListBuilder>();
                kernel.Bind<ITerritorySelectListBuilder>().To<TerritorySelectListBuilder>();                
                kernel.Bind<IPackingMaterialSelectListBuilder>().To<PackingMaterialSelectListBuilder>();
                kernel.Bind<IVehicleSelectListBuilder>().To<VehicleSelectListBuilder>();

                kernel.Bind<ICommodityBrandSelectListBuilder>().To<CommodityBrandSelectListBuilder>();
                kernel.Bind<ICommodityCategorySelectListBuilder>().To<CommodityCategorySelectListBuilder>();
                kernel.Bind<ICommodityTypeSelectListBuilder>().To<CommodityTypeSelectListBuilder>();
                kernel.Bind<IWarehouseAdjustmentTypeSelectListBuilder>().To<WarehouseAdjustmentTypeSelectListBuilder>();
                kernel.Bind<ICommodityClassSelectListBuilder>().To<CommodityClassSelectListBuilder>();
                kernel.Bind<ICommodityLineSelectListBuilder>().To<CommodityLineSelectListBuilder>();
                kernel.Bind<IShiftSelectListBuilder>().To<ShiftSelectListBuilder>();
                kernel.Bind<ITransferOrderTypeSelectListBuilder>().To<TransferOrderTypeSelectListBuilder>();

                kernel.Bind<IAspNetUserRepository>().To<AspNetUserRepository>();
                kernel.Bind<IUserRepository>().To<UserRepository>();
                kernel.Bind<IUserAPIRepository>().To<UserAPIRepository>();

                kernel.Bind<IMaterialService>().To<MaterialService>();
                kernel.Bind<IItemService>().To<ItemService>();
                kernel.Bind<IProductService>().To<ProductService>();
                kernel.Bind<ICommodityRepository>().To<CommodityRepository>();
                kernel.Bind<ICommodityAPIRepository>().To<CommodityAPIRepository>();
                kernel.Bind<IMaterialSelectListBuilder>().To<MaterialSelectListBuilder>();
                kernel.Bind<IItemSelectListBuilder>().To<ItemSelectListBuilder>();
                kernel.Bind<IProductSelectListBuilder>().To<ProductSelectListBuilder>();

                kernel.Bind<IWarehouseRepository>().To<WarehouseRepository>();                
                kernel.Bind<IVoidTypeRepository>().To<VoidTypeRepository>();
                
                kernel.Bind<IPaymentTermRepository>().To<PaymentTermRepository>();
                kernel.Bind<IMonetaryAccountRepository>().To<MonetaryAccountRepository>();
                kernel.Bind<IPriceCategoryRepository>().To<PriceCategoryRepository>();
                kernel.Bind<ICustomerCategoryRepository>().To<CustomerCategoryRepository>();
                kernel.Bind<ICustomerTypeRepository>().To<CustomerTypeRepository>();
                kernel.Bind<ITerritoryRepository>().To<TerritoryRepository>();
                kernel.Bind<IPackingMaterialRepository>().To<PackingMaterialRepository>();
                kernel.Bind<IVehicleRepository>().To<VehicleRepository>();

                kernel.Bind<ICommodityBrandRepository>().To<CommodityBrandRepository>();
                kernel.Bind<ICommodityCategoryRepository>().To<CommodityCategoryRepository>();
                kernel.Bind<ICommodityTypeRepository>().To<CommodityTypeRepository>();
                kernel.Bind<IWarehouseAdjustmentTypeRepository>().To<WarehouseAdjustmentTypeRepository>();
                kernel.Bind<ICommodityClassRepository>().To<CommodityClassRepository>();
                kernel.Bind<ICommodityLineRepository>().To<CommodityLineRepository>();
                kernel.Bind<IShiftRepository>().To<ShiftRepository>();
                kernel.Bind<ITransferOrderTypeRepository>().To<TransferOrderTypeRepository>();


                kernel.Bind<IMoldService>().To<MoldService>();
                kernel.Bind<IMoldRepository>().To<MoldRepository>();
                kernel.Bind<IMoldAPIRepository>().To<MoldAPIRepository>();
                kernel.Bind<IMoldSelectListBuilder>().To<MoldSelectListBuilder>();


                //kernel.Bind<IBomService>().To<BomService>();
                kernel.Bind<IBomRepository>().To<BomRepository>();
                kernel.Bind<IBomAPIRepository>().To<BomAPIRepository>();
                //kernel.Bind<IBomSelectListBuilder>().To<BomSelectListBuilder>();

                //kernel.Bind<IProductionLineService>().To<ProductionLineService>();
                kernel.Bind<IProductionLineRepository>().To<ProductionLineRepository>();
                kernel.Bind<IProductionLineAPIRepository>().To<ProductionLineAPIRepository>();
                //kernel.Bind<IProductionLineSelectListBuilder>().To<ProductionLineSelectListBuilder>();


                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }
    }
}
