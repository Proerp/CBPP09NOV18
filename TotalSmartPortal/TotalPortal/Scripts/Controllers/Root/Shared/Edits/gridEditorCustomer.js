define(["superBase", "gridEditorTemplate"], (function (superBase, gridEditorTemplate) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridEditorTemplate);


    //The customer here is AutoComplete Widget
    definedExemplar.prototype.handleSelect = function (e) {
        var currentDataSourceRow = this._getCurrentDataSourceRow();

        if (currentDataSourceRow != undefined) {
            var dataItem = e.sender.dataItem(e.item.index());

            currentDataSourceRow.set("CustomerID", dataItem.CustomerID);
            currentDataSourceRow.set("CustomerCode", dataItem.Code);
            currentDataSourceRow.set("CustomerName", dataItem.Name);
        }

        window.customerCodeBeforeChange = dataItem.Code;
    };


    definedExemplar.prototype.handleChange = function (e) {
        this._setEditorValue("CustomerCode", window.customerCodeBeforeChange);
    };




    return definedExemplar;
}));