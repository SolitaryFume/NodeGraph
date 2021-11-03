using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Linq;

namespace UnityEditor.NodeGraph
{
    public class NodeMenuProvider : ScriptableObject, ISearchWindowProvider
    {
        private List<SearchTreeEntry> entries;

        public delegate bool SerchMenuWindowOnSelectEntryDelegate(SearchTreeEntry searchTreeEntry, SearchWindowContext context);
        public SerchMenuWindowOnSelectEntryDelegate OnSelectEntryHandler;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            if (entries == null)
            { 
                entries = new List<SearchTreeEntry>();
                entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));
                //entries.Add(new SearchTreeEntry(new GUIContent("Test")) { level = 1, userData = typeof(TestNode) });
                //entries.Add(new SearchTreeEntry(new GUIContent("dofile")) { level = 1, userData = typeof(DofileNode) });
                //entries.Add(new SearchTreeEntry(new GUIContent("Entrance")) { level = 1, userData = typeof(EntranceNode) });
                //entries.Add(new SearchTreeEntry(new GUIContent("Exit")) { level = 1, userData = typeof(ExitNode) });

                var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(ass => ass.GetTypes()).Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttributes(typeof(NodeMenuAttribute), false).Length > 0);
                foreach (var type in types)
                {
                    var attr = type.GetCustomAttributes(typeof(NodeMenuAttribute), false)[0] as NodeMenuAttribute;
                    entries.Add(new SearchTreeEntry(new GUIContent(attr.m_path)) { level = 1, userData = type });
                }

            }    
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (OnSelectEntryHandler == null)
            {
                return false;
            }
            return OnSelectEntryHandler(searchTreeEntry, context);
        }
    }
}
