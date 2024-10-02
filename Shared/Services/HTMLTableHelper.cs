using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.Services
{
    public static class HTMLTableHelper
    {
        public static string ToHtmlTable<T>(this IEnumerable<T> enums)
        {
            var type = typeof(T);
            var props = type.GetProperties();
            var html = new StringBuilder("<table style='border: 1px solid black; border-collapse: collapse;'>");

            //Header
            html.Append("<thead><tr>");
            foreach (var p in props)
                html.Append("<th style='border: 1px solid black; border-collapse: collapse;padding:2px 4px 2px 4px; text-align: center;'>" + p.Name + "</th>");
            html.Append("</tr></thead>");

            //Body
            html.Append("<tbody>");
            foreach (var e in enums) {
                html.Append("<tr>");
                props.Select(s => s.GetValue(e)).ToList().ForEach(p => {
                    html.Append("<td style='border: 1px solid black; border-collapse: collapse; padding:2px 4px 2px 4px; text-align: center;'>" + p + "</td>");
                });
                html.Append("</tr>");
            }

            html.Append("</tbody>");
            html.Append("</table>");
            return html.ToString();
        }
    }
}
