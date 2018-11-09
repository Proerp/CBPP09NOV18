define(["superBase", "gridEditorTemplate"], (function (superBase, gridEditorTemplate) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridEditorTemplate);


    //The productionLine here is AutoComplete Widget
    definedExemplar.prototype.handleSelect = function (e) {
        var currentDataSourceRow = this._getCurrentDataSourceRow();

        if (currentDataSourceRow != undefined) {
            var dataItem = e.sender.dataItem(e.item.index());

            currentDataSourceRow.set("ProductionLineID", dataItem.ProductionLineID);
            currentDataSourceRow.set("ProductionLineCode", dataItem.Code);
        }

        window.productionLineCodeBeforeChange = dataItem.Code;
    };


    definedExemplar.prototype.handleChange = function (e) {
        this._setEditorValue("ProductionLineCode", window.productionLineCodeBeforeChange);
    };




    return definedExemplar;
}));