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
        public GeometryElement this[int index]
            => new GeometryElement(
                new Memory<int>(EntityIndexStorage, index, 1),
                new Memory<int>(NodesStorage, index * ElementNodeCount, ElementNodeCount));

        public IEnumerator<GeometryElement> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        public sealed class Enumerator : IEnumerator<GeometryElement>
        {
            private GeometryElementCollection container;
            private int index;

            public Enumerator(GeometryElementCollection container)
            {
                this.container = container ?? throw new ArgumentNullException(nameof(container));
                index = -1;
            }

            public GeometryElement Current => container[index];

            object IEnumerator.Current => container[index];

            public void Dispose()
            {
                return;
            }

            public bool MoveNext()
            {
                return (++index) < container.Count;
            }

            public void Reset()
            {
                index = -1;
            }
        }
    }
}
