function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(productionOrderGridDataSource, pendingFirmOrderGridDataSource) {
    if (productionOrderGridDataSource != undefined && pendingFirmOrderGridDataSource != undefined) {
        var pendingPlannedOrderDetailGridDataItems = pendingFirmOrderGridDataSource.view();
        var productionOrderJSON = productionOrderGridDataSource.data().toJSON();
        for (var i = 0; i < pendingPlannedOrderDetailGridDataItems.length; i++) {
            if (pendingPlannedOrderDetailGridDataItems[i].IsSelected === true)
                _setParentInput(productionOrderJSON, pendingPlannedOrderDetailGridDataItems[i]);
        }
        
        productionOrderJSON.push(new Object()); //Add a temporary empty row

        productionOrderGridDataSource.data(productionOrderJSON);

        var rawData = productionOrderGridDataSource.data()
        productionOrderGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row

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


    function _setParentInput(productionOrderJSON, plannedOrderGridDataItem) {

        //var dataRow = productionOrderJSON.add({});

        var dataRow = new Object();

        dataRow.LocationID = null;
        dataRow.EntryDate = null;

        dataRow.ProductionOrderDetailID = 0;
        dataRow.ProductionOrderID = window.parent.$("#ProductionOrderID").val();

        dataRow.PlannedOrderID = plannedOrderGridDataItem.PlannedOrderID;
        dataRow.FirmOrderID = plannedOrderGridDataItem.FirmOrderID;
        dataRow.FirmOrderCode = plannedOrderGridDataItem.FirmOrderCode;
        dataRow.FirmOrderReference = plannedOrderGridDataItem.FirmOrderReference;
        dataRow.FirmOrderEntryDate = plannedOrderGridDataItem.FirmOrderEntryDate;
        dataRow.FirmOrderDeliveryDate = plannedOrderGridDataItem.FirmOrderDeliveryDate;

        dataRow.CustomerID = plannedOrderGridDataItem.CustomerID;
        dataRow.CustomerName = plannedOrderGridDataItem.CustomerName;
        dataRow.CustomerCode = plannedOrderGridDataItem.CustomerCode;

        dataRow.Specs = plannedOrderGridDataItem.Specs;
        dataRow.Specification = plannedOrderGridDataItem.Specification;

        dataRow.BomID = plannedOrderGridDataItem.BomID;
        dataRow.BomCode = plannedOrderGridDataItem.BomCode;

        dataRow.QuantityRemains = plannedOrderGridDataItem.QuantityRemains;
        dataRow.Quantity = 0;
        
        dataRow.ProductionLineID = 0;
        dataRow.ProductionLineCode = null;

        dataRow.Remarks = null;
        dataRow.VoidTypeID = null;
        dataRow.VoidTypeName = null;
        dataRow.InActive = false;
        dataRow.InActivePartial = false;
        dataRow.InActiveIssue = false;

        productionOrderJSON.push(dataRow);
    }
}

