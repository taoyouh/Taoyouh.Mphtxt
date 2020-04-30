// <copyright file="GeometryElementCollection.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Taoyouh.Mphtxt
{
    public class GeometryElementCollection : IEnumerable<GeometryElement>
    {
        public GeometryElementCollection(int count, int elementNodeCount)
        {
            Count = count;
            ElementNodeCount = elementNodeCount;

            EntityIndexStorage = new int[count];
            NodesStorage = new int[count * elementNodeCount];
        }

        public int Count { get; }

        public int ElementNodeCount { get; }

        internal int[] EntityIndexStorage { get; }

        internal int[] NodesStorage { get; }

        public GeometryElement this[int index] => new GeometryElement(this, index);

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
