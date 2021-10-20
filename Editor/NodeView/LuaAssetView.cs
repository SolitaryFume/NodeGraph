using UnityLib.Graph;
using UnityEngine;
using XLua;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;

namespace UnityLib.GraphEditor
{
    [CustomView(typeof(LuaForAsset))]
    public class LuaAssetView : NodeView
    {
        private LuaTable table;

        public LuaAssetView(NodeData data) : base(data)
        {

        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpDateLua();
        }

        protected override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("刷新更新"))
            {
                UpDateLua();
            }
        }

        private void UpDateLua()
        {
            var luaasset = userData as LuaForAsset;
            if (luaasset.luaFile != null)
            {
                table = NodeGraphLuaEnv.Parse(luaasset.luaFile.text);
                OnPortGUI();
            }
        }

        private List<Port> keys = new List<Port>();
        protected override void OnPortGUI()
        {
            base.OnPortGUI();
            if(table==null)
                return;
            foreach (var port in keys)
            {
                this.outputContainer.Remove(port);
            }
            keys.Clear();

            foreach (var key in table.GetKeys())
            {
                var value = table[key];
                var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, value.GetType());
                var name = $"{key.ToString()}[{value.GetType()}]";
                port.portName = name;
                port.name = name;
                this.outputContainer.Insert(0,port);
                keys.Add(port);
            }
            this.RefreshExpandedState();
        }
    }
}
