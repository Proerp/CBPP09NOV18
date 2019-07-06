function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(goodsIssueGridDataSource, pendingDeliveryAdviceDetailGridDataSource) {
    if (goodsIssueGridDataSource != undefined && pendingDeliveryAdviceDetailGridDataSource != undefined) {

        var totalQuantityRemains = $("#TotalQuantityRemains").data('kendoNumericTextBox').value();

        var pendingDeliveryAdviceDetailGridDataItems = pendingDeliveryAdviceDetailGridDataSource.view();
        var goodsIssueJSON = goodsIssueGridDataSource.data().toJSON();
        for (var i = 0; i < pendingDeliveryAdviceDetailGridDataItems.length; i++) {
            if (pendingDeliveryAdviceDetailGridDataItems[i].IsSelected === true && pendingDeliveryAdviceDetailGridDataItems[i].GoodsReceiptDetailID != null)
                _setParentInput(goodsIssueJSON, pendingDeliveryAdviceDetailGridDataItems[i]);
        }

        goodsIssueJSON.push(new Object()); //Add a temporary empty row

        goodsIssueGridDataSource.data(goodsIssueJSON);

        var rawData = goodsIssueGridDataSource.data()
        goodsIssueGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row

        window.parent.updateQuantities($("#DeliveryAdviceDetailID").val());
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


    function _setParentInput(goodsIssueJSON, productionOrderGridDataItem) {

        //var dataRow = goodsIssueJSON.add({});

        var dataRow = new Object();

        dataRow.LocationID = null;
        dataRow.EntryDate = null;

        dataRow.GoodsIssuePackageID = 0;
        dataRow.GoodsIssueID = window.parent.$("#GoodsIssueID").val();

        dataRow.DeliveryAdviceID = productionOrderGridDataItem.DeliveryAdviceID;
        dataRow.DeliveryAdviceDetailID = productionOrderGridDataItem.DeliveryAdviceDetailID;

        dataRow.CommodityID = productionOrderGridDataItem.CommodityID;
        dataRow.CommodityName = productionOrderGridDataItem.CommodityName;
        dataRow.CommodityCode = productionOrderGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = productionOrderGridDataItem.CommodityTypeID;

        dataRow.GoodsReceiptID = productionOrderGridDataItem.GoodsReceiptID;
        dataRow.GoodsReceiptDetailID = productionOrderGridDataItem.GoodsReceiptDetailID;
        dataRow.GoodsReceiptCode = productionOrderGridDataItem.GoodsReceiptCode;
        dataRow.GoodsReceiptReference = productionOrderGridDataItem.GoodsReceiptReference;
        dataRow.GoodsReceiptEntryDate = productionOrderGridDataItem.GoodsReceiptEntryDate;
        dataRow.ExpiryDate = productionOrderGridDataItem.ExpiryDate;

        dataRow.WarehouseID = productionOrderGridDataItem.WarehouseID;

        dataRow.Barcode = productionOrderGridDataItem.Barcode;
        dataRow.BatchCode = productionOrderGridDataItem.BatchCode;
        dataRow.SealCode = productionOrderGridDataItem.SealCode;
        dataRow.LabCode = productionOrderGridDataItem.LabCode;

        dataRow.BinLocationID = productionOrderGridDataItem.BinLocationID;
        dataRow.BinLocationCode = productionOrderGridDataItem.BinLocationCode;

        dataRow.QuantityAvailables = productionOrderGridDataItem.QuantityAvailables;
        dataRow.QuantityRemains = productionOrderGridDataItem.QuantityRemains;
        dataRow.Quantity = DoRound(totalQuantityRemains > productionOrderGridDataItem.QuantityAvailables ? productionOrderGridDataItem.QuantityAvailables : (totalQuantityRemains > 0 ? totalQuantityRemains : 0), window.parent.requireConfig.websiteOptions.rndQuantity);
        totalQuantityRemains = DoRound(totalQuantityRemains - dataRow.Quantity, window.parent.requireConfig.websiteOptions.rndQuantity);

        dataRow.BatchID = productionOrderGridDataItem.BatchID;
        dataRow.BatchEntryDate = productionOrderGridDataItem.BatchEntryDate;

        dataRow.Remarks = null;


        goodsIssueJSON.push(dataRow);
    }
}

