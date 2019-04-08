function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(transferOrderGridDataSource, pendingGridDataSource) {
    if (transferOrderGridDataSource != undefined && pendingGridDataSource != undefined) {
        var pendingGridDataItems = pendingGridDataSource.view();
        var transferOrderJSON = transferOrderGridDataSource.data().toJSON();
        for (var i = 0; i < pendingGridDataItems.length; i++) {
            if (pendingGridDataItems[i].IsSelected === true)
                _setParentInput(transferOrderJSON, pendingGridDataItems[i]);
        }

        transferOrderJSON.push(new Object()); //Add a temporary empty row

        transferOrderGridDataSource.data(transferOrderJSON);

        var rawData = transferOrderGridDataSource.data()
        transferOrderGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row

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


    function _setParentInput(transferOrderJSON, pendingGridDataItem) {

        //var dataRow = transferOrderJSON.add({});

        var dataRow = new Object();

        dataRow.TransferOrderDetailID = 0;
        dataRow.TransferOrderID = window.parent.$("#TransferOrderID").val();

        dataRow.CommodityID = pendingGridDataItem.CommodityID;
        dataRow.CommodityName = pendingGridDataItem.CommodityName;
        dataRow.CommodityCode = pendingGridDataItem.CommodityCode;
        dataRow.CommodityTypeID = pendingGridDataItem.CommodityTypeID;
        dataRow.Weight = pendingGridDataItem.Weight;

        dataRow.QuantityAvailables = pendingGridDataItem.QuantityAvailables;
        dataRow.QuantityAvailableReceipts = pendingGridDataItem.QuantityAvailableReceipts;
        dataRow.Quantity = DoRound(pendingGridDataItem.QuantityRemains - pendingGridDataItem.QuantityTransferOrders - pendingGridDataItem.QuantityAvailableReceipts, window.parent.requireConfig.websiteOptions.rndQuantity);

        dataRow.QuantityIssued = 0;
        dataRow.QuantityRemains = dataRow.Quantity;        
        dataRow.Packages = dataRow.Weight * 1 > 0 ? DoRound(dataRow.QuantityRemains / dataRow.Weight, window.parent.requireConfig.websiteOptions.rndQuantity) : dataRow.QuantityRemains;

        dataRow.InActivePartial = false;
        dataRow.VoidTypeName = null;
        dataRow.Remarks = pendingGridDataItem.Specs;


        transferOrderJSON.push(dataRow);
    }
}

