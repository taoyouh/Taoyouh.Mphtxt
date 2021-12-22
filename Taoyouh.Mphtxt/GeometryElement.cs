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
        private readonly Memory<int> nodesStorage;

        /// <summary>
        /// Initializes an instance of <see cref="GeometryElement"/>.
        /// </summary>
        /// <param name="entityIndexStorage">The memory to store <see cref="EntityIndex"/>.</param>
        /// <param name="nodesStorage">The memory to store node indices.</param>
        public GeometryElement(Memory<int> entityIndexStorage, Memory<int> nodesStorage)
        {
            this.entityIndexStorage = entityIndexStorage;
            this.nodesStorage = nodesStorage;
        }

        /// <summary>
        /// Initializes an instance of <see cref="GeometryElement"/>.
        /// </summary>
        /// <param name="nodesPerElement">Number of mesh nodes per mesh element.</param>
        public GeometryElement(int nodesPerElement)
        {
            var array = new int[nodesPerElement + 1];
            this.entityIndexStorage = new Memory<int>(array, 0, 1);
            this.nodesStorage = new Memory<int>(array, 1, nodesPerElement);
        }

        /// <summary>
        /// The index of the geometry entity (e.g. a cube) that the element (e.g. a tetrahedron) belongs to.
        /// </summary>
        public ref int EntityIndex => ref entityIndexStorage.Span[0];

        /// <summary>
        /// The index of points (e.g. vertices of a tetrahedron) that the element has.
        /// </summary>
        /// <param name="i">To get or set the index of i-th node in this element.</param>
        public ref int this[int i] => ref nodesStorage.Span[i];

        /// <summary>
        /// Whether <paramref name="left"/> and <paramref name="right"/> references the same area of memory.
        /// </summary>
        /// <param name="left">One instance of <see cref="GeometryElement"/>.</param>
        /// <param name="right">Another instance of <see cref="GeometryElement"/>.</param>
        /// <returns>Whether the two instances are equal.</returns>
        public static bool operator ==(GeometryElement left, GeometryElement right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Whether <paramref name="left"/> and <paramref name="right"/> references different memory areas.
        /// </summary>
        /// <param name="left">One instance of <see cref="GeometryElement"/>.</param>
        /// <param name="right">Another instance of <see cref="GeometryElement"/>.</param>
        /// <returns>Whether the two instances are unequal.</returns>
        public static bool operator !=(GeometryElement left, GeometryElement right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        /// <seealso cref="operator ==(GeometryElement, GeometryElement)"/>
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

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return entityIndexStorage.GetHashCode()
                ^ nodesStorage.GetHashCode();
        }

        /// <inheritdoc/>
        /// <seealso cref="operator ==(GeometryElement, GeometryElement)"/>
        public bool Equals(GeometryElement other)
        {
            return entityIndexStorage.Equals(other.entityIndexStorage)
                && nodesStorage.Equals(other.nodesStorage);
        }

        /// <summary>
        /// Returns the node indices as a <see cref="Memory{T}"/>.
        /// </summary>
        /// <returns>The <see cref="Memory{T}"/> storing the node indices.</returns>
        public Memory<int> AsMemory() => nodesStorage;

        /// <summary>
        /// Returns the node indices as a <see cref="Span{T}"/>.
        /// </summary>
        /// <returns>The <see cref="Span{T}"/> storing the node indices.</returns>
        public Span<int> AsSpan() => nodesStorage.Span;
    }
}
