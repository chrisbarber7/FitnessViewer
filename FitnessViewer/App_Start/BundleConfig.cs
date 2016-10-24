using System.Web;
using System.Web.Optimization;

namespace FitnessViewer
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                            "~/Scripts/jquery-1.10.2.min.js",
                            "~/Scripts/jquery.validate.min.js",
                            "~/Scripts/modernizr-*",
                            "~/Scripts/bootstrap.min.js",
                            "~/Scripts/respond.min.js",
                            "~/Scripts/underscore.min.js",
                            "~/Scripts/moment.min.js",
                            "~/Scripts/app/app.js"));
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                            "~/Content/bootstrap.css",
                            "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                            "~/Scripts/DataTables/jquery.dataTables.js",
                            "~/Scripts/DataTables/dataTables.bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/dataTables").Include(
                            "~/Content/DataTables/css/jquery.dataTables.css",
                            "~/Content/DataTables/css/jquery.dataTables.min.css"));

            bundles.Add(new StyleBundle("~/bundles/chart").Include(
                            "~/Scripts/Chart.js"));
        }
    }
}
