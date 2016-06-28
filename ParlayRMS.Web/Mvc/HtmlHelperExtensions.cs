using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ParlayRMS.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
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