define(["superBase", "gridEditorTemplate"], (function (superBase, gridEditorTemplate) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridEditorTemplate);


    //The mold here is AutoComplete Widget
    definedExemplar.prototype.handleSelect = function (e) {
        var currentDataSourceRow = this._getCurrentDataSourceRow();

        if (currentDataSourceRow != undefined) {
            var dataItem = e.sender.dataItem(e.item.index());

            currentDataSourceRow.set("MoldID", dataItem.MoldID);
            currentDataSourceRow.set("MoldCode", dataItem.Code);

            currentDataSourceRow.set("MoldQuantity", dataItem.Quantity);
        }

        window.moldCodeBeforeChange = dataItem.Code;
    };


    definedExemplar.prototype.handleChange = function (e) {
        this._setEditorValue("MoldCode", window.moldCodeBeforeChange);
    };




    return definedExemplar;
}));