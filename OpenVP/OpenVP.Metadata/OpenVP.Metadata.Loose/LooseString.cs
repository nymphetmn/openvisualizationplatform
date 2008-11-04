// LooseString.cs created with MonoDevelop
// User: chris at 5:14 PM 11/3/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace OpenVP.Metadata.Loose
{
    public class LooseString : LooseObject, IEquatable<string>, IComparable<string>
    {
        private string str;

        public string String {
            get {
                return this.str;
            }

            set {
                this.str = (value == null) ? "" : value;
            }
        }
        
        public LooseString(string str)
        {
            this.String = str;
        }

        public override string ToString()
        {
            return this.String;
        }

        public override float ToNumber()
        {
            float f;

            if (!float.TryParse(this.String, out f))
                return float.NaN;

            return f;
        }

        public override bool Equals(object obj)
        {
            if (obj is string)
                return this.Equals((string) obj);

            return false;
        }

        public bool Equals(string str)
        {
            return this.String.Equals(str);
        }

        public override int GetHashCode()
        {
            return this.String.GetHashCode();
        }

        public int CompareTo(string str)
        {
            return this.String.CompareTo(str);
        }
        
        public static implicit operator string(LooseString str)
        {
            return str.String;
        }

        public static implicit operator LooseString(string str)
        {
            return new LooseString(str);
        }
    }
}
