// LooseNumber.cs created with MonoDevelop
// User: chris at 5:12 PM 11/3/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace OpenVP.Metadata.Loose
{
    public class LooseNumber : LooseObject,
        IEquatable<LooseNumber>, IEquatable<float>, IEquatable<int>,
        IComparable<LooseNumber>, IComparable<float>, IComparable<int>
    {
        public float Number;
        
        public LooseNumber(float number)
        {
            this.Number = number;
        }

        public override float ToNumber()
        {
            return this.Number;
        }

        public override string ToString()
        {
            return this.Number.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is LooseNumber)
                return this.Equals((LooseNumber) obj);

            if (obj is float)
                return this.Equals((float) obj);

            if (obj is int)
                return this.Equals((int) obj);

            return false;
        }

        public override int GetHashCode()
        {
            return this.Number.GetHashCode();
        }

        public bool Equals(LooseNumber number)
        {
            return this.Number == number.Number;
        }

        public bool Equals(float number)
        {
            return this.Number == number;
        }

        public bool Equals(int number)
        {
            return this.Number == number;
        }

        public int CompareTo(LooseNumber number)
        {
            return this.Number.CompareTo(number.Number);
        }

        public int CompareTo(float number)
        {
            return this.Number.CompareTo(number);
        }

        public int CompareTo(int number)
        {
            return this.Number.CompareTo(number);
        }

        public static implicit operator float(LooseNumber number)
        {
            return number.Number;
        }

        public static implicit operator int(LooseNumber number)
        {
            return (int) number.Number;
        }

        public static implicit operator LooseNumber(float number)
        {
            return new LooseNumber(number);
        }
    }
}
