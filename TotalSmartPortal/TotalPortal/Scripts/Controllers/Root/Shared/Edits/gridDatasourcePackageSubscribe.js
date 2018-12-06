define(["gridDatasourcePackage"], (function (gridDatasourcePackage) {
    $(document).ready(function () {

        $("#kendoGridDetails").data("kendoGrid").dataSource.bind("change", function (e) {
            var gridDatasourcePackageInstance = new gridDatasourcePackage("kendoGridDetails");
            gridDatasourcePackageInstance.handleDataSourceChange(e);
        });

    });
}));
