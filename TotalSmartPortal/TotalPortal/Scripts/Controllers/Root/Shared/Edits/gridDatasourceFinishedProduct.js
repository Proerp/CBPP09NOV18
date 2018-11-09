define(["superBase", "gridDatasourceQuantity"], (function (superBase, gridDatasourceQuantity) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridDatasourceQuantity);






    definedExemplar.prototype._removeTotalToModelProperty = function (dataRow) {
        this._updateTotalToModelProperty("TotalQuantityFailure", "QuantityFailure", "sum", requireConfig.websiteOptions.rndQuantity, false);
        this._updateTotalToModelProperty("TotalQuantityExcess", "QuantityExcess", "sum", requireConfig.websiteOptions.rndQuantity, false);
        this._updateTotalToModelProperty("TotalQuantityShortage", "QuantityShortage", "sum", requireConfig.websiteOptions.rndQuantity, false);
        this._updateTotalToModelProperty("TotalSwarfs", "Swarfs", "sum", requireConfig.websiteOptions.rndQuantity, false);

        definedExemplar._super._removeTotalToModelProperty.call(this, dataRow);
    }


    definedExemplar.prototype._changeQuantityFailure = function (dataRow) {
        if (dataRow.QuantityFailure > this._round(dataRow.QuantityRemains - dataRow.QuantityShortage, requireConfig.websiteOptions.rndQuantity)) dataRow.set("QuantityFailure", this._round(dataRow.QuantityRemains - dataRow.QuantityShortage, requireConfig.websiteOptions.rndQuantity));
        this._updateRowQuantity(dataRow);

        this._updateTotalToModelProperty("TotalQuantityFailure", "QuantityFailure", "sum", requireConfig.websiteOptions.rndQuantity);
    }

    definedExemplar.prototype._changeQuantityExcess = function (dataRow) {
        this._updateTotalToModelProperty("TotalQuantityExcess", "QuantityExcess", "sum", requireConfig.websiteOptions.rndQuantity);
    }

    definedExemplar.prototype._changeQuantityShortage = function (dataRow) {
        if (dataRow.QuantityShortage > this._round(dataRow.QuantityRemains - dataRow.QuantityFailure, requireConfig.websiteOptions.rndQuantity)) dataRow.set("QuantityShortage", this._round(dataRow.QuantityRemains - dataRow.QuantityFailure, requireConfig.websiteOptions.rndQuantity));
        this._updateRowQuantity(dataRow);

        this._updateTotalToModelProperty("TotalQuantityShortage", "QuantityShortage", "sum", requireConfig.websiteOptions.rndQuantity);
    }

    definedExemplar.prototype._changeQuantityAndExcess = function (dataRow) {
        this._updateRowQuantity(dataRow);
    }

    definedExemplar.prototype._changeSwarfs = function (dataRow) {
        this._updateTotalToModelProperty("TotalSwarfs", "Swarfs", "sum", requireConfig.websiteOptions.rndQuantity);
    }


    //TO USE THIS FUNCTION: DON'T ALLOW TO CHANGE Quantity & QuantityExcess BY USER (NOT EDITABLE)
    //IN BRIEF: Quantity & QuantityExcess ARE CALCULATED BY THIS FUNCTION
    definedExemplar.prototype._updateRowQuantity = function (dataRow) {
        if (dataRow.QuantityAndExcess > this._round(dataRow.QuantityRemains - dataRow.QuantityFailure - dataRow.QuantityShortage, requireConfig.websiteOptions.rndQuantity)) {
            dataRow.set("Quantity", this._round(dataRow.QuantityRemains - dataRow.QuantityFailure - dataRow.QuantityShortage, requireConfig.websiteOptions.rndQuantity));
            dataRow.set("QuantityExcess", this._round(dataRow.QuantityAndExcess - dataRow.Quantity, requireConfig.websiteOptions.rndQuantity));
        }
        else {
            dataRow.set("Quantity", dataRow.QuantityAndExcess);
            dataRow.set("QuantityExcess", 0);
        }
    }

    return definedExemplar;
}));