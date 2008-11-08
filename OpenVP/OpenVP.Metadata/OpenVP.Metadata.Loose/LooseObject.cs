// LooseObject.cs created with MonoDevelop
// User: chris at 5:11 PM 11/3/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace OpenVP.Metadata.Loose
{
    public abstract class LooseObject
    {
        protected LooseObject()
        {
        }

        public override string ToString()
        {
            return null;
        }

        public virtual float ToNumber()
        {
            return float.NaN;
        }

        // We need these here so we can assign a string/float/int directly to
        // a LooseObject reference variable.  We defer to the conversions in
        // derived classes directly to avoid duplicate code.
        public static implicit operator LooseObject(string str)
        {
            return (LooseString) str;
        }

        public static implicit operator LooseObject(float f)
        {
            return (LooseNumber) f;
        }
    }
}
