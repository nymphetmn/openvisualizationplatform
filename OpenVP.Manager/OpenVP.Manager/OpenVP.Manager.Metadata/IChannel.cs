// IChannel.cs created with MonoDevelop
// User: chris at 6:32 PM 7/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;

namespace OpenVP.Manager.Metadata {
    public interface IChannel<T> : IEnumerable<T> where T : Event {
        IEnumerable<T> this[int time] { get; }
        
        //int CurrentPosition { get; }
        int LookAhead { get; }
        
        IEnumerable<T> GetEventsPresentAt(int time);
        IEnumerable<T> GetEventsStartingAt(int time);
        IEnumerable<T> GetEventsEndingAt(int time);
    }
}
