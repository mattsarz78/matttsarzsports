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
			const string modernizerCdn = "https://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js";
            const string ajaxCdn = "http://ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js";

            bundles.Add(new ScriptBundle("~/bundles/jquery", jQueryCdn).Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/ajax", ajaxCdn).Include("~/Scripts/MicrosoftAjax.js"));
			bundles.Add(new ScriptBundle("~/bundles/weekly").Include("~/Scripts/Schedule.js", "~/Scripts/Weekly.js", "~/Scripts/BackToTop.js"));
			bundles.Add(new ScriptBundle("~/bundles/weektext").Include("~/Scripts/Text.js", "~/Scripts/WeekText.js", "~/Scripts/BackToTop.js"));
			bundles.Add(new ScriptBundle("~/bundles/daily").Include("~/Scripts/Schedule.js", "~/Scripts/Daily.js", "~/Scripts/BackToTop.js"));
			bundles.Add(new ScriptBundle("~/bundles/dailytext").Include("~/Scripts/Text.js", "~/Scripts/DailyText.js", "~/Scripts/BackToTop.js"));
			bundles.Add(new ScriptBundle("~/bundles/modernizr", modernizerCdn).Include("~/Scripts/modernizr-*"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
