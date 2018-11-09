function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(warehouseAdjustmentGridDataSource, goodsReceiptDetailAvailableGridDataSource, closeWhenFinished) {
    if (warehouseAdjustmentGridDataSource != undefined && goodsReceiptDetailAvailableGridDataSource != undefined) {
        var goodsReceiptDetailAvailableGridDataItems = goodsReceiptDetailAvailableGridDataSource.view();
        var warehouseAdjustmentJSON = warehouseAdjustmentGridDataSource.data().toJSON();
        for (var i = 0; i < goodsReceiptDetailAvailableGridDataItems.length; i++) {
            if (goodsReceiptDetailAvailableGridDataItems[i].IsSelected === true)
                _setParentInput(warehouseAdjustmentJSON, goodsReceiptDetailAvailableGridDataItems[i]);
        }

        warehouseAdjustmentJSON.push(new Object()); //Add a temporary empty row

        warehouseAdjustmentGridDataSource.data(warehouseAdjustmentJSON);

        var rawData = warehouseAdjustmentGridDataSource.data()
        warehouseAdjustmentGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row        

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


    function _setParentInput(warehouseAdjustmentJSON, productionOrderGridDataItem) {

        //var dataRow = warehouseAdjustmentJSON.add({});

        var dataRow = new Object();

        dataRow.LocationID = null;
        dataRow.EntryDate = null;

        dataRow.WarehouseAdjustmentDetailID = 0;
        dataRow.WarehouseAdjustmentID = window.parent.$("#WarehouseAdjustmentID").val();

        dataRow.CommodityID = productionOrderGridDataItem.CommodityID;
        dataRow.CommodityName = productionOrderGridDataItem.CommodityName;
        dataRow.CommodityCode = productionOrderGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = productionOrderGridDataItem.CommodityTypeID;

        dataRow.GoodsReceiptID = productionOrderGridDataItem.GoodsReceiptID;
        dataRow.GoodsReceiptDetailID = productionOrderGridDataItem.GoodsReceiptDetailID;
        dataRow.GoodsReceiptCode = productionOrderGridDataItem.GoodsReceiptCode;
        dataRow.GoodsReceiptReference = productionOrderGridDataItem.GoodsReceiptReference;
        dataRow.GoodsReceiptEntryDate = productionOrderGridDataItem.GoodsReceiptEntryDate;

        dataRow.BatchID = productionOrderGridDataItem.BatchID;
        dataRow.BatchEntryDate = productionOrderGridDataItem.BatchEntryDate;

        dataRow.WarehouseID = productionOrderGridDataItem.WarehouseID;
        dataRow.WarehouseCode = productionOrderGridDataItem.WarehouseCode;

        dataRow.QuantityAvailables = productionOrderGridDataItem.QuantityAvailables;
        dataRow.Quantity = 0;
        dataRow.QuantityPositive = 0;
        dataRow.QuantityNegative = 0;

        dataRow.Remarks = null;


        warehouseAdjustmentJSON.push(dataRow);
    }
}

