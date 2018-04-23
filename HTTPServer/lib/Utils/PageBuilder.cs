using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

namespace HTTPServer.lib.Utils
{
    public static class PageBuilder
    {
        private static readonly string GreenCheck = Properties.Resources.green_check;
        private static readonly string RedX = Properties.Resources.red_x;

        public static string MethodListInApi(Type type)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("<HTML>");
            result.AppendLine("<Head>");
            if (type.Namespace != null)
            {
                result.AppendLine($"<Title>{string.Join(".", type.Namespace.Split('.').Skip(1))}</Title>");
                result.AppendLine("<style>");
                result.AppendLine("body { background-color: powderblue; }");
                result.AppendLine("table { border-collapse: collapse; margin-left:50px;}");
                result.AppendLine("table, th, td { border: 1px solid #1c5191; }");
                result.AppendLine("h2 { text-decoration: underline double blue; font-size: 28px; font-weight: bold; }");
                result.AppendLine("tr:nth-child(even) {background: #739dd2}");
                result.AppendLine("tr:nth-child(odd) {background: #8abdfc}");
                result.AppendLine(
                    "th { text-align:left; padding:3px; background-color: #e6e6e6; font-size: 22px; font-weight: bold; }");
                result.AppendLine("th.NameColumn {min-width:250px;}");
                result.AppendLine("th.DescriptionColumn {min-width:250px; max-width:500px}");
                result.AppendLine("th.boolColumn {width:48px; text-align:center; }");
                result.AppendLine("td {  font-size: 18px; font-weight: bold; }");
                result.AppendLine("td.boolColumn { text-align:center; }");
                result.AppendLine("td.descriptionCell { font-weight: normal; padding-right: 5px}");
                result.AppendLine("</style>");
                result.AppendLine("</Head>");
                result.AppendLine("<Body>");

                // Make an "up one level" link
                result.Append(@"<a href=""../"">").Append("Up One Level").AppendLine("</a>");

                List<Type> nameSpaces = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.Namespace != null
                                && t.Namespace.StartsWith(type.Namespace, StringComparison.Ordinal)
                                && t.GetMethod(Steward.HANDLERNAME) != null).ToList();

                // There will always be at least one because the passed-in type will be present
                if (nameSpaces.Count > 1)
                {
                    result.AppendLine(@"<div id=""SubModules"">");
                    result.AppendLine(@"<h2>Sub-Modules:</h2>");
                    result.AppendLine(@"<ul class=""SubModulesList"">");


                    foreach (Type nameSpace in nameSpaces)
                    {
                        if (nameSpace.Namespace != null)
                        {
                            string nm = nameSpace.Namespace.Replace(type.Namespace, string.Empty);
                            if (nm.StartsWith("."))
                            {
                                nm = nm.Substring(1, nm.Length - 1);
                            }

                            if (nm != string.Empty)
                            {
                                result.AppendLine($"<li><a href=\"./{nm}/\">{nm}</a></li>");
                            }
                        }
                    }

                    result.AppendLine("</ul>");
                }
            }

            // Make a Pages List
            List<MethodInfo> pages = type.GetMethods().Where(m => m.GetCustomAttribute(typeof(Steward.WebPagesAttributes)) != null).ToList();
            if (pages.Any())
            {
                result.AppendLine(@"<div id=""Pages"">");
                result.AppendLine(@"<h2>Pages:</h2>");
                result.AppendLine(@"<ul class=""PagesList"">");


                foreach (MethodInfo page in pages)
                {
                    result.AppendLine($"<li><a href=\"./{page.Name}/\">{page.Name}</a></li>");
                }

                result.AppendLine("</ul>");
            }
            
            // Methods Table
            List<MethodInfo> methods = type.GetMethods().Where(m => m.GetCustomAttribute(typeof(Steward.ApiMethodAttributes)) != null).ToList();
            if (methods.Any())
            {
                result.AppendLine(@"<div id=""Methods"">");
                result.AppendLine(@"<h2>Methods:</h2>");
                result.AppendLine(@"<table class=""MethodTable"" border=""1"" min-width=""500px"" max-width=""100%"">");
                result.AppendLine(@"<tr>");
                result.AppendLine(@"<th class=""NameColumn"">Method Name</th>");
                result.AppendLine(@"<th class=""DescriptionColumn"">Description</th>");
                result.AppendLine(@"<th class=""boolColumn"">GET</th>");
                result.AppendLine(@"<th class=""boolColumn"">POST</th>");
                result.AppendLine(@"<th class=""boolColumn"">PUT</th>");
                result.AppendLine(@"<th class=""boolColumn"">DELETE</th>");
                result.AppendLine(@"</tr>");

                foreach (MethodInfo method in methods)
                {
                    Steward.ApiMethodAttributes attr = (Steward.ApiMethodAttributes)method.GetCustomAttribute(typeof(Steward.ApiMethodAttributes));
                    result.AppendLine("<tr>");
                    result.AppendLine($"<td>{method.Name}</th>");
                    result.AppendLine($"<td class=\"descriptionCell\">{attr.Description}</th>");
                    result.Append(@"<td class=""boolColumn"">").Append((attr.GetSupported) ? GreenCheck : RedX).AppendLine("</th>");
                    result.Append(@"<td class=""boolColumn"">").Append((attr.PostSupported) ? GreenCheck : RedX).AppendLine("</th>");
                    result.Append(@"<td class=""boolColumn"">").Append((attr.PutSupported) ? GreenCheck : RedX).AppendLine("</th>");
                    result.Append(@"<td class=""boolColumn"">").Append((attr.DeleteSupported) ? GreenCheck : RedX).AppendLine("</th>");
                    result.AppendLine("</tr>");
                }
                result.AppendLine("</table>");
                result.AppendLine("</div>");
            }
            result.AppendLine("</Body>");
            result.AppendLine("</HTML>");
            return result.ToString();
        }
    }
}
