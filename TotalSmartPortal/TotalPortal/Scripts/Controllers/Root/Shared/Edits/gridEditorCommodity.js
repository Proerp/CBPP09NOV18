define(["superBase", "gridEditorTemplate"], (function (superBase, gridEditorTemplate) {

    var definedExemplar = function (kenGridName) {
        definedExemplar._super.constructor.call(this, kenGridName);
    }

    var superBaseHelper = new superBase();
    superBaseHelper.inherits(definedExemplar, gridEditorTemplate);



    //The commodity here is AutoComplete Widget
    definedExemplar.prototype.handleSelect = function (e) {
        var currentDataSourceRow = this._getCurrentDataSourceRow();

        if (currentDataSourceRow != undefined) {
            var dataItem = e.sender.dataItem(e.item.index());

            currentDataSourceRow.set("CommodityID", dataItem.CommodityID);
            currentDataSourceRow.set("CommodityCode", dataItem.CommodityCode);
            currentDataSourceRow.set("CommodityName", dataItem.CommodityName);
            currentDataSourceRow.set("CommodityTypeID", dataItem.CommodityTypeID);
            //currentDataSourceRow.set("Quantity", 0);

            currentDataSourceRow.set("TradeDiscountRate", currentDataSourceRow.VATbyRow === true ? dataItem.TradeDiscountRate : $("#TradeDiscountRate").val());
            currentDataSourceRow.set("VATPercent", currentDataSourceRow.VATbyRow === true ? dataItem.VATPercent : $("#VATPercent").val());


            if (dataItem.ListedPrice > 0) {
                if (currentDataSourceRow.ListedPrice != undefined)
                    currentDataSourceRow.set("ListedPrice", dataItem.ListedPrice);
                else
                    currentDataSourceRow.set("UnitPrice", dataItem.ListedPrice);
            }
            else {

                currentDataSourceRow.set("GrossPrice", dataItem.GrossPrice);

                if (currentDataSourceRow.ListedPrice != undefined)
                    currentDataSourceRow.set("ListedPrice", currentDataSourceRow.UnitPrice);
                if (currentDataSourceRow.ListedGrossPrice != undefined)
                    currentDataSourceRow.set("ListedGrossPrice", currentDataSourceRow.GrossPrice);
            }

            currentDataSourceRow.set("DiscountPercent", dataItem.DiscountPercent);
            
            if (currentDataSourceRow.PiecePerPack != undefined) { currentDataSourceRow.set("PiecePerPack", dataItem.PiecePerPack); }
            if (currentDataSourceRow.QuantityAvailables != undefined) { currentDataSourceRow.set("QuantityAvailables", dataItem.QuantityAvailables); }

            if (currentDataSourceRow.BomID != undefined)
            {
                currentDataSourceRow.set("BomID", dataItem.BomID === null ? 0 : dataItem.BomID);
                currentDataSourceRow.set("BomCode", dataItem.BomCode);
                currentDataSourceRow.set("BlockUnit", dataItem.BlockUnit === null ? 0 : dataItem.BlockUnit);
                currentDataSourceRow.set("BlockQuantity", dataItem.BlockQuantity === null ? 0 : dataItem.BlockQuantity);
            }
            if (currentDataSourceRow.MoldID != undefined) {
                currentDataSourceRow.set("MoldID", dataItem.MoldID === null ? 0 : dataItem.MoldID);
                currentDataSourceRow.set("MoldCode", dataItem.MoldCode);
                currentDataSourceRow.set("MoldQuantity", dataItem.MoldQuantity === null ? 0 : dataItem.MoldQuantity);
            }
        }

        window.commodityCodeBeforeChange = dataItem.CommodityCode;
    };


    definedExemplar.prototype.handleChange = function (e) {
        this._setEditorValue("CommodityCode", window.commodityCodeBeforeChange);
    };




    return definedExemplar;
}));