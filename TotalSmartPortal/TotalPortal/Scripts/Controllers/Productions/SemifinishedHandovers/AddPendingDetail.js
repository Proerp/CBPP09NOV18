function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(semifinishedHandoverGridDataSource, pendingDetailGridDataSource) {
    if (semifinishedHandoverGridDataSource != undefined && pendingDetailGridDataSource != undefined) {
        var pendingDetailGridDataItems = pendingDetailGridDataSource.view();        

        var semifinishedHandoverJSON = semifinishedHandoverGridDataSource.data().toJSON();
        for (var i = 0; i < pendingDetailGridDataItems.length; i++) {
            if (pendingDetailGridDataItems[i].IsSelected === true)
                _setParentInput(semifinishedHandoverJSON, pendingDetailGridDataItems[i]);
        }
        
        semifinishedHandoverJSON.push(new Object()); //Add a temporary empty row

        semifinishedHandoverGridDataSource.data(semifinishedHandoverJSON);

        var rawData = semifinishedHandoverGridDataSource.data()
        semifinishedHandoverGridDataSource.remove(rawData[rawData.length - 1]); //Remove the last row: this is the temporary empty row

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


    function _setParentInput(semifinishedHandoverJSON, semifinishedHandoverGridDataItem) {
        //var dataRow = semifinishedHandoverJSON.add({});        
        var dataRow = new Object();

        dataRow.LocationID = null;
        dataRow.EntryDate = null;

        dataRow.SemifinishedHandoverID = window.parent.$("#SemifinishedHandoverID").val();
        dataRow.SemifinishedHandoverDetailID = 0;

        dataRow.SemifinishedProductID = semifinishedHandoverGridDataItem.SemifinishedProductID;
        dataRow.SemifinishedProductReference = semifinishedHandoverGridDataItem.SemifinishedProductReference;
        dataRow.SemifinishedProductEntryDate = semifinishedHandoverGridDataItem.SemifinishedProductEntryDate;

        dataRow.CustomerID = semifinishedHandoverGridDataItem.CustomerID;
        dataRow.CustomerName = semifinishedHandoverGridDataItem.CustomerName;
        dataRow.CustomerCode = semifinishedHandoverGridDataItem.CustomerCode;

        dataRow.ProductionLineID = semifinishedHandoverGridDataItem.ProductionLineID;
        dataRow.ProductionLineCode = semifinishedHandoverGridDataItem.ProductionLineCode;

        dataRow.CrucialWorkerID = semifinishedHandoverGridDataItem.CrucialWorkerID;
        dataRow.CrucialWorkerName = semifinishedHandoverGridDataItem.CrucialWorkerName;

        dataRow.Quantity = semifinishedHandoverGridDataItem.Quantity;

        dataRow.Remarks = null;        

        semifinishedHandoverJSON.push(dataRow);
    }
}

