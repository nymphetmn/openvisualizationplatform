// Event.cs created with MonoDevelop
// User: chris at 3:08 PM 11/3/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

using OpenVP.Metadata.Loose;

namespace OpenVP.Metadata
{
    public class Event : MetadataObject
    {
        public int BeginTime { get; protected set; }

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
            this.BeginTime = (int) data["begin"].ToNumber();
            this.EndTime = (int) data["end"].ToNumber();
            
            this.Source = source;
        }
    }
}
