// Channel.cs created with MonoDevelop
// User: chris at 3:10 PM 11/3/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenVP.Metadata
{
    public abstract class Channel<T> : MetadataObject, IChannel where T : Event
    {
        protected abstract IEnumerable<T> EventBuffer { get; }
        
        public int Position { get; protected set; }

        private List<WeakReference> windows = new List<WeakReference>();

        private int maximumWindowSize;

        protected int MaximumWindowSize {
            get {
                return this.maximumWindowSize;
            }

            private set {
                if (value == this.maximumWindowSize)
                    return;

                this.maximumWindowSize = value;
                this.OnMaximumWindowSizeUpdated(value);
            }
        }

        public MetadataManager Manager { get; private set; }
        
        protected Channel(MetadataManager manager)
        {
            this.Manager = manager;
        }

        protected virtual int RequestWindowSize(int windowSize)
        {
            return windowSize;
        }

        protected virtual void OnMaximumWindowSizeUpdated(int newSize)
        {
        }

        public ChannelWindow<T> CreateWindow(int windowSize)
        {
            windowSize = this.RequestWindowSize(windowSize);
            
            ChannelWindow<T> window = new ChannelWindow<T>(this, windowSize);

            if (windowSize > this.MaximumWindowSize)
                this.MaximumWindowSize = windowSize;

            this.windows.Add(new WeakReference(window, true));

            return window;
        }

        internal void DestroyWindow(ChannelWindow<T> window)
        {
            WeakReference r = null;
            int max = 1;
            
            foreach (WeakReference i in this.windows) {
                if (i.Target == window) {
                    r = i;
                } else {
                    max = Math.Max(max, ((ChannelWindow<T>) i.Target).WindowSize);
                }
            }

            if (r != null) {
                this.MaximumWindowSize = max;
                this.windows.Remove(r);
            }
        }

        internal IEnumerable<T> GetEvents(int windowSize)
        {
            int begin = this.Position;
            int end = this.Position + windowSize - 1;

            return
                from x in this.EventBuffer
                where (x.EndTime >= begin || x.BeginTime <= end)
                select x;
        }

        public abstract void SeekTo(int position);

        public abstract void SeekAhead(int amount);
    }
}
