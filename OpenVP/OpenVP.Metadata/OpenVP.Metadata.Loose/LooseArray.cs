// LooseArray.cs created with MonoDevelop
// User: chris at 1:38 AM 11/4/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenVP.Metadata.Loose
{
    public class LooseArray : LooseObject, IList<LooseObject>
    {
        private List<LooseObject> list = new List<LooseObject>();

        public LooseObject this[int index] {
            get { return list[index]; }
            set { list[index] = value; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public int Count {
            get { return list.Count; }
        }

        public LooseArray()
        {
        }

        public void Add(LooseObject item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public void CopyTo(LooseObject[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public bool Contains(LooseObject item)
        {
            return list.Contains(item);
        }

        public IEnumerator<LooseObject> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(LooseObject item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, LooseObject item)
        {
            list.Insert(index, item);
        }

        public bool Remove(LooseObject item)
        {
            return list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
