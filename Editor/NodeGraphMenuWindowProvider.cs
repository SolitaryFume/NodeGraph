using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityLib.Graph;
using System;
using System.Linq;
using System.Reflection;

namespace UnityLib.GraphEditor
{
    public class NodeEntry:IComparable<NodeEntry>
    {
        internal string menupath;
        internal Type type;

        public int CompareTo(NodeEntry other)
        {
            return other.menupath.CompareTo(other.menupath);
        }
    }

    public sealed class NodeGraphMenuWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        public delegate bool SerchMenuWindowOnSelectEntryDelegate(SearchTreeEntry searchTreeEntry, SearchWindowContext context);
        public SerchMenuWindowOnSelectEntryDelegate OnSelectEntryHandler;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            var list = new List<NodeEntry>();
            
            var allnode = typeof(NodeData).Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(NodeData))).ToList();
            //Debug.LogError(allnode.Count);
            foreach (var node in allnode)
            {
                if (node.GetCustomAttributes(typeof(NodeMenuAttribute), false) is NodeMenuAttribute[] attrs && attrs.Length > 0)
                {
                    var noedmenu = (NodeMenuAttribute)attrs[0];
                    list.Add(new NodeEntry() {
                        menupath = noedmenu.path,
                        type = node
                    });
                }
            }
            list.Sort((a, b) => { 
                return a.menupath.CompareTo(b.menupath);
            });

            entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));

            foreach (var item in list)
            {
                //tood 排序分组
                entries.Add(new SearchTreeEntry(new GUIContent(item.menupath)) { level = 1, userData = item.type });
            }
            

            //entries.Add(new SearchTreeGroupEntry(new GUIContent("Value")) { level = 1});
            //entries.Add(new SearchTreeEntry(new GUIContent(nameof(LuaTable))) { level = 2, userData = typeof(LuaTable) });
            //entries.Add(new SearchTreeGroupEntry(new GUIContent("T")) { level = 1 });
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
