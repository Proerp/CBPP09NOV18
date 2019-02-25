colorLayerCode = function (e) {
    var grid = $("#kendoGridDetails").data("kendoGrid");
    var data = grid.dataSource.data();
    $.each(data, function (i, row) {
        var layerCode = row.LayerCode.toUpperCase();
        if (layerCode != null) {
            if ($('tr[data-uid="' + row.uid + '"]').hasClass("Color-A")) $('tr[data-uid="' + row.uid + '"]').removeClass("Color-A");
            if ($('tr[data-uid="' + row.uid + '"]').hasClass("Color-B")) $('tr[data-uid="' + row.uid + '"]').removeClass("Color-B");
            if ($('tr[data-uid="' + row.uid + '"]').hasClass("Color-C")) $('tr[data-uid="' + row.uid + '"]').removeClass("Color-C");

            $('tr[data-uid="' + row.uid + '"]').addClass((layerCode == 'B' ? "Color-B" : (layerCode == 'C' ? "Color-C" : "Color-A")));
        }
    });
}