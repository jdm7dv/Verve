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
    public enum GraphEdgeLabel
    {
        UNLABELLED,
        COMPLELLED,
        REVERSIBLE
    }

    [Serializable]
    public class DirectedGraphEdge : IComparable<DirectedGraphEdge>, ICloneable
    {
        public string From { get; private set; }
        public string To { get; private set; }
        public double Cost { get; set; }
        public GraphEdgeLabel Label { get; set; }
        public string Key { get { return From + "," + To; } }

        public void ChangeEdgeFrom(string newLabel)
        {
            From = newLabel;
        }

        public void ChangeEdgeTo(string newLabel)
        {
            To = newLabel;
        }

        /// <summary>
        /// Comparator for DirectedEdge instances. This means that you
        /// can, e.g. sort a list without giving a comparator:
        /// List<DirectedGraphEdge> edges = ...
        /// edges.Sort();
        /// The function returns -1, if this instance is smaller than the 
        /// other instance (its 'Cost' is smaller).
        /// It returns 0, if both instances are considered equally important 
        /// (have the same 'Cost').
        /// The function returns 1, if this instance is larger than the 
        /// other instance (its 'Cost' is larger).
        /// </summary>
        /// <param name="other">other is another instance to which this instance is compared</param>
        /// <returns></returns>
        int IComparable<DirectedGraphEdge>.CompareTo(DirectedGraphEdge other)
        {
            if (other.Cost > this.Cost)
                return -1;
            else if (other.Cost == this.Cost)
                return 0;
            else
                return 1;
        }

        /// <summary>
        /// Constructs an edge (from,to) with cost 'Cost'.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cost"></param>
        public DirectedGraphEdge(string from, string to, double cost)
        {
            if (from != to)
            {
                this.From = from;
                this.To = to;
                this.Cost = cost;
                this.Label = GraphEdgeLabel.UNLABELLED;
            }
            else
            {
                throw new GraphException("Edge must not point from a node to itself.");
            }
        }

        /// <summary>
        /// Transposes an edge. This means that edge (u,v) will afterwards
        /// represent edge (v,u).
        /// </summary>
        public void Transpose()
        {
            string temp = this.From;
            this.From = this.To;
            this.To = temp;
        }

        /// <summary>
        /// Serves to construct a new edge with the same values as this 
        /// edge instance.
        /// </summary>
        /// <returns></returns>
        public Object Clone()
        {
            DirectedGraphEdge tempEdge = new DirectedGraphEdge(this.From, this.To, this.Cost);
            tempEdge.Label = this.Label;
            return tempEdge;
        }

        public override int GetHashCode()
        {
            return (this.From + "," + this.To).GetHashCode();
        }

        /// <summary>
        /// Overrides the standard ToString() function to get a more
        /// meaningful representation of the edge. The format is
        /// (u,v)[f] where (u,v) stands for the edge from u to v
        /// and f is the cost.
        /// </summary>
        /// <returns></returns>
        override public string ToString()
        {
            switch (this.Label)
            {
                case GraphEdgeLabel.UNLABELLED:
                    return " (" + this.From + "," + this.To + ")[" + this.Cost + "]";
                case GraphEdgeLabel.COMPLELLED:
                    return " (" + this.From + "," + this.To + ")[" + this.Cost + "][COMPELLED]";
                case GraphEdgeLabel.REVERSIBLE:
                    return " (" + this.From + "," + this.To + ")[" + this.Cost + "][REVERSIBLE]";
                default:
                    return " (" + this.From + "," + this.To + ")[" + this.Cost + "]";
            }
        }

        override public bool Equals(object obj)
        {
            DirectedGraphEdge tempEdge = (DirectedGraphEdge)obj;
            return (tempEdge.From.Equals(this.From)
                    && tempEdge.To.Equals(this.To)
                    && tempEdge.Cost == this.Cost
                    && tempEdge.Label == this.Label);
        }
    }

}
