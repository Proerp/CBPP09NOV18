define(["gridEditorProductionLine"], (function (gridEditorProductionLine) {

    gridEditorProductionLineSelect = function (e) {
        var gridEditorProductionLineInstance = new gridEditorProductionLine("kendoGridDetails");
        gridEditorProductionLineInstance.handleSelect(e);
    }

    gridEditorProductionLineChange = function (e) {
        var gridEditorProductionLineInstance = new gridEditorProductionLine("kendoGridDetails");
        gridEditorProductionLineInstance.handleChange(e);
    }

    gridEditorProductionLineDataBound = function (e) {
        $(".k-animation-container:has(#ProductionLineCode-list)").css("width", "382");
        $("#ProductionLineCode-list").css("width", "382");
        //$("#ProductionLineCode-list").css("height", $("#ProductionLineCode-list").height() + 1);
    }

}));