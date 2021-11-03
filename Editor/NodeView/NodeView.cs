using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityLib.Graph;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using UnityEngine;

namespace UnityLib.GraphEditor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomViewAttribute : Attribute
    {
        public readonly Type type;
        public CustomViewAttribute(Type type)
        {
            this.type = type;
        }
    }

    public class NodeView : Node
    {
        public new NodeData userData { get; set; }
        protected readonly SerializedObject serializedObject;

        public NodeView(NodeData data)
        {
            this.userData = data;
            this.serializedObject = new SerializedObject(data);

            this.viewDataKey = data.guid;
            this.title = data.GetType().Name;
            OnEnable();

            GUI();
            this.style.width = 320;
        }

        private void GUI()
        {
            this.mainContainer.Add(new IMGUIContainer(() =>
            {
                OnInspectorGUI();

            }));
            OnPortGUI();
        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnPortGUI()
        {
            var dty = userData.GetType();
            var fields = dty.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var input = field.GetCustomAttribute<InputAttribute>();
                var output = field.GetCustomAttribute<OutputAttribute>();
                if (input != null)
                {
                    var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, field.FieldType);
                    var name = string.IsNullOrEmpty(input.portName) ? field.Name : input.portName;
                    port.portName = name;
                    port.name = name;
                    this.inputContainer.Add(port);
                }

                if (output != null)
                {
                    var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, field.FieldType);
                    var name = string.IsNullOrEmpty(output.portName) ? field.Name : output.portName;
                    port.portName = name;
                    port.name = name;
                    this.outputContainer.Add(port);
                }
            }
            RefreshPorts();
        }

        protected virtual void OnInspectorGUI()
        {
            var iterator = serializedObject.FindProperty("m_Script");
            while (iterator.NextVisible(true))
            {
                EditorGUILayout.PropertyField(iterator);
            }

            serializedObject.ApplyModifiedProperties();
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            if(userData != null)
            {
                userData.Position = newPos.position;
            }
        }
    }
}
