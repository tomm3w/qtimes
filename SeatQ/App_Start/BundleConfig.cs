using System.Web;
using System.Web.Optimization;

namespace SeatQ
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/cssadmin").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Content/dashboard.css"));
            
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/ko").Include(
                "~/Scripts/knockout-3.1.0.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/table-summary").Include(
               "~/Scripts/app/table-summary.js"
               ));

            bundles.Add(new ScriptBundle("~/scripts/mobile").Include(
                "~/Scripts/app/mobile/waitlist.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/waitlist").Include(
                "~/Scripts/app/waitlist.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/seating").Include(
                "~/Scripts/app/seating.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/seating").Include(
                "~/Scripts/app/seating.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/returnguest").Include(
                "~/Scripts/app/returnguest.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/message").Include(
                "~/Scripts/app/message.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/metrices").Include(
                "~/Scripts/app/metrices.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/hostess").Include(
                "~/Scripts/app/hostess.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/restaurant").Include(
                "~/Scripts/app/restaurant.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/accountinfo").Include(
                "~/Scripts/app/accountinfo.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/preferences").Include(
                "~/Scripts/app/preferences.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/tables").Include(
                "~/Scripts/app/tables.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/header").Include(
               "~/Scripts/app/header.js"
               ));

            bundles.Add(new ScriptBundle("~/scripts/table-layouts").Include(
               "~/Scripts/app/table-layouts.js"
               ));

            bundles.Add(new ScriptBundle("~/scripts/staff").Include(
               "~/Scripts/app/staff.js"
               ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
