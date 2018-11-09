function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(goodsReceiptGridDataSource, pendingMaterialIssueDetailGridDataSource) {
    if (goodsReceiptGridDataSource != undefined && pendingMaterialIssueDetailGridDataSource != undefined) {
        var pendingPurchaseRequisitionDetailGridDataItems = pendingMaterialIssueDetailGridDataSource.view();
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


    function _setParentInput(goodsReceiptJSON, materialIssueGridDataItem) {

        //var dataRow = goodsReceiptJSON.add({});

        var dataRow = new Object();

        dataRow.LocationID = null;
        dataRow.EntryDate = null;

        dataRow.GoodsReceiptDetailID = 0;
        dataRow.GoodsReceiptID = window.parent.$("#GoodsReceiptID").val();


        dataRow.MaterialIssueID = materialIssueGridDataItem.MaterialIssueID;
        dataRow.MaterialIssueDetailID = materialIssueGridDataItem.MaterialIssueDetailID;
        dataRow.MaterialIssueEntryDate = materialIssueGridDataItem.MaterialIssueEntryDate;
        dataRow.ProductionLinesCode = materialIssueGridDataItem.ProductionLinesCode;

        dataRow.WorkshiftName = materialIssueGridDataItem.WorkshiftName;
        dataRow.WorkshiftEntryDate = materialIssueGridDataItem.WorkshiftEntryDate;


        dataRow.FinishedProductID = null;
        dataRow.FinishedProductPackageID = null;
        dataRow.FinishedProductEntryDate = null;
        dataRow.FirmOrderReference = materialIssueGridDataItem.FirmOrderReference;
        dataRow.FirmOrderCode = materialIssueGridDataItem.FirmOrderCode;
        dataRow.FirmOrderSpecs = materialIssueGridDataItem.FirmOrderSpecs;
        dataRow.SemifinishedProductReferences = null;


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
        dataRow.BatchEntryDate = materialIssueGridDataItem.BatchEntryDate;


        dataRow.CommodityID = materialIssueGridDataItem.CommodityID;
        dataRow.CommodityName = materialIssueGridDataItem.CommodityName;
        dataRow.CommodityCode = materialIssueGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = materialIssueGridDataItem.CommodityTypeID;


        dataRow.QuantityRemains = materialIssueGridDataItem.QuantityRemains;
        dataRow.Quantity = materialIssueGridDataItem.Quantity;

        dataRow.Remarks = null;


        goodsReceiptJSON.push(dataRow);
    }
}

