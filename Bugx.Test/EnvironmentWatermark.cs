using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Bugx.Test
{
    /// <summary>
    /// Summary description for EnvironmentWatermark
    /// </summary>
    public class EnvironmentWatermark:IHttpModule
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.PostAcquireRequestState += BeginRequest;
        }

        void BeginRequest(object sender, EventArgs e)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                var script = page.ClientScript.GetWebResourceUrl(typeof(EnvironmentWatermark), "Bugx.Test.Script.js");
                var images = page.ClientScript.GetWebResourceUrl(typeof(EnvironmentWatermark), "Bugx.Test.Images.png");
                var css = page.ClientScript.GetWebResourceUrl(typeof(EnvironmentWatermark), "Bugx.Test.Stylesheet.css");
                page.ClientScript.RegisterClientScriptInclude(typeof(EnvironmentWatermark), "ScriptInclude", script);
                page.ClientScript.RegisterClientScriptBlock(typeof(EnvironmentWatermark), "Script", "<script type=\"text/javascript\" defer=\"defer\">/*<![CDATA[*/RegisterWatermark('" + images + "', '" + css + "');/*]]>*/</script>", false);
            }
        }
    }
}