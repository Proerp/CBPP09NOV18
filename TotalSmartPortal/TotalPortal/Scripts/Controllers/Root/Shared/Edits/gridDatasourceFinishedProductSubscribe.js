define(["gridDatasourceFinishedProduct"], (function (gridDatasourceFinishedProduct) {
    $(document).ready(function () {

        $("#kendoGridDetails").data("kendoGrid").dataSource.bind("change", function (e) {
            var gridDatasourceFinishedProductInstance = new gridDatasourceFinishedProduct("kendoGridDetails");
            gridDatasourceFinishedProductInstance.handleDataSourceChange(e);
        });

    });
}));
