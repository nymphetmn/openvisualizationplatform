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
        private int begin;
        
        public int Begin {
            get { return this.begin; }
        }
        
        private int end;
        
        public int End {
            get { return this.end; }
        }
        
        public Event(int begin, int end) {
            if (begin < 0)
                throw new ArgumentOutOfRangeException("begin < 0");
            
            if (end < begin)
                throw new ArgumentOutOfRangeException("end < begin");
            
            this.begin = begin;
            this.end = end;
        }
        
        public Event(XElement e) : this((int) e.Attribute("begin"),
                                        (int) e.Attribute("end")) {
        }
    }
}
