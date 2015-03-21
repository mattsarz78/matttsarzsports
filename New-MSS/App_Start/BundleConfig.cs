using System.Web.Optimization;

namespace New_MSS.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            const string jQueryCdn = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.10.0.min.js";
            const string modernizerCdn = "http://cdnjs.cloudflare.com/ajax/libs/modernizr/2.6.2/modernizr.min.js";
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