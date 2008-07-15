// IEventGenerator.cs created with MonoDevelop
// User: chris at 9:11 PM 7/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;

namespace OpenVP.Manager.Metadata {
    public interface IEventGenerator<T> : IEnumerable<T> where T : Event {
        GeneratorMode GeneratorMode { get; }
    }
    
    public enum GeneratorMode {
        Discard,
        Retain,
        RetainIfPossible
    }
}
