function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(goodsReceiptGridDataSource, pendingPlannedOrderDetailGridDataSource) {
    if (goodsReceiptGridDataSource != undefined && pendingPlannedOrderDetailGridDataSource != undefined) {
        var pendingPurchaseRequisitionDetailGridDataItems = pendingPlannedOrderDetailGridDataSource.view();
        var goodsReceiptJSON = goodsReceiptGridDataSource.data().toJSON();
        for (var i = 0; i < pendingPurchaseRequisitionDetailGridDataItems.length; i++) {
            if (pendingPurchaseRequisitionDetailGridDataItems[i].IsSelected === true)
                _setParentInput(goodsReceiptJSON, pendingPurchaseRequisitionDetailGridDataItems[i]);
        }

        goodsReceiptJSON.push(new Object()); //Add a temporary empty row

        goodsReceiptGridDataSource.data(goodsReceiptJSON);

        var rawData = goodsReceiptGridDataSource.data()
        goodsReceiptGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row

        cancelButton_Click();
    }


    //http://www.telerik.com/forums/adding-multiple-rows-performance
    //By design the dataSource does not provide an opportunity for inserting multiple records via one operation. The performance is low, because each time when you add row through the addRow method, the DataSource throws its change event which forces the Grid to refresh and re-paint the content.
    //To avoid the problem you may try to modify the data of the DataSource manually.
    //var grid = $("#grid").data("kendoGrid");
    //var data = gr.dataSource.data().toJSON(); //the data of the DataSource

    ////change the data array
    ////any changes in the data array will not automatically reflect in the Grid

    //grid.dataSource.data(data); //set changed data as data of the Grid


    function _setParentInput(goodsReceiptJSON, plannedOrderGridDataItem) {

        //var dataRow = goodsReceiptJSON.add({});

        var dataRow = new Object();

        dataRow.LocationID = null;
        dataRow.EntryDate = null;

        dataRow.GoodsReceiptDetailID = 0;
        dataRow.GoodsReceiptID = window.parent.$("#GoodsReceiptID").val();


        dataRow.MaterialIssueID = null;
        dataRow.MaterialIssueDetailID = null;
        dataRow.MaterialIssueEntryDate = null;
        dataRow.ProductionLinesCode = null;

        dataRow.WorkshiftName = null;
        dataRow.WorkshiftEntryDate = null;
        

        dataRow.FinishedProductID = plannedOrderGridDataItem.FinishedProductID;
        dataRow.FinishedProductPackageID = plannedOrderGridDataItem.FinishedProductPackageID;
        dataRow.FinishedProductEntryDate = plannedOrderGridDataItem.FinishedProductEntryDate;
        dataRow.FirmOrderReference = plannedOrderGridDataItem.FirmOrderReference;
        dataRow.FirmOrderCode = plannedOrderGridDataItem.FirmOrderCode;
        dataRow.FirmOrderSpecs = null;
        dataRow.SemifinishedProductReferences = plannedOrderGridDataItem.SemifinishedProductReferences;


        dataRow.PurchaseRequisitionID = null;
        dataRow.PurchaseRequisitionDetailID = null;
        dataRow.PurchaseRequisitionCode = null;
        dataRow.PurchaseRequisitionReference = null;
        dataRow.PurchaseRequisitionEntryDate = null;


        dataRow.WarehouseTransferID = null;
        dataRow.WarehouseTransferDetailID = null;
        dataRow.WarehouseTransferReference = null;
        dataRow.WarehouseTransferEntryDate = null;
        dataRow.GoodsReceiptReference = null;
        dataRow.GoodsReceiptEntryDate = null;
        dataRow.BatchEntryDate = plannedOrderGridDataItem.FinishedProductEntryDate;


        dataRow.CommodityID = plannedOrderGridDataItem.CommodityID;
        dataRow.CommodityName = plannedOrderGridDataItem.CommodityName;
        dataRow.CommodityCode = plannedOrderGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = plannedOrderGridDataItem.CommodityTypeID;


        dataRow.QuantityRemains = plannedOrderGridDataItem.QuantityRemains;
        dataRow.Quantity = plannedOrderGridDataItem.Quantity;

        dataRow.Remarks = null;


        goodsReceiptJSON.push(dataRow);
    }
}

