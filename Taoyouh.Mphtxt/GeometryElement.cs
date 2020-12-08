// <copyright file="GeometryElement.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;

namespace Taoyouh.Mphtxt
{
    /// <summary>
    /// A mesh element.
    /// </summary>
    public readonly struct GeometryElement : IEquatable<GeometryElement>
    {
        private readonly Memory<int> entityIndexStorage;

        public GeometryElement(Memory<int> entityIndexStorage, Memory<int> nodesStorage)
        {
            this.entityIndexStorage = entityIndexStorage;
            this.NodesStorage = nodesStorage;
        }

        public GeometryElement(int nodesPerElement)
        {
            var array = new int[nodesPerElement + 1];
            this.entityIndexStorage = new Memory<int>(array, 0, 1);
            this.NodesStorage = new Memory<int>(array, 1, nodesPerElement);
        }

        /// <summary>
        /// The index of the geometry entity (e.g. a cube) that the element (e.g. a tetrahedron) belongs to.
        /// </summary>
        public ref int EntityIndex => ref entityIndexStorage.Span[0];

        internal Memory<int> NodesStorage { get; }

        /// <summary>
        /// The index of points (e.g. vertices of a tetrahedron) that the element has.
        /// </summary>
        /// <param name="i">To get or set the index of i-th node in this element.</param>
        public ref int this[int i] => ref NodesStorage.Span[i];

        public static bool operator ==(GeometryElement left, GeometryElement right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GeometryElement left, GeometryElement right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is GeometryElement geometryElement)
            {
                return Equals(geometryElement);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return entityIndexStorage.GetHashCode()
                ^ NodesStorage.GetHashCode();
        }

        public bool Equals(GeometryElement other)
        {
            return entityIndexStorage.Equals(other.entityIndexStorage)
                && NodesStorage.Equals(other.NodesStorage);
        }
    }
}
