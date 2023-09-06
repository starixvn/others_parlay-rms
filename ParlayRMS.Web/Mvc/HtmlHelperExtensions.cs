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
            var result = $"<select id='{id}' multiple='multiple'>";
            foreach (var item in items)
            {
                result +=
                    $"<option value='{item.Value}'{(item.Selected ? " selected='selected'" : string.Empty)}>{item.Text}</option>";
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
                var isChecked = item.Selected;
                stringBuilder.Append(helper.RadioButton(name, item.Value, isChecked, new { id }));
                stringBuilder.Append(helper.Label(id, item.Text));
            }

            return new MvcHtmlString(stringBuilder.ToString());
        }
    }
}
