// <copyright file="GeometryElementCollection.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Taoyouh.Mphtxt
{
    /// <summary>
    /// A collection of mesh elements.
    /// </summary>
    public class GeometryElementCollection : IEnumerable<GeometryElement>
    {
        /// <summary>
        /// Initializes an instance of <see cref="GeometryElementCollection"/>.
        /// </summary>
        /// <param name="count">The count of mesh elements.</param>
        /// <param name="elementNodeCount">The number of mesh nodes per mesh element.</param>
        public GeometryElementCollection(int count, int elementNodeCount)
        {
            Count = count;
            ElementNodeCount = elementNodeCount;

            EntityIndexStorage = new int[count];
            NodesStorage = new int[count * elementNodeCount];
        }

        /// <summary>
        /// The number of elements in the collection.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// The number of nodes in each element.
        /// </summary>
        public int ElementNodeCount { get; }

        internal int[] EntityIndexStorage { get; }

        internal int[] NodesStorage { get; }

        /// <summary>
        /// Gets a mesh element.
        /// </summary>
        /// <param name="index">
        /// The 0-based index of the mehs element.
        /// </param>
        public GeometryElement this[int index]
            => new GeometryElement(
                new Memory<int>(EntityIndexStorage, index, 1),
                new Memory<int>(NodesStorage, index * ElementNodeCount, ElementNodeCount));

        /// <inheritdoc/>
        public IEnumerator<GeometryElement> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// The enumerator to enumerate a <see cref="GeometryElementCollection"/>.
        /// </summary>
        public sealed class Enumerator : IEnumerator<GeometryElement>
        {
            private GeometryElementCollection container;
            private int index;

            /// <summary>
            /// Initializes an enumerator.
            /// </summary>
            /// <param name="container">The collection to be enumerated.</param>
            public Enumerator(GeometryElementCollection container)
            {
                this.container = container ?? throw new ArgumentNullException(nameof(container));
                index = -1;
            }

            /// <inheritdoc/>
            public GeometryElement Current => container[index];

            object IEnumerator.Current => container[index];

            /// <summary>
            /// Does nothing.
            /// </summary>
            public void Dispose()
            {
                return;
            }

            /// <inheritdoc/>
            public bool MoveNext()
            {
                return (++index) < container.Count;
            }

            /// <inheritdoc/>
            public void Reset()
            {
                index = -1;
            }
        }
    }
}
