// <copyright file="Coordinate.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;
using System.Buffers;

namespace Taoyouh.Mphtxt
{
    /// <summary>
    /// The position of a mesh node.
    /// </summary>
    public readonly struct Coordinate
    {
        public Coordinate(Memory<double> storage)
        {
            this.Storage = storage;
        }

        public Coordinate(int length)
        {
            this.Storage = new Memory<double>(new double[length], 0, length);
        }

        internal Memory<double> Storage { get; }

        /// <summary>
        /// Gets or sets a coordinate value of the node mesh.
        /// </summary>
        public ref double this[int i]
        {
            get => ref Storage.Span[i];
        }
    }
}
