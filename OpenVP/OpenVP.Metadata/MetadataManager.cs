// MetadataManager.cs created with MonoDevelop
// User: chris at 10:38 PM 11/3/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OpenVP.Metadata
{
    public class MetadataManager : IEnumerable<IChannel>
    {
        private List<IChannel> channels = new List<IChannel>();

        private Dictionary<string, IChannel> channelNameMap = new Dictionary<string, IChannel>();
        
        public MetadataManager()
        {
        }

        public void AddChannel<T>(Channel<T> channel) where T : Event
        {
            if (channel == null)
                throw new ArgumentNullException("channel");

            if (this.channels.Contains(channel))
                return;

            if (channel.Name != null) {
                if (this.channelNameMap.ContainsKey(channel.Name))
                    throw new ArgumentException("Channel with name \"" + channel.Name + "\" already exists.");

                this.channelNameMap[channel.Name] = channel;
            }

            this.channels.Add(channel);
        }

        public IChannel GetChannelByName(string name)
        {
            IChannel channel;

            if (!this.channelNameMap.TryGetValue(name, out channel))
                return null;

            return channel;
        }

        public IEnumerable<Channel<T>> GetChannelsByType<T>() where T : Event
        {
            return this.channels.OfType<Channel<T>>();
        }

        IEnumerator<IChannel> IEnumerable<IChannel>.GetEnumerator()
        {
            return this.channels.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.channels.GetEnumerator();
        }
    }
}
