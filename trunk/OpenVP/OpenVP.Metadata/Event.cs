// Event.cs created with MonoDevelop
// User: chris at 3:08 PM 11/3/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Reflection;

using OpenVP.Metadata.Loose;

namespace OpenVP.Metadata
{
    public class Event : MetadataObject
    {
        [LooseSource("begin")]
        public int BeginTime { get; protected set; }

        [LooseSource("end")]
        public int EndTime { get; protected set; }

        public IChannel Source { get; private set; }
        
        public Event(IChannel source, LooseDictionary data, int beginTime, int endTime) : base(data)
        {
            this.BeginTime = beginTime;
            this.EndTime = endTime;
            
            this.Source = source;
        }

        protected Event(IChannel source, LooseDictionary data) : base(data)
        {
            this.SetLoose();
            this.Source = source;
        }

        private void SetLoose()
        {
            Type type = this.GetType();

            foreach (MemberInfo member in type.GetMembers(BindingFlags.Instance |
                                                          BindingFlags.Public |
                                                          BindingFlags.NonPublic)) {
                FieldInfo field;
                PropertyInfo property;

                field = member as FieldInfo;
                property = member as PropertyInfo;

                if (field == null && property == null)
                    continue;
                
                object[] attrs = member.GetCustomAttributes(typeof(LooseSourceAttribute), false);
                LooseSourceAttribute attr;

                if (attrs.Length == 0)
                    continue;

                attr = (LooseSourceAttribute) attrs[0];

                LooseObject obj = this.LooseData[attr.Path];
                if (obj == null)
                    continue;
                
                object val;
                
                if (!TryConvert(field != null ? field.FieldType : property.PropertyType, obj, out val))
                    continue;

                if (field != null)
                    field.SetValue(this, val);
                else
                    property.SetValue(this, val, null);
            }
        }

        private static bool TryConvert(Type type, LooseObject obj, out object output)
        {
            if (type == typeof(int)) {
                output = (int) obj.ToNumber();
                return true;
            }

            if (type == typeof(float)) {
                output = obj.ToNumber();
                return true;
            }

            if (type == typeof(string)) {
                output = obj.ToString();
                return true;
            }

            if (typeof(LooseObject).IsAssignableFrom(type)) {
                output = obj;
                return true;
            }

            output = null;
            return false;
        }
    }
}
