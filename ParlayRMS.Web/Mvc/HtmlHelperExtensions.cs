// -----------------------------------------------------------------------
// <copyright file="HtmlHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString DropDownListWithMultipleSelected(this HtmlHelper helper, string id, List<SelectListItem> items)
        {
            var result = string.Format("<select id='{0}' multiple='multiple'>", id);
            foreach (var item in items)
            {
                result += string.Format("<option value='{0}'{1}>{2}</option>", item.Value, item.Selected ? " selected='selected'" : "", item.Text);
            }

            result += "</select>";
            return new MvcHtmlString(result);
        }
    }
}
