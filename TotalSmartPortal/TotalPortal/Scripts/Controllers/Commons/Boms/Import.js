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

        var xlsxWorkbookInstance = new xlsxWorkbook(["MCT", "TenHonHop", "NgayApDung", "KhachHang", "MaMang", "MaNVL", "KhoiLuong", "TrucVit", "TyLe"]);
        if (xlsxWorkbookInstance.checkValidColumn(excelRowCollection.ImportSheet)) {

            var gridDataSource = $("#kendoGridDetails").data("kendoGrid").dataSource;
            
            if (excelRowCollection.ImportSheet.length > 0) {

                var excelRow0 = excelRowCollection.ImportSheet[0];
                if (excelRow0["MCT"] != null) $("#Code").val(excelRow0["MCT"]);
                if (excelRow0["TenHonHop"] != null) $("#Name").val(excelRow0["TenHonHop"]);
                if (excelRow0["NgayApDung"] != null) $("#EffectiveDate").val(kendo.toString(excelRow0["NgayApDung"], Settings.DateFormat));

                _getCustomers(excelRow0["KhachHang"]);
                _getMasterCommoditiesByCode(excelRow0["MaMang"]);
                                
                for (i = 0; i < excelRowCollection.ImportSheet.length; i++) {

                    var dataRow = gridDataSource.add({});
                    var excelRow = excelRowCollection.ImportSheet[i];

                    dataRow.set("LayerCode", excelRow["TrucVit"]);
                    dataRow.set("BlockUnit", DoRound(excelRow["TyLe"], requireConfig.websiteOptions.rndN0));

                    dataRow.set("Remarks", DoRound(excelRow["KhoiLuong"], requireConfig.websiteOptions.rndQuantity));

                    _getCommoditiesByCode(dataRow, excelRow);
                }
            }
        }
        else {
            alert("Lỗi import dữ liệu. Vui lòng kiểm tra file excel cẩn thận trước khi thử import lại");
        }



        function _getCommoditiesByCode(dataRow, excelRow) {
            return $.ajax({
                url: window.urlCommoditiesApi,
                data: JSON.stringify({ "commodityTypeIDList": requireConfig.pageOptions.commodityTypeIDList, "searchText": excelRow["MaNVL"], "isOnlyAlphaNumericString": false }),

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


        function _getMasterCommoditiesByCode(masterCommodityCode) {
            return $.ajax({
                url: window.urlCommoditiesApi,
                data: JSON.stringify({ "commodityTypeIDList": requireConfig.pageOptions.masterCommodityTypeIDs, "searchText": masterCommodityCode, "isOnlyAlphaNumericString": false }),

                type: 'POST',
                contentType: 'application/json;',
                dataType: 'json',
                success: function (result) {
                    if (result.CommodityID > 0) {
                        $("#Commodity_CommodityID").val(result.CommodityID);
                        $("#Commodity_Code").val(result.CommodityCode);
                        $("#Commodity_Name").val(result.CommodityName);

                        $("#Commodity_label_Name").val(result.CommodityName);
                        $("#Commodity_TempCode").val(result.CommodityCode);
                    }
                    else
                        $("#Commodity_Code").val(result.CommodityCode);
                },
                error: function (jqXHR, textStatus) {
                    $("#Commodity_Code").val(textStatus);
                }
            });
        }

        function _getCustomers(customerCode) {
            return $.ajax({
                url: window.urlCustomersApi,
                data: JSON.stringify({ "searchText": customerCode, "warehouseTaskID": 0 }),

                type: 'POST',
                contentType: 'application/json;',
                dataType: 'json',
                success: function (result) {
                    if (result.CustomerID > 0) {
                        $("#Customer_CustomerID").val(result.CustomerID);
                        $("#Customer_Code").val(result.Code);
                        $("#Customer_Name").val(result.Name);
                        $("#Customer_CodeAndName").val(result.CodeAndName);
                        $("#Customer_OfficialName").val(result.OfficialName);

                        $("#Customer_TerritoryID").val(result.TerritoryID);
                        $("#Customer_SalespersonID").val(result.SalespersonID);
                        $("#Customer_PaymentTermID").val(result.PaymentTermID);
                        $("#Customer_PriceCategoryID").val(result.PriceCategoryID);
                        $("#Customer_WarehouseID").val(result.WarehouseID);
                        $("#Customer_ShowDiscount").val(result.ShowDiscount);

                        $("#Customer_TempCodeAndName").val(result.CodeAndName);
                    }
                    else
                        $("#Customer_Code").val(result.Code);
                },
                error: function (jqXHR, textStatus) {
                    $("#Customer_Code").val(textStatus);
                }
            });
        }

    }




});