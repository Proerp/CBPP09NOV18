function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(warehouseTransferGridDataSource, goodsReceiptDetailAvailableGridDataSource, closeWhenFinished) {
    if (warehouseTransferGridDataSource != undefined && goodsReceiptDetailAvailableGridDataSource != undefined) {
        var goodsReceiptDetailAvailableGridDataItems = goodsReceiptDetailAvailableGridDataSource.view();
        var warehouseTransferJSON = warehouseTransferGridDataSource.data().toJSON();
        for (var i = 0; i < goodsReceiptDetailAvailableGridDataItems.length; i++) {
            if (goodsReceiptDetailAvailableGridDataItems[i].IsSelected === true)
                _setParentInput(warehouseTransferJSON, goodsReceiptDetailAvailableGridDataItems[i]);
        }

        warehouseTransferJSON.push(new Object()); //Add a temporary empty row

        warehouseTransferGridDataSource.data(warehouseTransferJSON);

        var rawData = warehouseTransferGridDataSource.data()
        warehouseTransferGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row        

        if (closeWhenFinished)
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


    function _setParentInput(warehouseTransferJSON, warehouseTransferGridDataItem) {

        //var dataRow = warehouseTransferJSON.add({});

        var dataRow = new Object();

        dataRow.LocationID = null;
        dataRow.EntryDate = null;

        dataRow.WarehouseTransferDetailID = 0;
        dataRow.WarehouseTransferID = window.parent.$("#WarehouseTransferID").val();

        dataRow.CommodityID = warehouseTransferGridDataItem.CommodityID;
        dataRow.CommodityName = warehouseTransferGridDataItem.CommodityName;
        dataRow.CommodityCode = warehouseTransferGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = warehouseTransferGridDataItem.CommodityTypeID;

        dataRow.TransferOrderID = warehouseTransferGridDataItem.TransferOrderID === undefined ? null : warehouseTransferGridDataItem.TransferOrderID;
        dataRow.TransferOrderDetailID = warehouseTransferGridDataItem.TransferOrderDetailID === undefined ? null : warehouseTransferGridDataItem.TransferOrderDetailID;

        dataRow.GoodsReceiptID = warehouseTransferGridDataItem.GoodsReceiptID;
        dataRow.GoodsReceiptDetailID = warehouseTransferGridDataItem.GoodsReceiptDetailID;
        dataRow.GoodsReceiptCode = warehouseTransferGridDataItem.GoodsReceiptCode;
        dataRow.GoodsReceiptReference = warehouseTransferGridDataItem.GoodsReceiptReference;
        dataRow.GoodsReceiptEntryDate = warehouseTransferGridDataItem.GoodsReceiptEntryDate;

        dataRow.BatchID = warehouseTransferGridDataItem.BatchID;
        dataRow.BatchEntryDate = warehouseTransferGridDataItem.BatchEntryDate;

        dataRow.WarehouseID = warehouseTransferGridDataItem.WarehouseID;
        dataRow.WarehouseCode = warehouseTransferGridDataItem.WarehouseCode;

        dataRow.WarehouseReceiptID = warehouseTransferGridDataItem.WarehouseReceiptID;
        dataRow.WarehouseReceiptCode = warehouseTransferGridDataItem.WarehouseReceiptCode;

        dataRow.TransferOrderRemains = warehouseTransferGridDataItem.TransferOrderRemains === undefined ? 0 : warehouseTransferGridDataItem.TransferOrderRemains;
        dataRow.QuantityRemains = warehouseTransferGridDataItem.QuantityRemains;
        dataRow.QuantityAvailables = warehouseTransferGridDataItem.QuantityAvailables;
        dataRow.Quantity = warehouseTransferGridDataItem.QuantityAvailables; //INIT BY THE WHOLE QuantityAvailables

        dataRow.Remarks = warehouseTransferGridDataItem.Remarks;

        warehouseTransferJSON.push(dataRow);
    }
}

