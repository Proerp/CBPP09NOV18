﻿function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(goodsReceiptGridDataSource, pendingGoodsArrivalDetailGridDataSource) {
    if (goodsReceiptGridDataSource != undefined && pendingGoodsArrivalDetailGridDataSource != undefined) {
        var pendingGoodsArrivalDetailGridDataItems = pendingGoodsArrivalDetailGridDataSource.view();
        var goodsReceiptJSON = goodsReceiptGridDataSource.data().toJSON();
        for (var i = 0; i < pendingGoodsArrivalDetailGridDataItems.length; i++) {
            if (pendingGoodsArrivalDetailGridDataItems[i].IsSelected === true)
                _setParentInput(goodsReceiptJSON, pendingGoodsArrivalDetailGridDataItems[i]);
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


    function _setParentInput(goodsReceiptJSON, goodsArrivalGridDataItem) {

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


        dataRow.FinishedProductID = null;
        dataRow.FinishedProductPackageID = null;
        dataRow.FinishedProductEntryDate = null;
        dataRow.FirmOrderReference = null;
        dataRow.FirmOrderCode = null;
        dataRow.FirmOrderSpecs = null;
        dataRow.SemifinishedProductReferences = null;


        dataRow.PurchaseRequisitionID = null;
        dataRow.PurchaseRequisitionDetailID = null;
        dataRow.PurchaseRequisitionCode = null;
        dataRow.PurchaseRequisitionReference = null;
        dataRow.PurchaseRequisitionEntryDate = null;


        dataRow.GoodsArrivalID = goodsArrivalGridDataItem.GoodsArrivalID;
        dataRow.GoodsArrivalDetailID = goodsArrivalGridDataItem.GoodsArrivalDetailID;
        dataRow.GoodsArrivalCode = goodsArrivalGridDataItem.GoodsArrivalCode;
        dataRow.GoodsArrivalReference = goodsArrivalGridDataItem.GoodsArrivalReference;
        dataRow.GoodsArrivalEntryDate = goodsArrivalGridDataItem.GoodsArrivalEntryDate;


        dataRow.WarehouseTransferID = null;
        dataRow.WarehouseTransferDetailID = null;
        dataRow.WarehouseTransferReference = null;
        dataRow.WarehouseTransferEntryDate = null;
        dataRow.GoodsReceiptReference = null;
        dataRow.GoodsReceiptEntryDate = null;
        dataRow.BatchEntryDate = goodsArrivalGridDataItem.BatchEntryDate;


        dataRow.CommodityID = goodsArrivalGridDataItem.CommodityID;
        dataRow.CommodityName = goodsArrivalGridDataItem.CommodityName;
        dataRow.CommodityCode = goodsArrivalGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = goodsArrivalGridDataItem.CommodityTypeID;
        dataRow.Barcode = goodsArrivalGridDataItem.Barcode;
        dataRow.BatchCode = goodsArrivalGridDataItem.BatchCode;
        dataRow.SealCode = goodsArrivalGridDataItem.SealCode;
        dataRow.LabCode = goodsArrivalGridDataItem.LabCode;

        dataRow.BinLocationID = null;
        dataRow.BinLocationCode = null;

        dataRow.QuantityRemains = goodsArrivalGridDataItem.QuantityRemains;
        dataRow.Quantity = goodsArrivalGridDataItem.Quantity;

        dataRow.Remarks = null;


        goodsReceiptJSON.push(dataRow);
    }
}

