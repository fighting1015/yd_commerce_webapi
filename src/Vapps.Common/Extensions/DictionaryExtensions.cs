using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;
using Vapps.Infrastructure;

namespace Vapps
{
    [Serializable]
    public class ConvertProblem
    {
        public object Item { get; set; }
        public PropertyInfo Property { get; set; }
        public object AttemptedValue { get; set; }
        public Exception Exception { get; set; }

        public override string ToString()
        {
            return
                @"Item type:     {0}
                Property:        {1}
                Property Type:   {2}
                Attempted Value: {3}
                Exception:
                {4}."
                    .FormatCurrent(
                    ((Item != null) ? Item.GetType().FullName : "(null)"),
                    Property.Name,
                    Property.PropertyType,
                    AttemptedValue,
                    Exception);
        }
    }


    [Serializable]
    public class DictionaryConvertException : Exception
    {
        public DictionaryConvertException(ICollection<ConvertProblem> problems)
            : this(CreateMessage(problems), problems)
        {
        }

        public DictionaryConvertException(string message, ICollection<ConvertProblem> problems)
            : base(message)
        {
            Problems = problems;
        }


        public ICollection<ConvertProblem> Problems { get; private set; }

        private static string CreateMessage(ICollection<ConvertProblem> problems)
        {
            var counter = 0;
            var builder = new StringBuilder();
            builder.Append("Could not convert all input values into their expected types:");
            builder.Append(Environment.NewLine);
            foreach (var prob in problems)
            {
                builder.AppendFormat("-----Problem[{0}]---------------------", counter++);
                builder.Append(Environment.NewLine);
                builder.Append(prob);
                builder.Append(Environment.NewLine);
            }
            return builder.ToString();
        }
    }

    public static class DictionaryConverter
    {

        public static bool CanCreateType(Type itemType)
        {
            return itemType.GetTypeInfo().IsClass && itemType.GetConstructor(Type.EmptyTypes) != null;
        }


        public static void AddDataIfNotEmpty(this Dictionary<string, string> dictionary, XDocument document, string elementName)
        {
            XElement xElement = document.Root.Element(elementName);
            if (xElement != null)
            {
                dictionary.AddItemIfNotEmpty(elementName, xElement.Value);
            }
        }
        public static void AddItemIfNotEmpty(this IDictionary<string, string> dictionary, string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (!string.IsNullOrEmpty(value))
            {
                dictionary[key] = value;
            }
        }

        public static string GetString(this Dictionary<string, string> source)
        {
            StringBuilder dictionary = new StringBuilder("{");
            List<string> param = new List<string>();
            foreach (var item in source)
            {
                param.Add($"{item.Key}:\"{item.Value}\"");
            }

            return $"{{{string.Join(",", param)}}}";
        }

    }

}
