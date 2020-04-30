// <copyright file="GeometryElement.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;

namespace Taoyouh.Mphtxt
{
    public class GeometryElement
    {
        private GeometryElementCollection parent;
        private int index;

        internal GeometryElement(GeometryElementCollection parent, int index)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            this.index = index;
        }

        /// <summary>
        /// Thie index of item that the elelemt belongs to.
        /// </summary>
        public int EntityIndex
        {
            get => parent.EntityIndexStorage[index];
            set => parent.EntityIndexStorage[index] = value;
        }

        /// <summary>
        /// The index of points that the element has.
        /// </summary>
        /// <param name="i">To get or set the index of i-th node in this element.</param>
        public int this[int i]
        {
            get => parent.NodesStorage[(parent.ElementNodeCount * index) + i];
            set => parent.NodesStorage[(parent.ElementNodeCount * index) + i] = value;
        }
    }
}
