using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.NodeGraph
{
    public class Bl : Blackboard
    {
        public Bl() :base()
        {
            this.title = "参数";

            addItemRequested += AddItemRequested;
        }

        private void AddItemRequested(Blackboard obj)
        {
            var gm = new GenericMenu();
            AddPropertyItems(gm);
            gm.ShowAsContext();
        }

        private void AddPropertyItems(GenericMenu gm)
        {
            gm.AddItem(new GUIContent("测试"), false, () => {
                this.Add(new BlField(null, "text", "typeText"));
            });
        }
    }

    public class BlField : BlackboardField
    {
        public BlField(Texture2D icon, string text, string typeText) : base(icon, text, typeText) {
            
        }

        public override void OnSelected()
        {
            base.OnSelected();
            Debug.Log("选中");
        }
    }
}
