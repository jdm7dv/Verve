//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;

namespace MBT.Escience.Graph
{
    public enum GraphNodeColor
    {
        WHITE,
        GREY,
        BLACK
    }

    [Serializable]
    public class GraphNode : IComparable<GraphNode>, IComparable, ICloneable
    {
        public string Name { get; set; }
        public GraphNodeColor Color { get; set; }
        public uint DfsOrder { get; set; }
        public uint ComponentNumber { get; set; }

        public GraphNode(string name)
        {
            this.Name = name;
            this.Color = GraphNodeColor.WHITE;
            this.DfsOrder = 0;
            this.ComponentNumber = 0;
        }

        override public string ToString()
        {
            return " " + this.Name + "," + this.ComponentNumber + "[" + this.DfsOrder + "]";
        }

        public override bool Equals(object obj)
        {
            if (obj is GraphNode)
            {
                GraphNode node = (GraphNode)obj;
                return ((this.Name.Equals(node.Name)
                        && this.Color == node.Color
                        && this.ComponentNumber == node.ComponentNumber
                        && this.DfsOrder == node.DfsOrder));
            }
            else
            {
                return false;
            }
        }

        int IComparable<GraphNode>.CompareTo(GraphNode other)
        {
            // compares using topological order
            if (this.DfsOrder < other.DfsOrder)
                return 1;
            else if (other.DfsOrder == this.DfsOrder)
                return 0;
            else
                return -1;
        }

        int IComparable.CompareTo(Object otherObject)
        {
            if (!(otherObject is GraphNode))
            {
                return 0;
            }
            GraphNode other = (GraphNode)otherObject;

            // compares using topological order
            if (this.DfsOrder < other.DfsOrder)
                return 1;
            else if (other.DfsOrder == this.DfsOrder)
                return 0;
            else
                return -1;
        }

        public Object Clone()
        {
            GraphNode node = new GraphNode(this.Name);
            node.Color = this.Color;
            node.DfsOrder = this.DfsOrder;
            node.ComponentNumber = this.ComponentNumber;
            return node;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
