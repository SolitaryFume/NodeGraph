using System;
using System.Text;
using UnityEngine;

namespace UnityEditor.NodeGraph.Lua
{
    [NodeMenu("Flow/if_else")]
    [Serializable]
    public class Lua_If: AbstractNode,IGeneratesBodyCode
    {
        public const int EntranceId = -1;
        public const string kEntranceName = "Entrance";
        
        public const int If = -2;
        public const string kIf = "If";

        public const int Else = -3;
        public const string kElse = "Else";

        public const int ConditionId = 1;
        public const string KCondition = "Condition";

        public Lua_If()
        {
            name = "if_else";
            InitPort();
        }

        public void GenerateNodeCode(StringBuilder luaStringBuilder, bool getNext = false)
        {
            luaStringBuilder.AppendFormat("if({0}) then\n{1}\nelse\n{2}\nend", GetVariableNameForSlot(ConditionId), GetCodeBlock(If), GetCodeBlock(Else));
        }

        private string GetCodeBlock(int flowId)
        {
            var luaStringBuilder = new StringBuilder();

            var slot = FindSlot<AbstractSolt>(flowId);
            if (slot == null)
                throw new Exception("no find exit node");
            var conSolt = Owner.GetConnectSolt(slot);
            if (conSolt != null && conSolt.Owner is IGeneratesBodyCode nextNode)
            {
                nextNode.GenerateNodeCode(luaStringBuilder, true);
            }
            return luaStringBuilder.ToString();
        }

        private void GetNextNode(StringBuilder luaStringBuilder = null,int folwId = 0)
        {
            var slot = FindSlot<AbstractSolt>(folwId);
            if (slot == null)
                throw new Exception("no find exit node");
            var conSolt = Owner.GetConnectSolt(slot);
            if (conSolt != null && conSolt.Owner is IGeneratesBodyCode nextNode)
            {
                nextNode.GenerateNodeCode(luaStringBuilder, true);
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
            AddSlot(new FlowSlot(If, kIf, SlotType.Output));
            AddSlot(new FlowSlot(Else, kElse, SlotType.Output));
            AddSlot(new SlotBool(ConditionId, KCondition, SlotType.Input));
        }
    }


    public abstract class Variable<T> : AbstractNode, IGeneratesBodyCode
    {
        [SerializeField] protected T m_value;
        public virtual T Value { get => m_value; set => m_value = value; }
        public abstract void GenerateNodeCode(StringBuilder luaStringBuilder, bool getNext = false);
    }

    //public class Variable_String : Variable<string>
    //{
    //    [TextureControl] public override string Value { get => base.Value; set => base.Value = value; }
    //    public override void GenerateNodeCode(StringBuilder luaStringBuilder, bool getNext = false)
    //    {
    //        //luaStringBuilder.Insert(0, $"local variable = {Value}");
    //        luaStringBuilder.Append()
    //    }
    //}
}