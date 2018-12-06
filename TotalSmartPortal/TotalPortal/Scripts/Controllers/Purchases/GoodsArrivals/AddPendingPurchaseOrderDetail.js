function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(goodsArrivalGridDataSource, pendingPurchaseOrderDetailGridDataSource) {
    if (goodsArrivalGridDataSource != undefined && pendingPurchaseOrderDetailGridDataSource != undefined) {
        var pendingPurchaseOrderDetailGridDataItems = pendingPurchaseOrderDetailGridDataSource.view();
        var goodsArrivalJSON = goodsArrivalGridDataSource.data().toJSON();
        for (var i = 0; i < pendingPurchaseOrderDetailGridDataItems.length; i++) {
            if (pendingPurchaseOrderDetailGridDataItems[i].IsSelected === true)
                _setParentInput(goodsArrivalJSON, pendingPurchaseOrderDetailGridDataItems[i]);
        }

        goodsArrivalJSON.push(new Object()); //Add a temporary empty row

        goodsArrivalGridDataSource.data(goodsArrivalJSON);

        var rawData = goodsArrivalGridDataSource.data()
        goodsArrivalGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row

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


    function _setParentInput(goodsArrivalJSON, purchaseOrderGridDataItem) {

        //var dataRow = goodsArrivalJSON.add({});

        var dataRow = new Object();

        dataRow.LocationID = null;
        dataRow.EntryDate = null;

        dataRow.GoodsArrivalDetailID = 0;
        dataRow.GoodsArrivalID = window.parent.$("#GoodsArrivalID").val();
        
        dataRow.PurchaseOrderID = purchaseOrderGridDataItem.PurchaseOrderID;
        dataRow.PurchaseOrderDetailID = purchaseOrderGridDataItem.PurchaseOrderDetailID;
        dataRow.PurchaseOrderCode = purchaseOrderGridDataItem.PurchaseOrderCode;
        dataRow.PurchaseOrderReference = purchaseOrderGridDataItem.PurchaseOrderReference;
        dataRow.PurchaseOrderEntryDate = purchaseOrderGridDataItem.PurchaseOrderEntryDate;

        dataRow.CommodityID = purchaseOrderGridDataItem.CommodityID;
        dataRow.CommodityName = purchaseOrderGridDataItem.CommodityName;
        dataRow.CommodityCode = purchaseOrderGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = purchaseOrderGridDataItem.CommodityTypeID;
        dataRow.Shelflife = purchaseOrderGridDataItem.Shelflife;

        dataRow.QuantityAvailable = purchaseOrderGridDataItem.QuantityAvailable;
        dataRow.ControlFreeQuantity = purchaseOrderGridDataItem.ControlFreeQuantity;
        dataRow.QuantityRemains = purchaseOrderGridDataItem.QuantityRemains;
        dataRow.Quantity = purchaseOrderGridDataItem.Quantity;

        dataRow.UnitWeight = 1;
        dataRow.Packages = 0;

        dataRow.SealCode = null;
        dataRow.BatchCode = null;
        dataRow.LabCode = null;

        dataRow.ProductionDate = null;
        dataRow.ExpiryDate = null;

        dataRow.Remarks = null;

        goodsArrivalJSON.push(dataRow);
    }
}