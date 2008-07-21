// Event.cs created with MonoDevelop
// User: chris at 6:33 PM 7/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Linq;
using System.Xml.Linq;

namespace OpenVP.Manager.Metadata {
    public class Event {
        public int Begin { get; private set; }
        
        public int End { get; private set; }
        
        public Event(int begin, int end) {
            if (begin < 0)
                throw new ArgumentOutOfRangeException("begin < 0");
            
            if (end < begin)
                throw new ArgumentOutOfRangeException("end < begin");
            
            this.Begin = begin;
            this.End = end;
        }
        
        public Event(XElement e) : this((int) e.Attribute("begin"),
                                        (int) e.Attribute("end")) {
        }
    }
}
