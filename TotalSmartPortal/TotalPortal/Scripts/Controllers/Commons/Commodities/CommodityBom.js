function AddCommodityBom(bomID, commodityID) {
    if ($("#Editable").val() != 'True')
        return;

    return $.ajax({
        url: window.urlAddCommodityBom,
        data: JSON.stringify({ "bomID": bomID, "commodityID": commodityID }),

        type: 'POST',
        contentType: 'application/json;',
        dataType: 'json',
        success: function (result) {
            if (result.AddResult == "Successfully") {
                $("#gridBoms").data("kendoGrid").dataSource.read();
            }
            else
                alert(result.AddResult);
        },
        error: function (jqXHR, textStatus) {
            alert(textStatus);
        }
    });
}

function RemoveCommodityBom(commodityBomID) {
    if ($("#Editable").val() != 'True')
        return;
    return $.ajax({
        url: window.urlRemoveCommodityBom,
        data: JSON.stringify({ "commodityBomID": commodityBomID }),

        type: 'POST',
        contentType: 'application/json;',
        dataType: 'json',
        success: function (result) {
            if (result.RemoveResult == "Successfully") {
                $("#gridBoms").data("kendoGrid").dataSource.read();
            }
            else
                alert(result.RemoveResult);
        },
        error: function (jqXHR, textStatus) {
            alert(textStatus);
        }
    });
}

function SaveCommodityBom(e) {
    if (e.values.BlockUnit != undefined || e.values.BlockQuantity != undefined || e.values.Remarks != undefined)
        UpdateCommodityBom(e.model.CommodityBomID, e.model.CommodityID, e.values.BlockUnit != undefined ? e.values.BlockUnit : e.model.BlockUnit, e.values.BlockQuantity != undefined ? e.values.BlockQuantity : e.model.BlockQuantity, e.values.Remarks != undefined ? e.values.Remarks : e.model.Remarks, e.model.IsDefault)
}

$('#gridBoms').on('click', '.GridCheckbox', function () {
    var grid = $('#gridBoms').data().kendoGrid;
    var dataItem = grid.dataItem($(this).closest('tr'));

    var checked = $(this).is(':checked');

    UpdateCommodityBom(dataItem.CommodityBomID, dataItem.CommodityID, dataItem.BlockUnit, dataItem.BlockQuantity, dataItem.Remarks, (this.id === "IsDefault" ? checked : dataItem.IsDefault))
})

function UpdateCommodityBom(commodityBomID, commodityID, blockUnit, blockQuantity, remarks, isDefault) {
    if ($("#Editable").val() != 'True')
        return;

    return $.ajax({
        url: window.urlUpdateCommodityBom,
        data: JSON.stringify({ "commodityBomID": commodityBomID, "commodityID": commodityID, "blockUnit": blockUnit, "blockQuantity": blockQuantity, "remarks": remarks, "isDefault": isDefault }),

        type: 'POST',
        contentType: 'application/json;',
        dataType: 'json',
        success: function (result) {
            if (result.SetResult == "Successfully") {
                $("#gridBoms").data("kendoGrid").dataSource.read();
            }
            else
                alert(result.SetResult);
        },
        error: function (jqXHR, textStatus) {
            alert(textStatus);
        }
    });
}