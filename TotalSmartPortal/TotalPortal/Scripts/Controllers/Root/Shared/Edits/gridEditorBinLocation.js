define(["superBase", "gridEditorTemplate"], (function (superBase, gridEditorTemplate) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridEditorTemplate);


    //The BinLocation here is AutoComplete Widget
    definedExemplar.prototype.handleSelect = function (e) {
        var currentDataSourceRow = this._getCurrentDataSourceRow();

        if (currentDataSourceRow != undefined) {
            var dataItem = e.sender.dataItem(e.item.index());

            currentDataSourceRow.set("BinLocationID", dataItem.BinLocationID);
            currentDataSourceRow.set("BinLocationCode", dataItem.Code);
        }

        window.binLocationCodeBeforeChange = dataItem.Code;
    };


    definedExemplar.prototype.handleChange = function (e) {
        this._setEditorValue("BinLocationCode", window.binLocationCodeBeforeChange);
    };




    return definedExemplar;
}));