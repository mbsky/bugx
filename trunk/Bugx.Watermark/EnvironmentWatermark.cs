// <copyright file="EnvironmentWatermark.cs" company="Wavenet">
// Copyright © Wavenet 2009
// </copyright>
// <author>Olivier Bossaer</author>
// <email>olivier.bossaer@gmail.com</email>
// <date>2006-04-29</date>

namespace Bugx.Watermark
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Globalization;
    using System.Text;
    using System.Collections.Specialized;

    /// <summary>
    /// The <see cref="EnvironmentWatermark"/> module add a watermark on all pages.
    /// </summary>
    public class EnvironmentWatermark : IHttpModule
    {
        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements System.Web.IHttpModule.        
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Initializes <see cref="EnvironmentWatermark"/> and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            context.PostAcquireRequestState += this.BeginRequest;
        }

        /// <summary>
        /// For each page request a script is include to handle the watermark.
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains no event data</param>
        private void BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current == null || !ConfigurationSettings.Enable)
            {
                return;
            }

            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                var script = page.ClientScript.GetWebResourceUrl(typeof(EnvironmentWatermark), "Bugx.Watermark.Script.js");
                var images = page.ClientScript.GetWebResourceUrl(typeof(EnvironmentWatermark), "Bugx.Watermark.Images.png");
                var buttons = page.ClientScript.GetWebResourceUrl(typeof(EnvironmentWatermark), "Bugx.Watermark.Buttons.gif");
                var css = page.ClientScript.GetWebResourceUrl(typeof(EnvironmentWatermark), "Bugx.Watermark.Stylesheet.css");
                page.ClientScript.RegisterClientScriptInclude(typeof(EnvironmentWatermark), "ScriptInclude", script);
                page.ClientScript.RegisterClientScriptBlock(
                    typeof(EnvironmentWatermark), 
                    "Script",
                    string.Format(CultureInfo.InvariantCulture, "<script type=\"text/javascript\" defer=\"defer\">/*<![CDATA[*/RegisterWatermark('{0}', '{1}', '{2}', {3}, '{4}');/*]]>*/</script>", images, css, ConfigurationSettings.Text.Replace("'", "\\'"), this.Items, buttons),
                    false);
            }
        }

        private string Items
        {
            get
            {
                StringBuilder result = new StringBuilder("[");
                NameValueCollection items = ConfigurationSettings.MenuItems;
                if (items.Count > 0)
                {
                    foreach (string key in items.AllKeys)
                    {
                        result.AppendFormat("{{Title:'{0}',Url:'{1}'}},", key.Replace("'", "\\'"), items[key].Replace("'", "\\'"));
                    }

                    result.Length--;
                }

                return result.Append(']').ToString();
            }
        }
    }
}