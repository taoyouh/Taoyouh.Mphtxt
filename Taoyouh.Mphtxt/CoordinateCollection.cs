// <copyright file="CoordinateCollection.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace Taoyouh.Mphtxt
{
    public class CoordinateCollection : IEnumerable<Coordinate>
    {
        public CoordinateCollection(int dimension, int count)
        {
            Dimension = dimension;
            Count = count;
            Storage = new double[dimension * count];
        }

        public int Dimension { get; }

        public int Count { get; }

        internal double[] Storage { get; }

        public Coordinate this[int index]
        {
            get => new Coordinate(this, index);
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
