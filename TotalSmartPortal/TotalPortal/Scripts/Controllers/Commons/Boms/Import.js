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
                data: JSON.stringify({ "commodityTypeIDList": requireConfig.pageOptions.commodityTypeIDList, "searchText": excelRow["CommodityCode"], "isOnlyAlphaNumericString": false }),

                type: 'POST',
                contentType: 'application/json;',
                dataType: 'json',
                success: function (result) {
                    if (result.CommodityID > 0) {
                        
                        dataRow.CommodityID = result.CommodityID;
                        dataRow.CommodityName = result.CommodityName;
                        dataRow.CommodityCode = result.CommodityCode;
                        dataRow.CommodityTypeID = result.CommodityTypeID;

                        dataRow.set("Quantity", DoRound(dataRow.Remarks, requireConfig.websiteOptions.rndQuantity));
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