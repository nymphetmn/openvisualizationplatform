// LooseDictionary.cs created with MonoDevelop
// User: chris at 5:16 PM 11/3/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections;
using System.Collections.Generic;

using LooseMap = System.Collections.Generic.Dictionary<string, OpenVP.Metadata.Loose.LooseObject>;
using ILooseMap = System.Collections.Generic.IDictionary<string, OpenVP.Metadata.Loose.LooseObject>;
using ILooseCollection = System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, OpenVP.Metadata.Loose.LooseObject>>;

namespace OpenVP.Metadata.Loose
{
    public class LooseDictionary : LooseObject, ILooseMap
    {
        public LooseObject this[string key] {
            get {
                LooseDictionary dict = this;
                ResolveKey(ref key, ref dict);

                return dict.GetFlat(key);
            }

            set {
                LooseDictionary dict = this;
                ResolveKey(ref key, ref dict);

                if (value == null)
                    dict.Remove(key);
                else
                    dict.dictionary[key] = value;
            }
        }
        
        private LooseMap dictionary = new LooseMap();

        public int Count {
            get { return this.dictionary.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public ICollection<string> Keys {
            get { return this.dictionary.Keys; }
        }

        public ICollection<LooseObject> Values {
            get { return this.dictionary.Values; }
        }

        public LooseDictionary()
        {
        }

        private LooseObject GetFlat(string key)
        {
            LooseObject obj;

            if (!this.dictionary.TryGetValue(key, out obj))
                return null;

            return obj;
        }

        private static void ResolveKey(ref string key, ref LooseDictionary dict)
        {
            string[] parts = key.Split('.');

            LooseObject obj;
            
            for (int i = 0; i < parts.Length - 1; i++) {
                if (!dict.TryGetValue(parts[i], out obj))
                    throw new ArgumentException("key: Contains non-existant path.");

                dict = obj as LooseDictionary;
                
                if (dict == null)
                    throw new ArgumentException("key: Contains non-dictionary in path.");
            }

            key = parts[parts.Length - 1];
        }

        #region interfaces
        public void Add(string key, LooseObject value)
        {
            this.dictionary.Add(key, value);
        }
        
        void ILooseCollection.Add(KeyValuePair<string, LooseObject> item)
        {
            ((ILooseCollection) this.dictionary).Add(item);
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, LooseObject> item)
        {
            return ((IDictionary<string, LooseObject>) this.dictionary).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return this.dictionary.ContainsKey(key);
        }

        void ILooseCollection.CopyTo(KeyValuePair<string, LooseObject>[] array, int arrayIndex)
        {
            ((ILooseCollection) this.dictionary).CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) this.dictionary).GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, LooseObject>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        bool ILooseCollection.Remove(KeyValuePair<string, LooseObject> item)
        {
            return ((ILooseCollection) this.dictionary).Remove(item);
        }

        public bool Remove(string key)
        {
            return this.dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out LooseObject obj)
        {
            return this.dictionary.TryGetValue(key, out obj);
        }
        #endregion
    }
}
