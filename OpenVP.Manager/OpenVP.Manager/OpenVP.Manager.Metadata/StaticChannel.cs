// StaticChannel.cs created with MonoDevelop
// User: chris at 6:36 PM 7/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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
        
        public StaticChannel(XElement channel) : this(DeserializeEvents(channel)) {
        }
        
        private static IEnumerable<T> DeserializeEvents(XElement c) {
            foreach (XElement i in c.Elements("event")) {
                foreach (T e in DeserializeEvent(i)) {
                    yield return e;
                }
            }
        }
        
        private static IEnumerable<T> DeserializeEvent(XElement e) {
            string type = (string) e.Attribute("type");
            if (type == null)
                throw new ArgumentException("Event has no type.");
            
            Type t = Type.GetType(type);
            if (t == null)
                throw new ArgumentException("Unknown type: " + type);
            if (!typeof(T).IsAssignableFrom(t))
                throw new ArgumentException("Event type " + type + 
                                            " cannot be converted to channel type " + typeof(T).FullName);
            
            Event evnt = (Event) Activator.CreateInstance(t, e);
            
            return ProcessEvent(evnt);
        }
        
        private static IEnumerable<T> ProcessEvent(Event evnt) {
            IEventGenerator gen = evnt as IEventGenerator;
            if (gen != null) {
                if (gen.GeneratorMode != GeneratorMode.Discard) {
                    if (typeof(T).IsAssignableFrom(gen.GetType()))
                        yield return (T) gen;
                    else if (gen.GeneratorMode == GeneratorMode.Retain)
                        throw new ArgumentException("Generator cannot be retained.");
                }
                
                foreach (Event e in gen) {
                    foreach (T i in ProcessEvent(e)) {
                        yield return i;
                    }
                }
            } else if (typeof(T).IsAssignableFrom(gen.GetType())) {
                yield return (T) evnt;
            } else {
                throw new ArgumentException("Event type is not compatible with this channel.");
            }
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
