define(["gridEditorBom"], (function (gridEditorBom) {

    gridEditorBomSelect = function (e) {
        var gridEditorBomInstance = new gridEditorBom("kendoGridDetails");
        gridEditorBomInstance.handleSelect(e);
    }

    gridEditorBomChange = function (e) {
        var gridEditorBomInstance = new gridEditorBom("kendoGridDetails");
        gridEditorBomInstance.handleChange(e);
    }

    gridEditorBomDataBound = function (e) {
        $(".k-animation-container:has(#BomCode-list)").css("width", "382");
        $("#BomCode-list").css("width", "382");
        //$("#BomCode-list").css("height", $("#BomCode-list").height() + 1);
    }

}));