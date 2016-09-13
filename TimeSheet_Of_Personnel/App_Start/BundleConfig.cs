using System.Web;
using System.Web.Optimization;

namespace TimeSheet_Of_Personnel
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // IF ENABLE THIS - JQ-DATA-PICKER CAN NOT ACHIEVE ITS STYLES
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            // For JQuery-UI DataPicker
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                    "~/Scripts/jquery-ui-{version}.js",
                    "~/Scripts/a-datapicker-ua.js"));

            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
                      "~/Content/themes/base/all.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // TIME-SHEET BUNDLES FOR MONTH-VIEW :
            bundles.Add(new ScriptBundle("~/bundles/monthView").Include(
                      "~/Scripts/a-time-sheet.js"));

            bundles.Add(new StyleBundle("~/Content/monthView").Include(
                    "~/Content/a-time-sheet.css"));
        }
    }
}
