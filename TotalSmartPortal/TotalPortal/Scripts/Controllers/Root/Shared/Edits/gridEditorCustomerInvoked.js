define(["gridEditorCustomer"], (function (gridEditorCustomer) {

    gridEditorCustomerSelect = function (e) {
        var gridEditorCustomerInstance = new gridEditorCustomer("kendoGridDetails");
        gridEditorCustomerInstance.handleSelect(e);
    }

    gridEditorCustomerChange = function (e) {
        var gridEditorCustomerInstance = new gridEditorCustomer("kendoGridDetails");
        gridEditorCustomerInstance.handleChange(e);
    }

    gridEditorCustomerDataBound = function (e) {
        $(".k-animation-container:has(#CustomerCode-list)").css("width", "382");
        $("#CustomerCode-list").css("width", "382");
        //$("#CustomerCode-list").css("height", $("#CustomerCode-list").height() + 1);
    }

}));