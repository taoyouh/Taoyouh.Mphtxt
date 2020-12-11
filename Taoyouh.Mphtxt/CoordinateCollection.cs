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
        /// <summary>
        /// Initializes an instance of <see cref="CoordinateCollection"/>.
        /// </summary>
        /// <param name="dimension">The dimension of each mesh node.</param>
        /// <param name="count">The count of mesh nodes.</param>
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
        /// <param name="index">The 0-based index of the mesh node.</param>
        public Coordinate this[int index]
        {
            get => new Coordinate(new Memory<double>(Storage, index * Dimension, Dimension));
        }

        /// <inheritdoc/>
        public IEnumerator<Coordinate> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// The enumerator to enumerate a <see cref="CoordinateCollection"/>.
        /// </summary>
        public sealed class Enumerator : IEnumerator<Coordinate>
        {
            private CoordinateCollection container;
            private int index;

            /// <summary>
            /// Initializes an enumerator.
            /// </summary>
            /// <param name="container">The collection to be enumerated.</param>
            public Enumerator(CoordinateCollection container)
            {
                this.container = container ?? throw new ArgumentNullException(nameof(container));
                index = -1;
            }

            /// <inheritdoc/>
            public Coordinate Current => container[index];

            /// <inheritdoc/>
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
