using System;
using System.Text;

namespace UnityEditor.NodeGraph
{
    [NodeMenu("Entrance")]
    [Serializable]
    public class EntranceNode : AbstracMasterNode,IGeneratesBodyCode
    {
        public const int EntranceId = 0;
        public const string kEntranceName = "Entrance";

        public EntranceNode() {
            name = "Entrance";
            InitPort();
        }

        public void GenerateNodeCode(StringBuilder luaStringBuilder, bool getNext = false)
        {
            luaStringBuilder.AppendFormat("function {0}()\n",name);
            var toSlot = Owner.GetConnectSolt(FindSlot<FlowSlot>(EntranceId));
            if (toSlot != null)
            { 
                if(toSlot.Owner is IGeneratesBodyCode generater)
                {
                    generater.GenerateNodeCode(luaStringBuilder,true);
                }
            }
            
            luaStringBuilder.AppendLine("end");
        }

        protected override void InitPort()
        {
            AddSlot(new FlowSlot(EntranceId,kEntranceName,SlotType.Output));
        }
    }

    //[NodeMenu("Exit")]
    //[Serializable]
    //public class ExitNode : AbstracMasterNode
    //{
    //    public const int ExitId = 0;
    //    public const string kExitName = "Exit";

    //    public ExitNode()
    //    {
    //        name = "Exit";
    //        InitPort();
    //    }

    //    protected override void InitPort()
    //    {
    //        AddSlot(new FlowSlot(ExitId, kExitName, SlotType.Input));
    //    }

    //    public AbstractSolt GetConnectSolt(int id)
    //    {
    //        var solt = FindSlot<AbstractSolt>(id);
    //        return Owner.GetConnectSolt(solt);
    //    }
    //}
}