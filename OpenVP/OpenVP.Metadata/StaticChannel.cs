// StaticChannel.cs
//
//  Copyright (C) 2008 [name of author]
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenVP.Metadata
{
    public class StaticChannel<T> : Channel<T> where T : Event
    {
        private List<T> eventBuffer;
        
        public StaticChannel(MetadataManager manager, IEnumerable<T> events) : base(manager)
        {
            this.eventBuffer = new List<T>(events);
            this.eventBuffer.Sort(delegate(T a, T b) {
                return a.BeginTime - b.BeginTime;
            });
        }

        protected internal override IEnumerable<T> GetEvents(int windowSize)
        {
            int begin = this.Position;
            int end = this.Position + windowSize - 1;
            
            return
                from x in this.eventBuffer
                where (x.EndTime >= begin || x.BeginTime <= end)
                select x;
        }

        public override void SeekAhead(int amount)
        {
            this.Position += amount;
        }

        public override void SeekTo(int position)
        {
            this.Position = position;
        }
    }
}
