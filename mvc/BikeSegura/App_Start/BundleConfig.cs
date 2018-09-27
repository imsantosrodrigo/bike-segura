﻿using System.Web;
using System.Web.Optimization;

namespace BikeSegura
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            //Modelo para adicionar novo script
            //bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
            //          "~/Scripts/bootstrap.js",
            //          ""));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/pagina-inicial.css",
                      "~/Content/all.css"));
            //"~/Content/site.css"));

            //bundles.Add(new StyleBundle("~/Content/css-form").Include(
            //          "~/Content/bootstrap.min.css",
            //          "~/Content/formulario.css",
            //          "~/Content/all.css"));
        }
    }
}
