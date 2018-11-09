namespace TotalPortal.ViewModels.Helpers
{
    public class PrintViewModel
    {
        public int? Id { get; set; }
        public int? DetailID { get; set; }

        public int? FilterID { get; set; }

        public int LocationID { get; set; }

        public string ServerName { get { return "DATA-SERVER"; } }
        public string CatalogName { get { return "TotalSmartPortal"; } }

        public string ReportFolder { get { return "TotalSmartPortal"; } }
        public string ReportPath { get; set; }

        public int PrintOptionID { get; set; }
    }
}