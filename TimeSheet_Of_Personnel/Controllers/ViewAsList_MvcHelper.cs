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
        // FOR USING HTML-HELPER

        public static MvcHtmlString ViewAsList(this HtmlHelper hlpr, List<string> strings)
        {
            TagBuilder tagBldr = new TagBuilder("ul");

            foreach (string str in strings)
            {
                TagBuilder liTag = new TagBuilder("li");
                liTag.SetInnerText(str);
                tagBldr.InnerHtml += liTag.ToString();
            }

            return new MvcHtmlString(tagBldr.ToString());
        }
    }
}