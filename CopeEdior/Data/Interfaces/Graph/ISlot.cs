using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.NodeGraph
{
    public interface ISlot: IEquatable<ISlot>
    {
        int id { get; }
        string displayName { get; set; }
        bool isInputSolt { get; }
        bool isOutputSolt {  get; }
        int priority { get; set; }
        AbstractNode owner{get;set;}
        bool hidden { get; set; }
    }
}