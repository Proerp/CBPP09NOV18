define(["superBase", "gridDatasourceQuantity"], (function (superBase, gridDatasourceQuantity) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridDatasourceQuantity);






    definedExemplar.prototype._removeTotalToModelProperty = function (dataRow) {
        this._updateTotalToModelProperty("TotalPackages", "Packages", "sum", requireConfig.websiteOptions.rndQuantity, false);

        definedExemplar._super._removeTotalToModelProperty.call(this, dataRow);
    }








    definedExemplar.prototype._changeQuantity = function (dataRow) {
        this._updateRowWeight(dataRow);

        definedExemplar._super._changeQuantity.call(this, dataRow);
    }

    definedExemplar.prototype._changeUnitWeight = function (dataRow) {
        this._updateRowWeight(dataRow);
    }

    definedExemplar.prototype._changePackages = function (dataRow) {
        this._updateTotalToModelProperty("TotalPackages", "Packages", "sum", requireConfig.websiteOptions.rndQuantity);
    }





    definedExemplar.prototype._updateRowWeight = function (dataRow) {
        dataRow.set("Packages", dataRow.UnitWeight != 0 ? this._round(dataRow.Quantity / dataRow.UnitWeight, requireConfig.websiteOptions.rndQuantity) : 0);
    }


    return definedExemplar;
}));