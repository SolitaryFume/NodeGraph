namespace UnityEditor.NodeGraph
{
    public abstract class AbstracMasterNode : AbstractNode
    {
        public override bool allowedInSubGraph => false;
    }
}