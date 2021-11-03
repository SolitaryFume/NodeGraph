using System.Text;
using System.Reflection;
using System;
using System.Linq;

namespace UnityEditor.NodeGraph
{
    public abstract class FunctionNode : AbstracMasterNode,IGeneratesBodyCode
    {
        protected class DynamicSlot { }
        protected class SlotAttribute : Attribute
        {
            protected string functionName = string.Empty;

            public int slotId { get; private set; }
            public SlotValueLuaType valueType { get; private set; }
            public bool hidden { get; private set; }

            public SlotAttribute(int slotid, SlotValueLuaType valuetype, bool hidden = false)
            {
                slotId = slotid;
                valueType = valuetype;
                this.hidden = hidden;
            }
        }

        public const int EntranceId = -1;
        public const int ExitId = -2;
        public const string kEntranceName = "Entrance";
        public const string kExitName = "Exit";

        protected string funName = string.Empty;
        public void GenerateNodeCode(StringBuilder luaStringBuilder,bool getNext = false)
        {
            var fun = GetFunction();
            if (fun == null)
                throw new Exception("");
            var call = fun.Name + "(";
            var args = fun.GetParameters();

            for (int i = 0; i < args.Length; i++)
            {
                if (!args[i].IsOut)
                { 
                    if (i != 0)
                        call += ",";
                    var arg = GetVariableNameForSlot(i);
                    call += arg;
                }
            }
            call += ")";

            if(getNext)
                luaStringBuilder.AppendLine(call);
            else
                luaStringBuilder.Append(call);

            if (getNext == true)
            {
                GetNextNode(luaStringBuilder);
            }
        }

        private void GetNextNode(StringBuilder luaStringBuilder)
        {
            var slot = FindSlot<AbstractSolt>(ExitId);
            if (slot == null)
                throw new Exception("no find exit node");
            var conSolt = Owner.GetConnectSolt(slot);
            if (conSolt != null && conSolt.Owner is IGeneratesBodyCode nextNode)
            {
                nextNode.GenerateNodeCode(luaStringBuilder,true);
            }
        }

        public override string GetVariableNameForSlot(int slotId)
        {
            var slot = FindSlot<AbstractSolt>(slotId);
            if (slot == null)
                throw new Exception("");
            var conSlot = Owner.GetConnectSolt(slot);
            if (conSlot == null)
                return slot.GetDefaultValue();
            else if (conSlot.Owner is IGeneratesBodyCode conGenerate)
            { 
                var temp = new StringBuilder();
                conGenerate.GenerateNodeCode(temp);
                return temp.ToString();
            }
            return String.Empty;
        }


        protected override void InitPort()
        {
            AddSlot(new FlowSlot(EntranceId, kEntranceName, SlotType.Input));
            AddSlot(new FlowSlot(ExitId, kExitName, SlotType.Output));

            var fun = GetFunction();
            if (fun == null)
                throw new Exception("no find function !");
            ParameterInfo[] args = fun.GetParameters();
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var slotAtt = arg.GetCustomAttribute<SlotAttribute>();
                if (slotAtt == null)
                    throw new Exception("no add SlotAttribute");
                var solt = LuaSlot.CrateSlot(i,arg.Name,arg.IsOut?SlotType.Output:SlotType.Input,slotAtt.valueType,false);
                AddSlot(solt);
            }  
        }

        protected virtual MethodInfo GetFunction()
        {
            if (!string.IsNullOrEmpty(funName)) 
                return GetType().GetMethod(funName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            throw new Exception("function name is null or empty !");
        }
        
    }
}