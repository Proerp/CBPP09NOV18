function kendGrid_ErrorHandler(e) {
    if (e.errors) {
        var message = "Errors:\n";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });
        alert(message);
    }
}


function CreatePopUp(popuptitle, widthsize, heightsize, popupWindow) {
    popupWindow.append("<div id='window'></div>");
    var mywindow = $("#window")
    .kendoWindow({
        width: widthsize,
        height: heightsize,
        actions: ["Pin", "Close"],
        title: popuptitle,
        //draggable: false,
        resizable: false,
        modal: true,
        iframe: true,
        pinned: true,
        //open: function(){
        //    this.wrapper.css({ top: 20 });
        //},
        deactivate: function () {
            this.destroy();
            this.close();
        }
    }).data("kendoWindow");
    return mywindow;
}



function keydown_insert(columnEdit) {
    $("#kendoGridDetails").children(".k-grid-toolbar").children(".k-grid-add").trigger("click");
    var grid = $("#kendoGridDetails").data("kendoGrid");
    var row = grid.tbody.find('tr').last();
    grid.select(row);

    var cells = $(row).find("td");

    var columns = grid.columns;
    for (var i = 0; i < columns.length; i++) {
        if (columns[i].title == columnEdit) {
            grid.editCell(cells[i]);
        }
    }
}



function cloneSelectedItem(kendoGridName, selectedItem) { //https://www.telerik.com/forums/copy-row-in-the-grid
    var kenGrid = $(kendoGridName != undefined ? kendoGridName : "#kendoGridDetails").data("kendoGrid");
    var baseItem; if (selectedItem != undefined) baseItem = selectedItem; else baseItem = kenGrid.dataItem(kenGrid.select());

    if (baseItem != undefined) {
        var clonedItem = baseItem.toJSON();

        tailorClonedItem(clonedItem); //MUST ADD [function tailorClonedItem] SPECIFIC FOR EACH MODULE WHEN USE THIS function cloneSelectedItem(). SEE _GoodsArrival.cshtml FOR AN EXAMPLE

        var idx = kenGrid.dataSource.indexOf(baseItem); // Get the index in the DataSource (not in current page of the grid) https://stackoverflow.com/questions/24931390/kendo-ui-grid-inline-insert-new-row-at-a-specific-position-on-the-grid
        kenGrid.dataSource.insert(idx + 1, clonedItem);
    }
}





function DoRound(value, decimals) {
    if (arguments.length < 2 || decimals === undefined || decimals === 0)
        return Math.round(value);
    else
        return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
}