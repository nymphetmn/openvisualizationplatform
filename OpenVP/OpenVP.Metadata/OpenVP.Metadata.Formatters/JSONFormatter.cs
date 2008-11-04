// JSONFormatter.cs created with MonoDevelop
// User: chris at 1:32 AM 11/4/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.Text;

using OpenVP.Metadata.Loose;

namespace OpenVP.Metadata.Formatters
{
    public static class JSONFormatter
    {
        public static string Serialize(LooseObject obj)
        {
            StringBuilder str = new StringBuilder();

            SerializeObject(str, obj);

            return str.ToString();
        }
        
        private static void SerializeObject(StringBuilder str, LooseObject obj)
        {
            if (obj == null)
                str.Append("null");
            else if (obj is LooseDictionary)
                SerializeDictionary(str, (LooseDictionary) obj);
            else if (obj is LooseArray)
                SerializeArray(str, (LooseArray) obj);
            else if (obj is LooseString)
                SerializeString(str, (LooseString) obj);
            else if (obj is LooseNumber)
                str.Append(((LooseNumber) obj).Number.ToString());
        }

        private static void SerializeDictionary(StringBuilder str, LooseDictionary dict)
        {
            str.Append('{');

            List<string> keys = new List<string>(dict.Keys);
            keys.Sort();

            bool comma = false;
            
            foreach (string key in keys) {
                if (comma)
                    str.Append(',');

                comma = true;

                SerializeString(str, key);
                str.Append(':');

                SerializeObject(str, dict[key]);
            }

            str.Append('}');
        }

        private static void SerializeString(StringBuilder output, string str)
        {
            output.Append('"');

            foreach (char i in str) {
                if (i == '"') {
                    output.Append("\\\"");
                } else if (i < ' ' || i > '~') {
                    output.AppendFormat("\\u{0:x4}", (ushort) i);
                } else {
                    output.Append(i);
                }
            }

            output.Append('"');
        }

        private static void SerializeArray(StringBuilder str, LooseArray array)
        {
            str.Append('[');

            bool comma = false;
            
            foreach (LooseObject obj in array) {
                if (comma)
                    str.Append(',');

                comma = true;

                SerializeObject(str, obj);
            }
            
            str.Append(']');
        }
    }
}
