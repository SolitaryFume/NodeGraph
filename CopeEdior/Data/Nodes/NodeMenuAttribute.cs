using System;

namespace UnityEditor.NodeGraph
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeMenuAttribute : Attribute
    {
        public string m_path;

        public NodeMenuAttribute(string path)
        {
            this.m_path = path;
        }
    }
}