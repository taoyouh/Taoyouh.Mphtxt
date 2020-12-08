// <copyright file="CoordinateCollection.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Taoyouh.Mphtxt
{
    /// <summary>
    /// A collection of mesh node coordinates.
    /// </summary>
    public class CoordinateCollection : IEnumerable<Coordinate>
    {
        public CoordinateCollection(int dimension, int count)
        {
            Dimension = dimension;
            Count = count;
            Storage = new double[dimension * count];
        }

        /// <summary>
        /// Gets the dimension of mesh nodes.
        /// </summary>
        public int Dimension { get; }

        /// <summary>
        /// Gets the number of mesh nodes.
        /// </summary>
        public int Count { get; }

        internal double[] Storage { get; }

        /// <summary>
        /// Gets the coordinates of a mesh node.
        /// </summary>
        public Coordinate this[int index]
        {
            get => new Coordinate(new Memory<double>(Storage, index * Dimension, Dimension));
        }

        public IEnumerator<Coordinate> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        public sealed class Enumerator : IEnumerator<Coordinate>
        {
            private CoordinateCollection container;
            private int index;

            public Enumerator(CoordinateCollection container)
            {
                this.container = container ?? throw new ArgumentNullException(nameof(container));
                index = -1;
            }

            public Coordinate Current => container[index];

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
