// --------------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Web.Mvc
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    /// <summary>
    /// Html Helper Extensions.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Creates the drop down list which allows to select multiple options.
        /// </summary>
        /// <param name="helper">The HtmlHelper instance which the method will extend to.</param>
        /// <param name="id">The identifier of the HTML control.</param>
        /// <param name="items">The list of items which will be used to initialize the drop down list.</param>
        /// <returns>MVCHtmlString object represents the created control.</returns>
        public static MvcHtmlString DropDownListWithMultipleSelected(this HtmlHelper helper, string id, List<SelectListItem> items)
        {
            var result = string.Format("<select id='{0}' multiple='multiple'>", id);
            foreach (var item in items)
            {
                result += string.Format("<option value='{0}'{1}>{2}</option>", item.Value, item.Selected ? " selected='selected'" : string.Empty, item.Text);
            }

            result += "</select>";
            return new MvcHtmlString(result);
        }

        /// <summary>
        /// Creates the radio button list.
        /// </summary>
        /// <param name="helper">The HtmlHelper instance which the method will extend to.</param>
        /// <param name="name">The group name of the radio button list.</param>
        /// <param name="selectList">The list of items which will be used to initialize the radio button list.</param>
        /// <returns>MVCHtmlString object represents the created control.</returns>
        public static MvcHtmlString RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList)
        {
            var stringBuilder = new StringBuilder();
            foreach (var item in selectList)
            {
                var id = name + item.Value;
                bool isChecked = item.Selected == true;
                stringBuilder.Append(helper.RadioButton(name, item.Value, isChecked, new { id = id }));
                stringBuilder.Append(helper.Label(id, item.Text));
            }

            return new MvcHtmlString(stringBuilder.ToString());
        }
    }
}