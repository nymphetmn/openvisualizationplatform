// MetadataObject.cs created with MonoDevelop
// User: chris at 3:05 PM 11/3/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

using OpenVP.Metadata.Loose;

namespace OpenVP.Metadata
{
    public abstract class MetadataObject
    {
        public LooseDictionary LooseData { get; protected set; }
        
        public string Name { get; protected set; }
        
        protected MetadataObject() : this((string) null)
        {
        }

        protected MetadataObject(string name)
        {
            this.LooseData = new LooseDictionary();
            this.Name = name;
        }

        protected MetadataObject(LooseDictionary data)
        {
            this.LooseData = data;
            this.Name = data["name"].ToString();
        }
    }
}
