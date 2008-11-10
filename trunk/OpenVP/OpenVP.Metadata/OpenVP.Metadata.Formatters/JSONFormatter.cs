// JSONFormatter.cs created with MonoDevelop
// User: chris at 1:32 AM 11/4/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using OpenVP.Metadata.Loose;

namespace OpenVP.Metadata.Formatters
{
    public static class JSONFormatter
    {
        private static readonly Dictionary<char, char> stringCharMap;

        static JSONFormatter()
        {
            stringCharMap = new Dictionary<char, char>();

            stringCharMap['b'] = '\b';
            stringCharMap['f'] = '\f';
            stringCharMap['n'] = '\n';
            stringCharMap['r'] = '\r';
            stringCharMap['t'] = '\t';
        }
        
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

        private static void EatWhitespace(TextReader reader)
        {
            int i;

            while ((i = reader.Peek()) != -1 && !char.IsWhiteSpace((char) i))
                reader.Read();
        }

        private static string ReadKeyword(TextReader reader)
        {
            StringBuilder sb = new StringBuilder();

            int ci;
            while ((ci = reader.Peek()) != -1) {
                char c = (char) ci;

                if (!char.IsLetter(c))
                    break;

                sb.Append(c);
                reader.Read();
            }

            return sb.ToString();
        }

        private static LooseObject DeserializeObject(TextReader reader)
        {
            EatWhitespace(reader);

            int ci = reader.Peek();

            if (ci == -1)
                throw new ArgumentException("reader: Premature end of stream.");

            char c = (char) ci;

            switch (c) {
            case '"':
                return DeserializeString(reader);

            case '[':
                return DeserializeArray(reader);

            case '{':
                return DeserializeDictionary(reader);
            }

            if (char.IsLetter(c)) {
                string keyword = ReadKeyword(reader);

                switch (keyword) {
                case "null":
                    return null;

                case "true":
                    return 1;

                case "false":
                    return 0;
                }
                
                throw new ArgumentException("Encountered unrecognized keyword: " + keyword);
            }

            // TODO: number
            throw new NotImplementedException("Number parsing.");
        }

        private static LooseDictionary DeserializeDictionary(TextReader reader)
        {
            if (reader.Read() != '{')
                throw new ArgumentException("reader: Not a dictionary.");

            for (;;) {
                EatWhitespace(reader);
            }
        }

        private static LooseArray DeserializeArray(TextReader reader)
        {
            if (reader.Read() != '[')
                throw new ArgumentException("reader: Does not contain a serialized array.");

            LooseArray array = new LooseArray();

            bool first = true;
            for (;;) {
                EatWhitespace(reader);

                if (reader.Peek() == ']')
                    break;

                if (!first) {
                    if (reader.Read() != ',')
                        throw new ArgumentException("reader: Malformed array.");

                    first = false;
                }

                array.Add(DeserializeObject(reader));
            }

            reader.Read();

            return array;
        }

        private static LooseString DeserializeString(TextReader reader)
        {
            if (reader.Read() != '"')
                throw new ArgumentException("reader: Does not represent a serialized string.");

            StringBuilder sb = new StringBuilder();

            bool slash = false;
            
            for (;;) {
                int ci = reader.Read();

                if (ci == -1)
                    throw new ArgumentException("reader: Unterminated string.");

                char c = (char) ci;

                if (slash) {
                    char esc;
                    
                    if (c == 'u') {
                        char[] buffer = new char[4];
                        if (reader.Read(buffer, 0, 4) != 4)
                            throw new ArgumentException("reader: Unterminated string.");

                        ushort cbuf;
                        if (!ushort.TryParse(new string(buffer), NumberStyles.AllowHexSpecifier, null, out cbuf))
                            throw new ArgumentException("reader: Invalid escape sequence.");

                        sb.Append((char) cbuf);
                    } else if (stringCharMap.TryGetValue(c, out esc)) {
                        sb.Append(esc);
                    } else {
                        sb.Append(c);
                    }
                    
                    slash = false;
                } else if (c == '"') {
                    break;
                } else if (c == '\\') {
                    slash = true;
                } else {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}
