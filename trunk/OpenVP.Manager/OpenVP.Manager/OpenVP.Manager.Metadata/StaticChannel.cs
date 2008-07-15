// StaticChannel.cs created with MonoDevelop
// User: chris at 6:36 PM 7/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OpenVP.Manager.Metadata {
    public class StaticChannel<T> : IChannel<T> where T : Event {
        public IEnumerable<T> this[int time] {
            get { return this.eventTimeline[time]; }
        }
        
        private List<T> events;
        
        private List<T>[] eventTimeline;
        
        public int LookAhead {
            get { return this.eventTimeline.Length - 1; }
        }
        
        public StaticChannel(IEnumerable<T> source) {
            this.events = new List<T>(source);
            
            this.CreateTimeline();
        }
        
        private void CreateTimeline() {
            this.events.Sort((a, b) => a.Begin - b.Begin);
            
            int last = 0;
            
            foreach (Event i in this.events) {
                if (i.End > last) {
                    last = i.End;
                }
            }
            
            this.eventTimeline = new List<T>[last + 1];
            
            foreach (T i in this.events) {
                for (int pos = i.Begin; pos <= i.End; pos++) {
                    this.AddToTimeline(pos, i);
                }
            }
        }
        
        private void AddToTimeline(int time, T ev) {
            if (this.eventTimeline[time] == null)
                this.eventTimeline[time] = new List<T>();
            
            this.eventTimeline[time].Add(ev);
        }
        
        public IEnumerator<T> GetEnumerator() {
            return this.events.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
        
        public IEnumerable<T> GetEventsPresentAt(int time) {
            return this[time];
        }
        
        public IEnumerable<T> GetEventsStartingAt(int time) {
            return this[time].Where(x => x.Begin == time);
        }
        
        public IEnumerable<T> GetEventsEndingAt(int time) {
            return this[time].Where(x => x.End == time);
        }
    }
}
