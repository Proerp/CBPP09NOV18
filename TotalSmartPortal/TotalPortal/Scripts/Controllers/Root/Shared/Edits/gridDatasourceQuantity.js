define(["superBase", "gridDatasource"], (function (superBase, gridDatasource) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridDatasource);






    definedExemplar.prototype._removeTotalToModelProperty = function (dataRow) {
        this._updateTotalToModelProperty("TotalQuantity", "Quantity", "sum", requireConfig.websiteOptions.rndQuantity, false);

        if ($("#TotalQuantityPositive").val() != undefined) { this._updateTotalToModelProperty("TotalQuantityPositive", "QuantityPositive", "sum", requireConfig.websiteOptions.rndQuantity, false); }
        if ($("#TotalQuantityNegative").val() != undefined) { this._updateTotalToModelProperty("TotalQuantityNegative", "QuantityNegative", "sum", requireConfig.websiteOptions.rndQuantity, false); }

        definedExemplar._super._removeTotalToModelProperty.call(this, dataRow);
    }








    definedExemplar.prototype._changeQuantity = function (dataRow) {
        if (dataRow.QuantityPositive != undefined) { dataRow.set("QuantityPositive", dataRow.Quantity); }
        if (dataRow.QuantityNegative != undefined) { dataRow.set("QuantityNegative", this._round(0 - dataRow.Quantity, requireConfig.websiteOptions.rndQuantity)); }

        this._updateTotalToModelProperty("TotalQuantity", "Quantity", "sum", requireConfig.websiteOptions.rndQuantity);
    }

    definedExemplar.prototype._changeQuantityPositive = function (dataRow) {
        dataRow.set("Quantity", dataRow.QuantityPositive);
        dataRow.set("QuantityNegative", this._round(0 - dataRow.QuantityPositive, requireConfig.websiteOptions.rndQuantity));

        this._updateTotalToModelProperty("TotalQuantityPositive", "QuantityPositive", "sum", requireConfig.websiteOptions.rndQuantity);
    }

    definedExemplar.prototype._changeQuantityNegative = function (dataRow) {
        dataRow.set("Quantity", this._round(0 - dataRow.QuantityNegative, requireConfig.websiteOptions.rndQuantity));
        dataRow.set("QuantityPositive", dataRow.Quantity);

        this._updateTotalToModelProperty("TotalQuantityNegative", "QuantityNegative", "sum", requireConfig.websiteOptions.rndQuantity);
    }

    return definedExemplar;
}));