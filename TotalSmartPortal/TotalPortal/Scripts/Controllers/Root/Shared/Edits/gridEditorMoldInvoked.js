define(["gridEditorMold"], (function (gridEditorMold) {

    gridEditorMoldSelect = function (e) {
        var gridEditorMoldInstance = new gridEditorMold("kendoGridDetails");
        gridEditorMoldInstance.handleSelect(e);
    }

    gridEditorMoldChange = function (e) {
        var gridEditorMoldInstance = new gridEditorMold("kendoGridDetails");
        gridEditorMoldInstance.handleChange(e);
    }

    gridEditorMoldDataBound = function (e) {
        $(".k-animation-container:has(#MoldCode-list)").css("width", "382");
        $("#MoldCode-list").css("width", "382");
        //$("#MoldCode-list").css("height", $("#MoldCode-list").height() + 1);
    }

}));