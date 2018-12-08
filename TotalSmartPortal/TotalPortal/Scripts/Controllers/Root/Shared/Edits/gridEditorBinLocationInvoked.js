define(["gridEditorBinLocation"], (function (gridEditorBinLocation) {

    gridEditorBinLocationSelect = function (e) {
        var gridEditorBinLocationInstance = new gridEditorBinLocation("kendoGridDetails");
        gridEditorBinLocationInstance.handleSelect(e);
    }

    gridEditorBinLocationChange = function (e) {
        var gridEditorBinLocationInstance = new gridEditorBinLocation("kendoGridDetails");
        gridEditorBinLocationInstance.handleChange(e);
    }

    gridEditorBinLocationDataBound = function (e) {
        $(".k-animation-container:has(#BinLocationCode-list)").css("width", "382");
        $("#BinLocationCode-list").css("width", "135");
        //$("#BinLocationCode-list").css("height", $("#BinLocationCode-list").height() + 1);
    }

}));