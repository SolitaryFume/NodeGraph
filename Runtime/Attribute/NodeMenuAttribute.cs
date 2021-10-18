using System;

namespace UnityLib.Graph
{
    public class NodeMenuAttribute : Attribute
    { 
        public string path { get; set; }

        public NodeMenuAttribute(string path)
        {
            this.path = path;
        }
    }
}