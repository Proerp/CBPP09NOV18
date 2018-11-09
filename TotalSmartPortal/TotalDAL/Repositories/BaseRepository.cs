using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Helpers;
using TotalCore.Repositories;


namespace TotalDAL.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        #region Main
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public BaseRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;

            this.RepositoryBag = new Dictionary<string, object>();
        }

        private ObjectContext TotalSmartPortalObjectContext
        {
            get { return ((IObjectContextAdapter)this.totalSmartPortalEntities).ObjectContext; }
        }

        public TotalSmartPortalEntities TotalSmartPortalEntities { get { return this.totalSmartPortalEntities; } }


        #region Menu
        public int GetModuleID(GlobalEnums.NmvnTaskID nmvnTaskID, int userID, ref int moduleDetailID)
        {
            int localModuleDetailID = moduleDetailID;
            if (localModuleDetailID == 0) localModuleDetailID = this.GetDefaultModuleDetailID(nmvnTaskID, userID);

            var moduleDetail = this.totalSmartPortalEntities.ModuleDetails.Where(w => w.TaskID == (int)nmvnTaskID && w.ModuleDetailID == localModuleDetailID).FirstOrDefault();
            if (moduleDetail == null)
            {
                localModuleDetailID = this.GetDefaultModuleDetailID(nmvnTaskID, userID);
                moduleDetail = this.totalSmartPortalEntities.ModuleDetails.Where(w => w.TaskID == (int)nmvnTaskID && w.ModuleDetailID == localModuleDetailID).FirstOrDefault();
            }

            moduleDetailID = localModuleDetailID;
            return moduleDetail != null ? moduleDetail.ModuleID : 0;
        }

        private int GetDefaultModuleDetailID(GlobalEnums.NmvnTaskID nmvnTaskID, int userID)
        {
            var moduleDefault = this.totalSmartPortalEntities.ModuleDefaults.Where(w => w.TaskID == (int)nmvnTaskID && w.UserID == (int)userID).FirstOrDefault();

            if (moduleDefault != null)
                return moduleDefault.ModuleDetailID;
            {
                var moduleDetail = this.totalSmartPortalEntities.ModuleDetails.Where(w => w.TaskID == (int)nmvnTaskID).FirstOrDefault();
                return moduleDetail != null ? moduleDetail.ModuleDetailID : 0;
            }
        }
        #endregion Menu

        /// <summary>
        ///     Detect whether the context is dirty (i.e., there are changes in entities in memory that have
        ///     not yet been saved to the database).
        /// </summary>
        /// <param name="context">The database context to check.</param>
        /// <returns>True if dirty (unsaved changes); false otherwise.</returns>
        public bool IsDirty()
        {
            //Contract.Requires<ArgumentNullException>(context != null);

            // Query the change tracker entries for any adds, modifications, or deletes.
            IEnumerable<DbEntityEntry> res = from e in this.totalSmartPortalEntities.ChangeTracker.Entries()
                                             where e.State.HasFlag(EntityState.Added) ||
                                                 e.State.HasFlag(EntityState.Modified) ||
                                                 e.State.HasFlag(EntityState.Deleted)
                                             select e;

            var myTestOnly = res.ToList();

            if (res.Any())
                return true;

            return false;
        }

        public Dictionary<string, object> RepositoryBag { get; set; }

        public virtual ICollection<TElement> ExecuteFunction<TElement>(string functionName, params ObjectParameter[] parameters)
        {
            this.TotalSmartPortalObjectContext.CommandTimeout = 300;
            var objectResult = this.TotalSmartPortalObjectContext.ExecuteFunction<TElement>(functionName, parameters);

            return objectResult.ToList<TElement>();
        }

        public virtual int ExecuteFunction(string functionName, params ObjectParameter[] parameters)
        {
            this.TotalSmartPortalObjectContext.CommandTimeout = 300;
            return this.TotalSmartPortalObjectContext.ExecuteFunction(functionName, parameters);
        }

        public virtual int ExecuteStoreCommand(string commandText, params Object[] parameters)
        {
            this.TotalSmartPortalObjectContext.CommandTimeout = 300;
            return this.TotalSmartPortalObjectContext.ExecuteStoreCommand(commandText, parameters);
        }




        public T GetEntity<T>(bool proxyCreationEnabled, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (!proxyCreationEnabled) this.totalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;


            IQueryable<T> result = this.totalSmartPortalEntities.Set<T>();

            if (includes != null && includes.Any())
                result = includes.Aggregate(result, (current, include) => current.Include(include));


            T entity = null;

            if (predicate != null)
                entity = result.FirstOrDefault(predicate);
            else
                entity = result.FirstOrDefault();


            if (!proxyCreationEnabled) this.totalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;


            return entity;
        }


        public T GetEntity<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return this.GetEntity<T>(true, predicate, includes);
        }

        public T GetEntity<T>(bool proxyCreationEnabled, params Expression<Func<T, object>>[] includes) where T : class
        {
            return this.GetEntity<T>(proxyCreationEnabled, null, includes);
        }

        public T GetEntity<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return this.GetEntity<T>(null, includes);
        }






        public ICollection<T> GetEntities<T>(bool proxyCreationEnabled, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (!proxyCreationEnabled) this.totalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;


            IQueryable<T> result = this.totalSmartPortalEntities.Set<T>();

            if (includes != null && includes.Any())
                result = includes.Aggregate(result, (current, include) => current.Include(include));

            ICollection<T> entities = null;

            if (predicate != null)
                entities = result.Where(predicate).ToList();
            else
                entities = result.ToList();



            if (!proxyCreationEnabled) this.totalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return entities;

        }

        public ICollection<T> GetEntities<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return this.GetEntities<T>(true, predicate, includes);
        }

        public ICollection<T> GetEntities<T>(bool proxyCreationEnabled, params Expression<Func<T, object>>[] includes) where T : class
        {
            return this.GetEntities<T>(proxyCreationEnabled, null, includes);
        }

        public ICollection<T> GetEntities<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return this.GetEntities<T>(null, includes);
        }


        public String GetSystemInfos()
        {
            return this.GetSystemInfos(false);
        }

        public String GetSystemInfos(bool secureEncoding)
        {
            return SystemInfos.GetSystemInfos(secureEncoding);
        }

        public bool SystemInfoValidate()
        {
            return SystemInfos.Validate();
        }


        #endregion Main
























        #region Version basic
        public int? GetStoredID(int configID)
        {
            return this.totalSmartPortalEntities.GetStoredID(configID).Single();
        }

        public int? GetVersionID(int configID)
        {
            return this.totalSmartPortalEntities.GetVersionID(configID).Single();
        }

        public bool VersionValidate(bool bothKeyID)
        {
            if (bothKeyID)
            { //CHECK BOTH VersionID AND StoredID
                if (this.totalSmartPortalEntities.GetVersionValidate(GlobalEnums.ConfigID, GlobalEnums.ConfigVersionID(GlobalEnums.ConfigID)).Single() == null)
                    this.HandleException("This web application must be updated. Please contact your administrators.");
            }
            else
            { //CHECK ONLY VersionID.
                int? versionID = this.GetVersionID(GlobalEnums.ConfigID);
                if (versionID == null || (int)versionID != GlobalEnums.ConfigVersionID(GlobalEnums.ConfigID)) this.HandleException("This web application must be updated. Please contact your administrators.");
            }
            return true;
        }

        private void HandleException(string message)
        {
            throw new Exception(message);
        }





        public bool AutoUpdates(bool restoreProcedures)
        {
            if (restoreProcedures || this.GetStoredID(GlobalEnums.ConfigID) < GlobalEnums.MaxConfigVersionID()) this.RestoreProcedures();

            return this.GetStoredID(GlobalEnums.ConfigID) == GlobalEnums.MaxConfigVersionID();
        }

        #endregion Version basic



        #region Version Update

        private bool RestoreProcedures()
        {

            var commodities = this.totalSmartPortalEntities.Commodities.ToList();
            foreach (Commodity commodity in commodities)
            {
                if (commodity.CodePartB.IndexOf("[") > 0 || commodity.CodePartC.IndexOf("[") > 0 || (commodity.CodePartD != null && commodity.CodePartD.IndexOf("[") > 0)) throw new Exception("[9999999999999");

                string newCode = ((commodity.CommodityTypeID != (int)GlobalEnums.CommodityTypeID.Items && !String.IsNullOrWhiteSpace(commodity.CodePartA) ? commodity.CodePartA + " " : "") + (!String.IsNullOrWhiteSpace(commodity.CodePartB) ? commodity.CodePartB + " " : "") + (!String.IsNullOrWhiteSpace(commodity.CodePartC) ? commodity.CodePartC + " " : "") + (!String.IsNullOrWhiteSpace(commodity.CodePartD) ? commodity.CodePartD + " " : "") + (commodity.CommodityTypeID == (int)GlobalEnums.CommodityTypeID.Items && !String.IsNullOrWhiteSpace(commodity.CodePartA) ? commodity.CodePartA + " " : "") + (!String.IsNullOrWhiteSpace(commodity.CodePartE) ? commodity.CodePartE + " x " : "") + (!String.IsNullOrWhiteSpace(commodity.CodePartF) ? commodity.CodePartF : "")).Trim();
                this.ExecuteStoreCommand("UPDATE Commodities SET Code = N'" + newCode + "', OfficialCode = N'" + TotalBase.CommonExpressions.AlphaNumericString(newCode) + "' WHERE CommodityID = " + commodity.CommodityID, new ObjectParameter[] { });
            }




            this.totalSmartPortalEntities.ColumnAdd("Repacks", "SerialID", "int", "0", true);
            this.totalSmartPortalEntities.ColumnAdd("WarehouseAdjustments", "CustomerID", "int", "1", true);
            this.totalSmartPortalEntities.ColumnAdd("WarehouseAdjustmentDetails", "CustomerID", "int", "1", true);

            this.InitReports();

            this.CreateStoredProcedure();

            //SET LASTEST VERSION AFTER RESTORE SUCCESSFULL
            this.ExecuteStoreCommand("UPDATE Configs SET StoredID = " + GlobalEnums.MaxConfigVersionID() + " WHERE StoredID < " + GlobalEnums.MaxConfigVersionID(), new ObjectParameter[] { });

            return true;
        }


        public void CreateStoredProcedure()
        {
            //return;

            Helpers.SqlProgrammability.Inventories.WarehouseTransfer warehouseTransfer = new Helpers.SqlProgrammability.Inventories.WarehouseTransfer(totalSmartPortalEntities);
            warehouseTransfer.RestoreProcedure();


            //return;

            Helpers.SqlProgrammability.Inventories.TransferOrder transferOrder = new Helpers.SqlProgrammability.Inventories.TransferOrder(totalSmartPortalEntities);
            transferOrder.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Inventories.WarehouseAdjustment warehouseAdjustment = new Helpers.SqlProgrammability.Inventories.WarehouseAdjustment(totalSmartPortalEntities);
            warehouseAdjustment.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Inventories.MaterialIssue materialIssue = new Helpers.SqlProgrammability.Inventories.MaterialIssue(totalSmartPortalEntities);
            materialIssue.RestoreProcedure();


            //return;

            Helpers.SqlProgrammability.Commons.Commodity commodity = new Helpers.SqlProgrammability.Commons.Commodity(totalSmartPortalEntities);
            commodity.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Productions.PlannedOrder plannedOrder = new Helpers.SqlProgrammability.Productions.PlannedOrder(totalSmartPortalEntities);
            plannedOrder.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Productions.ProductionOrder productionOrder = new Helpers.SqlProgrammability.Productions.ProductionOrder(totalSmartPortalEntities);
            productionOrder.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Productions.SemifinishedProduct semifinishedProduct = new Helpers.SqlProgrammability.Productions.SemifinishedProduct(totalSmartPortalEntities);
            semifinishedProduct.RestoreProcedure();

            Helpers.SqlProgrammability.Productions.SemifinishedHandover semifinishedHandover = new Helpers.SqlProgrammability.Productions.SemifinishedHandover(totalSmartPortalEntities);
            semifinishedHandover.RestoreProcedure();

            Helpers.SqlProgrammability.Productions.FinishedProduct finishedProduct = new Helpers.SqlProgrammability.Productions.FinishedProduct(totalSmartPortalEntities);
            finishedProduct.RestoreProcedure();

            Helpers.SqlProgrammability.Productions.FinishedHandover finishedHandover = new Helpers.SqlProgrammability.Productions.FinishedHandover(totalSmartPortalEntities);
            finishedHandover.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Purchases.PurchaseRequisition purchaseRequisition = new Helpers.SqlProgrammability.Purchases.PurchaseRequisition(totalSmartPortalEntities);
            purchaseRequisition.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Commons.ProductionLine productionLine = new Helpers.SqlProgrammability.Commons.ProductionLine(totalSmartPortalEntities);
            productionLine.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Commons.Mold mold = new Helpers.SqlProgrammability.Commons.Mold(totalSmartPortalEntities);
            mold.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Commons.Workshift workshift = new Helpers.SqlProgrammability.Commons.Workshift(totalSmartPortalEntities);
            workshift.RestoreProcedure();




            //return;
            Helpers.SqlProgrammability.Generals.AccessControl accessControl = new Helpers.SqlProgrammability.Generals.AccessControl(totalSmartPortalEntities);
            accessControl.RestoreProcedure();


            //return;

            Helpers.SqlProgrammability.Inventories.GoodsReceipt goodsReceipt = new Helpers.SqlProgrammability.Inventories.GoodsReceipt(totalSmartPortalEntities);
            goodsReceipt.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Reports.ProductionReports productionReports = new Helpers.SqlProgrammability.Reports.ProductionReports(totalSmartPortalEntities);
            productionReports.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Reports.InventoryReports inventoryReports = new Helpers.SqlProgrammability.Reports.InventoryReports(totalSmartPortalEntities);
            inventoryReports.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Analysis.Report report = new Helpers.SqlProgrammability.Analysis.Report(totalSmartPortalEntities);
            report.RestoreProcedure();


            //return;

            Helpers.SqlProgrammability.Commons.Customer customer = new Helpers.SqlProgrammability.Commons.Customer(totalSmartPortalEntities);
            customer.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Sales.SalesOrder salesOrder = new Helpers.SqlProgrammability.Sales.SalesOrder(totalSmartPortalEntities);
            salesOrder.RestoreProcedure();

            //return;


            Helpers.SqlProgrammability.Sales.DeliveryAdvice deliveryAdvice = new Helpers.SqlProgrammability.Sales.DeliveryAdvice(totalSmartPortalEntities);
            deliveryAdvice.RestoreProcedure();

            Helpers.SqlProgrammability.Commons.Bom bom = new Helpers.SqlProgrammability.Commons.Bom(totalSmartPortalEntities);
            bom.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Generals.UserReference userReference = new Helpers.SqlProgrammability.Generals.UserReference(totalSmartPortalEntities);
            userReference.RestoreProcedure();



            return;

            Helpers.SqlProgrammability.Reports.SaleReports saleReports = new Helpers.SqlProgrammability.Reports.SaleReports(totalSmartPortalEntities);
            saleReports.RestoreProcedure();









            return;

            return;

            Helpers.SqlProgrammability.Accounts.Receipt receipt = new Helpers.SqlProgrammability.Accounts.Receipt(totalSmartPortalEntities);
            receipt.RestoreProcedure();






            return;
            return;

            Helpers.SqlProgrammability.Inventories.HandlingUnit handlingUnit = new Helpers.SqlProgrammability.Inventories.HandlingUnit(totalSmartPortalEntities);
            handlingUnit.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Inventories.GoodsIssue goodsIssue = new Helpers.SqlProgrammability.Inventories.GoodsIssue(totalSmartPortalEntities);
            goodsIssue.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Commons.CommodityPrice commodityPrice = new Helpers.SqlProgrammability.Commons.CommodityPrice(totalSmartPortalEntities);
            commodityPrice.RestoreProcedure();









            //return;

            //return;
            Helpers.SqlProgrammability.Inventories.Inventories inventories = new Helpers.SqlProgrammability.Inventories.Inventories(totalSmartPortalEntities);
            inventories.RestoreProcedure();



            //return;

            Helpers.SqlProgrammability.Sales.SalesReturn salesReturn = new Helpers.SqlProgrammability.Sales.SalesReturn(totalSmartPortalEntities);
            salesReturn.RestoreProcedure();





            //return;

            Helpers.SqlProgrammability.Commons.Promotion promotion = new Helpers.SqlProgrammability.Commons.Promotion(totalSmartPortalEntities);
            promotion.RestoreProcedure();

            //return;

            //AccountInvoice: NOT CHECK FOR Approved COMMPLETELY, PLS CHECK IT CAREFULLY LATER. (SaveRelative, GetPendingGoodsIssueDetails, ...). ALSO DO THE SAME CHECK FOR ALL OTHER MODULES
            //AccountInvoice: SHOULD SAVE BillingAddress

            Helpers.SqlProgrammability.Accounts.AccountInvoice accountInvoice = new Helpers.SqlProgrammability.Accounts.AccountInvoice(totalSmartPortalEntities);
            accountInvoice.RestoreProcedure();



            //return;


            Helpers.SqlProgrammability.Commons.Commons commons = new Helpers.SqlProgrammability.Commons.Commons(totalSmartPortalEntities);
            commons.RestoreProcedure();


            //return;

            Helpers.SqlProgrammability.Commons.Employee employee = new Helpers.SqlProgrammability.Commons.Employee(totalSmartPortalEntities);
            employee.RestoreProcedure();



            //return;

            Helpers.SqlProgrammability.Accounts.CreditNote creditNote = new Helpers.SqlProgrammability.Accounts.CreditNote(totalSmartPortalEntities);
            creditNote.RestoreProcedure();

            //return;
            Helpers.SqlProgrammability.Inventories.GoodsDelivery goodsDelivery = new Helpers.SqlProgrammability.Inventories.GoodsDelivery(totalSmartPortalEntities);
            goodsDelivery.RestoreProcedure();


        }


        private void InitReports()
        {
            ////this.ExecuteStoreCommand("DELETE FROM Reports", new ObjectParameter[] { });

            ////string optionBoxIDs = "";
            ////string reportTabPageIDs = ((int)GlobalEnums.ReportTabPageID.TabPageBatchMasters).ToString() + "," + ((int)GlobalEnums.ReportTabPageID.TabPageBatchTypes).ToString();

            ////this.ExecuteStoreCommand("SET IDENTITY_INSERT Reports ON  INSERT INTO Reports (ReportID, ReportUniqueID, ReportGroupID, ReportGroupName, ReportName, ReportURL, ReportTabPageIDs, OptionBoxIDs, ReportTypeID, SerialID, Remarks) VALUES (" + (int)GlobalEnums.ReportID.ProductionJournal001 + ", " + (int)GlobalEnums.ReportID.ProductionJournal001 + ", 1, N'1.BÁO CÁO THEO BATCH', N'Báo cáo thành phẩm theo batch', N'ProductionJournal001', N'" + reportTabPageIDs + "', N'" + optionBoxIDs + "', " + (int)GlobalEnums.ReportTypeID.ProductionJournal + ", 10, N'')      SET IDENTITY_INSERT Reports OFF ", new ObjectParameter[] { });

            ////optionBoxIDs = GlobalEnums.OBx(GlobalEnums.OptionBoxID.PalletVersusCartonAndPack);
            ////this.ExecuteStoreCommand("SET IDENTITY_INSERT Reports ON  INSERT INTO Reports (ReportID, ReportUniqueID, ReportGroupID, ReportGroupName, ReportName, ReportURL, ReportTabPageIDs, OptionBoxIDs, ReportTypeID, SerialID, Remarks) VALUES (" + (int)GlobalEnums.ReportID.ProductionJournal002 + ", " + (int)GlobalEnums.ReportID.ProductionJournal002 + ", 1, N'1.BÁO CÁO THEO BATCH', N'Báo cáo thành phẩm chi tiết', N'ProductionJournal003', N'" + reportTabPageIDs + "', N'" + optionBoxIDs + "', " + (int)GlobalEnums.ReportTypeID.ProductionJournal + ", 20, N'')      SET IDENTITY_INSERT Reports OFF ", new ObjectParameter[] { });
            ////this.ExecuteStoreCommand("SET IDENTITY_INSERT Reports ON  INSERT INTO Reports (ReportID, ReportUniqueID, ReportGroupID, ReportGroupName, ReportName, ReportURL, ReportTabPageIDs, OptionBoxIDs, ReportTypeID, SerialID, Remarks) VALUES (" + (int)GlobalEnums.ReportID.ProductionJournal003 + ", " + (int)GlobalEnums.ReportID.ProductionJournal003 + ", 1, N'1.BÁO CÁO THEO BATCH', N'Báo cáo thành phẩm tổng hợp', N'ProductionJournal002', N'" + reportTabPageIDs + "', N'" + optionBoxIDs + GlobalEnums.OBx(GlobalEnums.OptionBoxID.SummaryVersusDetail) + "', " + (int)GlobalEnums.ReportTypeID.ProductionJournal + ", 30, N'')      SET IDENTITY_INSERT Reports OFF ", new ObjectParameter[] { });


            ////optionBoxIDs = GlobalEnums.OBx(GlobalEnums.OptionBoxID.FromDate) + GlobalEnums.OBx(GlobalEnums.OptionBoxID.ToDate) + GlobalEnums.OBx(GlobalEnums.OptionBoxID.PalletVersusCartonAndPack);
            ////reportTabPageIDs = ((int)GlobalEnums.ReportTabPageID.TabPageFillingLines).ToString() + "," + ((int)GlobalEnums.ReportTabPageID.TabPageBatchTypes).ToString() + "," + ((int)GlobalEnums.ReportTabPageID.TabPageCommodities).ToString();

            ////this.ExecuteStoreCommand("SET IDENTITY_INSERT Reports ON  INSERT INTO Reports (ReportID, ReportUniqueID, ReportGroupID, ReportGroupName, ReportName, ReportURL, ReportTabPageIDs, OptionBoxIDs, ReportTypeID, SerialID, Remarks) VALUES (" + (int)GlobalEnums.ReportID.ProductionJournal004 + ", " + (int)GlobalEnums.ReportID.ProductionJournal004 + ", 2, N'2.BÁO CÁO THEO NGÀY SẢN XUẤT', N'Báo cáo thành phẩm chi tiết', N'ProductionJournal003', N'" + reportTabPageIDs + "', N'" + optionBoxIDs + "', " + (int)GlobalEnums.ReportTypeID.ProductionJournal + ", 20, N'')      SET IDENTITY_INSERT Reports OFF ", new ObjectParameter[] { });
            ////this.ExecuteStoreCommand("SET IDENTITY_INSERT Reports ON  INSERT INTO Reports (ReportID, ReportUniqueID, ReportGroupID, ReportGroupName, ReportName, ReportURL, ReportTabPageIDs, OptionBoxIDs, ReportTypeID, SerialID, Remarks) VALUES (" + (int)GlobalEnums.ReportID.ProductionJournal005 + ", " + (int)GlobalEnums.ReportID.ProductionJournal005 + ", 2, N'2.BÁO CÁO THEO NGÀY SẢN XUẤT', N'Báo cáo thành phẩm tổng hợp', N'ProductionJournal002', N'" + reportTabPageIDs + "', N'" + optionBoxIDs + GlobalEnums.OBx(GlobalEnums.OptionBoxID.SummaryVersusDetail) + "', " + (int)GlobalEnums.ReportTypeID.ProductionJournal + ", 30, N'')      SET IDENTITY_INSERT Reports OFF ", new ObjectParameter[] { });
        }


        private void Backup()
        {
            //////////////this.ExecuteStoreCommand("INSERT INTO ModuleDetails (ModuleDetailID, ModuleID, Code, Name, FullName, Actions, Controller, LastOpen, SerialID, ImageIndex, InActive) VALUES(" + (int)GlobalEnums.NmvnTaskID.Repack + ", 108, 'Reports', 'Reports', '#', '#', '#', 1, 10, 1, 0) ", new ObjectParameter[] { });
            //////////////this.ExecuteStoreCommand("INSERT INTO AccessControls (UserID, NMVNTaskID, OrganizationalUnitID, AccessLevel, ApprovalPermitted, UnApprovalPermitted, VoidablePermitted, UnVoidablePermitted, ShowDiscount, AccessLevelBACKUP, ApprovalPermittedBACKUP, UnApprovalPermittedBACKUP) SELECT UserID, " + (int)GlobalEnums.NmvnTaskID.Repack + " AS NMVNTaskID, OrganizationalUnitID, 2 AS AccessLevel, ApprovalPermitted, UnApprovalPermitted, VoidablePermitted, UnVoidablePermitted, ShowDiscount, AccessLevelBACKUP, ApprovalPermittedBACKUP, UnApprovalPermittedBACKUP FROM AccessControls WHERE (NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.Pack + ") AND (SELECT COUNT(*) FROM AccessControls WHERE NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.Repack + ") = 0", new ObjectParameter[] { }); 


            ////////////////////this.ExecuteStoreCommand("INSERT INTO ModuleDetails (ModuleDetailID, ModuleID, Code, Name, FullName, Actions, Controller, LastOpen, SerialID, ImageIndex, InActive) VALUES(" + (int)GlobalEnums.NmvnTaskID.BatchMaster + ", 108, 'Reports', 'Reports', '#', '#', '#', 1, 10, 1, 0) ", new ObjectParameter[] { });
            ////////////////////this.ExecuteStoreCommand("INSERT INTO AccessControls (UserID, NMVNTaskID, OrganizationalUnitID, AccessLevel, ApprovalPermitted, UnApprovalPermitted, VoidablePermitted, UnVoidablePermitted, ShowDiscount, AccessLevelBACKUP, ApprovalPermittedBACKUP, UnApprovalPermittedBACKUP) SELECT UserID, " + (int)GlobalEnums.NmvnTaskID.BatchMaster + " AS NMVNTaskID, OrganizationalUnitID, 2 AS AccessLevel, ApprovalPermitted, UnApprovalPermitted, VoidablePermitted, UnVoidablePermitted, ShowDiscount, AccessLevelBACKUP, ApprovalPermittedBACKUP, UnApprovalPermittedBACKUP FROM AccessControls WHERE (NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.Commodity + ") AND (SELECT COUNT(*) FROM AccessControls WHERE NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.BatchMaster + ") = 0", new ObjectParameter[] { }); 



            //this.ExecuteStoreCommand("INSERT INTO ModuleDetails (ModuleDetailID, ModuleID, Code, Name, FullName, Actions, Controller, LastOpen, SerialID, ImageIndex, InActive) VALUES(" + (int)GlobalEnums.NmvnTaskID.Report + ", 9, 'Reports', 'Reports', '#', '#', '#', 1, 10, 1, 0) ", new ObjectParameter[] { });
            //this.ExecuteStoreCommand("INSERT INTO AccessControls (UserID, NMVNTaskID, OrganizationalUnitID, AccessLevel, ApprovalPermitted, UnApprovalPermitted, VoidablePermitted, UnVoidablePermitted, ShowDiscount, AccessLevelBACKUP, ApprovalPermittedBACKUP, UnApprovalPermittedBACKUP) SELECT UserID, " + (int)GlobalEnums.NmvnTaskID.Report + " AS NMVNTaskID, OrganizationalUnitID, 1 AS AccessLevel, ApprovalPermitted, UnApprovalPermitted, VoidablePermitted, UnVoidablePermitted, ShowDiscount, AccessLevelBACKUP, ApprovalPermittedBACKUP, UnApprovalPermittedBACKUP FROM AccessControls WHERE (NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.Commodity + ") AND (SELECT COUNT(*) FROM AccessControls WHERE NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.Report + ") = 0", new ObjectParameter[] { });

            if (!this.totalSmartPortalEntities.ColumnExists("Pallets", "MinPackDate"))
            {
                this.totalSmartPortalEntities.ColumnAdd("Pallets", "MinPackDate", "datetime", null, false);
                this.totalSmartPortalEntities.ColumnAdd("Pallets", "MaxPackDate", "datetime", null, false);

                this.ExecuteStoreCommand(" UPDATE      Pallets     SET     MinPackDate = (SELECT MIN(Packs.EntryDate) FROM Packs INNER JOIN Cartons ON Packs.CartonID = Cartons.CartonID WHERE Cartons.PalletID = Pallets.PalletID) ", new ObjectParameter[] { });
                this.ExecuteStoreCommand(" UPDATE      Pallets     SET     MaxPackDate = (SELECT MAX(Packs.EntryDate) FROM Packs INNER JOIN Cartons ON Packs.CartonID = Cartons.CartonID WHERE Cartons.PalletID = Pallets.PalletID) ", new ObjectParameter[] { });

                this.ExecuteStoreCommand(" ALTER TABLE Pallets ALTER COLUMN MinPackDate datetime NOT NULL", new ObjectParameter[] { });
                this.ExecuteStoreCommand(" ALTER TABLE Pallets ALTER COLUMN MaxPackDate datetime NOT NULL", new ObjectParameter[] { });

                this.ExecuteStoreCommand(" UPDATE       Commodities SET Unit = N'Lon'", new ObjectParameter[] { });
            }
        }

        #endregion Version Update
    }
}
