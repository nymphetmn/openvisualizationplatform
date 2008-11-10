// ChannelWindow.cs created with MonoDevelop
// User: chris at 3:27 PM 11/3/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;

namespace OpenVP.Metadata
{
    public class ChannelWindow<T> : IDisposable where T : Event
    {
        public int WindowSize { get; private set; }

        private Channel<T> channel;

        public Channel<T> Channel {
            get {
                if (this.channel == null)
                    throw new ObjectDisposedException("this");

                return this.channel;
            }
        }
        
        internal ChannelWindow(Channel<T> channel, int windowSize)
        {
            if (channel == null)
                throw new ArgumentNullException("channel");

            if (windowSize < 1)
                throw new ArgumentOutOfRangeException("windowSize < 1");
            
            this.WindowSize = windowSize;
            this.channel = channel;
        }

        public void Dispose()
        {
            this.Channel.DestroyWindow(this);
            this.channel = null;

            GC.SuppressFinalize(this);
        }

        ~ChannelWindow()
        {
            this.Dispose();
        }

        public IEnumerable<T> GetEvents()
        {
            return this.Channel.GetEvents(this.WindowSize);
        }

        public IEnumerable<T> GetEvents(int windowSize)
        {
            if (windowSize < 1 || windowSize > this.WindowSize)
                throw new ArgumentOutOfRangeException("windowSize must be >= 1 and <= WindowSize");

            return this.Channel.GetEvents(windowSize);
        }
    }
}
