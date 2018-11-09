require(["xlsxNmvn", "xlsxWorkbook"], function (xlsxNmvn, xlsxWorkbook) {

    $(document).ready(function () {
        var xlf = document.getElementById('xlf');

        if (xlf != null) {
            if (xlf.addEventListener) {
                xlf.addEventListener('change', handleFile, false);
            }
        }
    });




    process_wb = function (wb) {

        var jsonWorkBook = JSON.stringify(to_json(wb), 2, 2); //jsonWorkBook = to_formulae(wb); //jsonWorkBook = to_csv(wb);
        var excelRowCollection = JSON.parse(jsonWorkBook);

        var xlsxWorkbookInstance = new xlsxWorkbook(["CommodityCode", "Quantity"]);
        if (xlsxWorkbookInstance.checkValidColumn(excelRowCollection.ImportSheet)) {

            var gridDataSource = $("#kendoGridDetails").data("kendoGrid").dataSource;

            for (i = 0; i < excelRowCollection.ImportSheet.length; i++) {

                var dataRow = gridDataSource.add({});
                var excelRow = excelRowCollection.ImportSheet[i];

                dataRow.set("Remarks", DoRound(excelRow["Quantity"], requireConfig.websiteOptions.rndQuantity));

                _getCommoditiesByCode(dataRow, excelRow);
            }
        }
        else {
            alert("Lỗi import dữ liệu. Vui lòng kiểm tra file excel cẩn thận trước khi thử import lại");
        }



        function _getCommoditiesByCode(dataRow, excelRow) {
            return $.ajax({
                url: window.urlCommoditiesApi,
                data: JSON.stringify({ "locationID": requireConfig.pageOptions.LocationID, "customerID": $("#Customer_CustomerID").val(), "warehouseID": $("#Warehouse_WarehouseID").val(), "priceCategoryID": $("#PriceCategoryID").val(), "applyToSalesVersusReturns": requireConfig.pageOptions.ApplyToSalesVersusReturns, "promotionID": $("#Promotion_PromotionID").val(), "entryDate": $("#EntryDate").data("kendoDateTimePicker").value().toUTCString(), "searchText": excelRow["CommodityCode"] }),

                type: 'POST',
                contentType: 'application/json;',
                dataType: 'json',
                success: function (result) {
                    if (result.CommodityID > 0) {
                        dataRow.set("CommodityID", result.CommodityID);
                        dataRow.set("CommodityCode", result.CommodityCode);
                        dataRow.set("CommodityName", result.CommodityName);
                        dataRow.set("CommodityTypeID", result.CommodityTypeID);

                        dataRow.set("WarehouseID", result.WarehouseID);
                        dataRow.set("WarehouseCode", result.WarehouseCode);

                        dataRow.set("TradeDiscountRate", $("#VATbyRow").val() == 'True' ? result.TradeDiscountRate : $("#TradeDiscountRate").val());
                        dataRow.set("VATPercent", DoRound(result.VATPercent, 0));

                        if (result.ListedPrice > 0) {
                            dataRow.set("ListedPrice", DoRound(result.ListedPrice, requireConfig.websiteOptions.rndAmount));
                        }
                        else {

                            dataRow.set("GrossPrice", DoRound(result.GrossPrice, requireConfig.websiteOptions.rndAmount));
                            dataRow.set("ListedPrice", DoRound(dataRow.UnitPrice, requireConfig.websiteOptions.rndAmount));
                            dataRow.set("ListedGrossPrice", DoRound(dataRow.GrossPrice, requireConfig.websiteOptions.rndAmount));
                        }

                        dataRow.set("DiscountPercent", DoRound(result.DiscountPercent, requireConfig.websiteOptions.rndDiscountPercent));

                        dataRow.set("QuantityAvailable", DoRound(result.QuantityAvailable, requireConfig.websiteOptions.rndQuantity));
                        dataRow.set("ControlFreeQuantity", DoRound(result.ControlFreeQuantity, requireConfig.websiteOptions.rndQuantity));
                        dataRow.set("Quantity", DoRound(dataRow.Remarks, requireConfig.websiteOptions.rndQuantity));

                        dataRow.set("Remarks", "Imported");
                    }
                    else
                        dataRow.set("CommodityName", result.CommodityName);
                },
                error: function (jqXHR, textStatus) {
                    dataRow.set("CommodityName", textStatus);
                }
            });
        }



    }




});