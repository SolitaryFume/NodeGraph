using System;
using System.Reflection;

namespace UnityEditor.NodeGraph
{
    [NodeMenu("Test")]
    [Serializable]
    public class TestNode : FunctionNode
    {
        public const int OutputSlotId = 0;
        public const int InputSlotId = 1;
        private const string kOutputSlotName = "Out";
        private const string InputSlotName = "In";

        [TextureControl] public string test { get; set; }

        public TestNode()
        {
            name = "Test";
            InitPort();
        }

        //protected override void InitPort()
        //{
        //    this.AddSlot(new NumberSlot(OutputSlotId, kOutputSlotName, SlotType.Output));
        //    this.AddSlot(new NumberSlot(InputSlotId,"Number", SlotType.Input));
        //    this.AddSlot(new StringSlot(2, "String", SlotType.Input,false,string.Empty));
        //    this.AddSlot(new SlotBool(3, "Bool", SlotType.Input, false));
        //}

        private string getPath([Slot(0,SlotValueLuaType.Boolean)] bool boolean,
            [Slot(1,SlotValueLuaType.Nmuber)] int number,
            [Slot(2,SlotValueLuaType.String)] out string Out)
        {
            Out = default;
            return string.Empty;
        }

        protected override MethodInfo GetFunction()
        {
            return GetType().GetMethod(nameof(getPath), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }
    }
}