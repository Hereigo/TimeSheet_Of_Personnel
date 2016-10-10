using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeSheet_Of_Personnel.Controllers
{
    public static class ViewAsList_MvcHelper
    {
        // HAVE TO ADD INTO WEB.CONFIG (FOR VIEWS)
        // <add namespace="TimeSheet_Of_Personnel.Controllers" />
        // FOR USING HTML-HELPER ON ANY PAGE
        // OR WRITE ON NEEDED PAGES:
        // @using TimeSheet_Of_Personnel.Controllers

        public static MvcHtmlString ViewAsList(this HtmlHelper hlpr, List<string> strings)
        {
            TagBuilder tagBldr = new TagBuilder("ul");

            foreach (string str in strings)
            {
                TagBuilder liTag = new TagBuilder("li");
                liTag.MergeAttribute("style", "font-weight:bold;color:blue");
                liTag.SetInnerText(str);
                tagBldr.InnerHtml += liTag.ToString();
            }
            // If RETURN just STRING - LI-tags will be returned as <li>-TEXT!
            return new MvcHtmlString(tagBldr.ToString());
        }
    }
}