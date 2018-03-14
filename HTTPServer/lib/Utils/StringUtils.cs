using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace HTTPServer.lib.Utils
{
    public static class StringUtils
    {
        public static string AddSpacesToSentence(string text, bool preserveAcronyms = true)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                {
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    {
                        newText.Append(' ');
                    }
                }
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
        
        public static string PrettyXml(string xml)
        {
            return PrettyXml(XDocument.Parse(xml));
        }

        public static string PrettyXml(XDocument xml)
        {
            StringBuilder result = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = false;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(result, settings))
            {
                xml.Save(xmlWriter);
            }

            return result.ToString();
        }

        public static string PrettyJson(string json)
        {
            return PrettyJson(Newtonsoft.Json.JsonConvert.DeserializeObject(json));
        }

        public static string PrettyJson(object json)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
