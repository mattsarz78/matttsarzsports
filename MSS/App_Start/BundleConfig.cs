using System.Web.Optimization;

namespace MSS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            const string jQueryCdn = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.10.0.min.js";
            const string modernizerCdn = "http://ajax.aspnetcdn.com/ajax/modernizr/modernizr-2.8.3.js";
            const string ajaxCdn = "http://ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js";

            bundles.Add(new ScriptBundle("~/bundles/jquery", jQueryCdn).Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/ajax", ajaxCdn).Include("~/Scripts/MicrosoftAjax.js"));
            bundles.Add(new ScriptBundle("~/bundles/weekly").Include("~/Scripts/Weekly.js", "~/Scripts/BackToTop.js"));
            bundles.Add(new ScriptBundle("~/bundles/weektext").Include("~/Scripts/WeekText.js", "~/Scripts/BackToTop.js"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr", modernizerCdn).Include("~/Scripts/modernizr-*"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
