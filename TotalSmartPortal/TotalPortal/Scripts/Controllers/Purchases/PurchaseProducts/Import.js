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

        var xlsxWorkbookInstance = new xlsxWorkbook(["ImportID", "SoDonHang", "NgayDatHang", "NhaCungCap", "Rcode", "SoLuong", "LabCode", "ProductionDate", "ExpiryDate", "VoucherDate"]);
        if (xlsxWorkbookInstance.checkValidColumn(excelRowCollection.ImportSheet)) {

            var gridDataSource = $("#kendoGridDetails").data("kendoGrid").dataSource;

            if (excelRowCollection.ImportSheet.length > 0) {

                for (j = 0; j < excelRowCollection.ImportSheet.length; j++) {

                    
                    if (excelRowCollection.ImportSheet[j]["ImportID"] == 1) {



                        var excelRow0 = excelRowCollection.ImportSheet[j];
                        if (excelRow0["SoDonHang"] != null) $("#Code").val(excelRow0["SoDonHang"]);
                        if (excelRow0["TenHonHop"] != null) $("#Name").val(excelRow0["TenHonHop"]);

                        if (excelRow0["NgayDatHang"] != "") {
                            var mySplits = excelRow0["NgayDatHang"].split(/[.,\/ -]/);
                            if (mySplits.length == 3) {
                                var voucherDate = new Date((mySplits[2].length == 2 ? "20" + mySplits[2] : mySplits[2]), mySplits[0] - 1, mySplits[1]);
                                if (excelRow0["NgayDatHang"] != null) $("#EntryDate").val(kendo.toString(voucherDate, Settings.DateFormat));
                            }
                        }

                        if (excelRow0["VoucherDate"] != "") {
                            var mySplits = excelRow0["VoucherDate"].split(/[.,\/ -]/);
                            if (mySplits.length == 3) {
                                var deliveryDate = new Date((mySplits[2].length == 2 ? "20" + mySplits[2] : mySplits[2]), mySplits[0] - 1, mySplits[1]);
                                if (excelRow0["VoucherDate"] != null) $("#VoucherDate").val(kendo.toString(deliveryDate, Settings.DateFormat));
                            }
                        }

                        _getCustomers(excelRow0["NhaCungCap"]);

                        for (i = j; i < excelRowCollection.ImportSheet.length; i++) {

                            if (excelRowCollection.ImportSheet[i]["ImportID"] != 1) { break; }

                            var dataRow = gridDataSource.add({});
                            var excelRow = excelRowCollection.ImportSheet[i];
                            
                            dataRow.LabCode = excelRow["LabCode"];

                            if (excelRow["ProductionDate"] != "") {
                                var mySplits = excelRow["ProductionDate"].split(/[.,\/ -]/);
                                if (mySplits.length == 3) {
                                    var productionDate = new Date((mySplits[2].length == 2 ? "20" + mySplits[2] : mySplits[2]), mySplits[0] - 1, mySplits[1]);
                                    if (excelRow["ProductionDate"] != null) dataRow.ProductionDate = kendo.toString(productionDate, Settings.DateFormat);
                                }
                            }

                            if (excelRow["ExpiryDate"] != "") {
                                var mySplits = excelRow["ExpiryDate"].split(/[.,\/ -]/);
                                if (mySplits.length == 3) {
                                    var expiryDate = new Date((mySplits[2].length == 2 ? "20" + mySplits[2] : mySplits[2]), mySplits[0] - 1, mySplits[1]);
                                    if (excelRow["ExpiryDate"] != null) dataRow.ExpiryDate = kendo.toString(expiryDate, Settings.DateFormat);
                                }
                            }

                            dataRow.set("QuantityTemp", DoRound(excelRow["SoLuong"], requireConfig.websiteOptions.rndQuantity));

                            _getCommoditiesByCode(dataRow, excelRow);
                        }


                        break;

                    }
                }
            }
        }
        else {
            alert("Lỗi import dữ liệu. Vui lòng kiểm tra file excel cẩn thận trước khi thử import lại");
        }



        function _getCommoditiesByCode(dataRow, excelRow) {
            return $.ajax({
                url: window.urlCommoditiesApi,
                data: JSON.stringify({ "commodityTypeIDList": requireConfig.pageOptions.commodityTypeIDList, "searchText": excelRow["Rcode"], "isOnlyAlphaNumericString": false }),

                type: 'POST',
                contentType: 'application/json;',
                dataType: 'json',
                success: function (result) {
                    if (result.CommodityID > 0) {

                        dataRow.CommodityID = result.CommodityID;
                        dataRow.CommodityName = result.CommodityName;
                        dataRow.CommodityCode = result.CommodityCode;
                        dataRow.CommodityTypeID = result.CommodityTypeID;
                        
                        dataRow.set("Quantity", DoRound(dataRow.QuantityTemp, requireConfig.websiteOptions.rndQuantity));
                    }
                    else
                        dataRow.set("CommodityName", result.CommodityName);
                },
                error: function (jqXHR, textStatus) {
                    dataRow.set("CommodityName", textStatus);
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





                        $("#Transporter_CustomerID").val(result.CustomerID);
                        $("#Transporter_Code").val(result.Code);
                        $("#Transporter_Name").val(result.Name);
                        $("#Transporter_CodeAndName").val(result.CodeAndName);
                        $("#Transporter_OfficialName").val(result.OfficialName);

                        $("#Transporter_TerritoryID").val(result.TerritoryID);
                        $("#Transporter_SalespersonID").val(result.SalespersonID);
                        $("#Transporter_PaymentTermID").val(result.PaymentTermID);
                        $("#Transporter_PriceCategoryID").val(result.PriceCategoryID);
                        $("#Transporter_WarehouseID").val(result.WarehouseID);
                        $("#Transporter_ShowDiscount").val(result.ShowDiscount);

                        $("#Transporter_TempCodeAndName").val(result.CodeAndName);
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








































//require(["xlsxNmvn", "xlsxWorkbook"], function (xlsxNmvn, xlsxWorkbook) {

//    $(document).ready(function () {
//        var xlf = document.getElementById('xlf');

//        if (xlf != null) {
//            if (xlf.addEventListener) {
//                xlf.addEventListener('change', handleFile, false);
//            }
//        }
//    });




//    process_wb = function (wb) {

//        var jsonWorkBook = JSON.stringify(to_json(wb), 2, 2); //jsonWorkBook = to_formulae(wb); //jsonWorkBook = to_csv(wb);
//        var excelRowCollection = JSON.parse(jsonWorkBook);

//        var xlsxWorkbookInstance = new xlsxWorkbook(["SoDonHang", "NgayDatHang", "NhaCungCap", "Rcode", "SoLuong"]);
//        if (xlsxWorkbookInstance.checkValidColumn(excelRowCollection.ImportSheet)) {

//            var gridDataSource = $("#kendoGridDetails").data("kendoGrid").dataSource;

//            if (excelRowCollection.ImportSheet.length > 0) {

//                var excelRow0 = excelRowCollection.ImportSheet[0];
//                if (excelRow0["SoDonHang"] != null) $("#Code").val(excelRow0["SoDonHang"]);
//                if (excelRow0["TenHonHop"] != null) $("#Name").val(excelRow0["TenHonHop"]);

//                if (excelRow0["NgayDatHang"] != "") {
//                    var mySplits = excelRow0["NgayDatHang"].split(/[.,\/ -]/);
//                    if (mySplits.length == 3) {
//                        var voucherDate = new Date((mySplits[2].length == 2 ? "20" + mySplits[2] : mySplits[2]), mySplits[0] - 1, mySplits[1]);
//                        if (excelRow0["NgayDatHang"] != null) $("#VoucherDate").val(kendo.toString(voucherDate, Settings.DateFormat));
//                    }
//                }

//                _getCustomers(excelRow0["NhaCungCap"]);

//                for (i = 0; i < excelRowCollection.ImportSheet.length; i++) {

//                    var dataRow = gridDataSource.add({});
//                    var excelRow = excelRowCollection.ImportSheet[i];

//                    dataRow.set("QuantityTemp", DoRound(excelRow["SoLuong"], requireConfig.websiteOptions.rndQuantity));

//                    _getCommoditiesByCode(dataRow, excelRow);
//                }
//            }
//        }
//        else {
//            alert("Lỗi import dữ liệu. Vui lòng kiểm tra file excel cẩn thận trước khi thử import lại");
//        }



//        function _getCommoditiesByCode(dataRow, excelRow) {
//            return $.ajax({
//                url: window.urlCommoditiesApi,
//                data: JSON.stringify({ "commodityTypeIDList": requireConfig.pageOptions.commodityTypeIDList, "searchText": excelRow["Rcode"], "isOnlyAlphaNumericString": false }),

//                type: 'POST',
//                contentType: 'application/json;',
//                dataType: 'json',
//                success: function (result) {
//                    if (result.CommodityID > 0) {

//                        dataRow.CommodityID = result.CommodityID;
//                        dataRow.CommodityName = result.CommodityName;
//                        dataRow.CommodityCode = result.CommodityCode;
//                        dataRow.CommodityTypeID = result.CommodityTypeID;

//                        dataRow.set("Quantity", DoRound(dataRow.QuantityTemp, requireConfig.websiteOptions.rndQuantity));
//                    }
//                    else
//                        dataRow.set("CommodityName", result.CommodityName);
//                },
//                error: function (jqXHR, textStatus) {
//                    dataRow.set("CommodityName", textStatus);
//                }
//            });
//        }


//        function _getCustomers(customerCode) {
//            return $.ajax({
//                url: window.urlCustomersApi,
//                data: JSON.stringify({ "searchText": customerCode, "warehouseTaskID": 0 }),

//                type: 'POST',
//                contentType: 'application/json;',
//                dataType: 'json',
//                success: function (result) {
//                    if (result.CustomerID > 0) {
//                        $("#Customer_CustomerID").val(result.CustomerID);
//                        $("#Customer_Code").val(result.Code);
//                        $("#Customer_Name").val(result.Name);
//                        $("#Customer_CodeAndName").val(result.CodeAndName);
//                        $("#Customer_OfficialName").val(result.OfficialName);

//                        $("#Customer_TerritoryID").val(result.TerritoryID);
//                        $("#Customer_SalespersonID").val(result.SalespersonID);
//                        $("#Customer_PaymentTermID").val(result.PaymentTermID);
//                        $("#Customer_PriceCategoryID").val(result.PriceCategoryID);
//                        $("#Customer_WarehouseID").val(result.WarehouseID);
//                        $("#Customer_ShowDiscount").val(result.ShowDiscount);

//                        $("#Customer_TempCodeAndName").val(result.CodeAndName);





//                        $("#Transporter_CustomerID").val(result.CustomerID);
//                        $("#Transporter_Code").val(result.Code);
//                        $("#Transporter_Name").val(result.Name);
//                        $("#Transporter_CodeAndName").val(result.CodeAndName);
//                        $("#Transporter_OfficialName").val(result.OfficialName);

//                        $("#Transporter_TerritoryID").val(result.TerritoryID);
//                        $("#Transporter_SalespersonID").val(result.SalespersonID);
//                        $("#Transporter_PaymentTermID").val(result.PaymentTermID);
//                        $("#Transporter_PriceCategoryID").val(result.PriceCategoryID);
//                        $("#Transporter_WarehouseID").val(result.WarehouseID);
//                        $("#Transporter_ShowDiscount").val(result.ShowDiscount);

//                        $("#Transporter_TempCodeAndName").val(result.CodeAndName);
//                    }
//                    else
//                        $("#Customer_Code").val(result.Code);
//                },
//                error: function (jqXHR, textStatus) {
//                    $("#Customer_Code").val(textStatus);
//                }
//            });
//        }

//    }




//});
