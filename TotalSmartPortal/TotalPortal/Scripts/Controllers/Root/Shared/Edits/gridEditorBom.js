define(["superBase", "gridEditorTemplate"], (function (superBase, gridEditorTemplate) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridEditorTemplate);


    //The Bom here is AutoComplete Widget
    definedExemplar.prototype.handleSelect = function (e) {
        var currentDataSourceRow = this._getCurrentDataSourceRow();

        if (currentDataSourceRow != undefined) {
            var dataItem = e.sender.dataItem(e.item.index());

            currentDataSourceRow.set("BomID", dataItem.BomID);
            currentDataSourceRow.set("BomCode", dataItem.Code);

            currentDataSourceRow.set("BlockUnit", dataItem.BlockUnit);
            currentDataSourceRow.set("BlockQuantity", dataItem.BlockQuantity);
        }

        window.bomCodeBeforeChange = dataItem.Code;
    };


    definedExemplar.prototype.handleChange = function (e) {
        this._setEditorValue("BomCode", window.bomCodeBeforeChange);
    };




    return definedExemplar;
}));