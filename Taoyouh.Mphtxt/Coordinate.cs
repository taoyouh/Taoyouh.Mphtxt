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
    public readonly struct Coordinate : IEquatable<Coordinate>
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
        /// <param name="i">
        /// The index of coordinate value. (e.g. x -> 0, y -> 1, z -> 2)
        /// </param>
        public ref double this[int i]
        {
            get => ref Storage.Span[i];
        }

        public static bool operator ==(Coordinate left, Coordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is Coordinate coordinate)
            {
                return Storage.Equals(coordinate.Storage);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Storage.GetHashCode();
        }

        public bool Equals(Coordinate other)
        {
            return Storage.Equals(other.Storage);
        }
    }
}
