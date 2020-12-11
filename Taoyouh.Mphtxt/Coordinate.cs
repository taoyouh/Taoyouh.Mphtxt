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
        /// <summary>
        /// Initializes an instance of <see cref="Coordinate"/>.
        /// </summary>
        /// <param name="storage">The memory to store the coordinate values (components).</param>
        public Coordinate(Memory<double> storage)
        {
            this.Storage = storage;
        }

        /// <summary>
        /// Initializes an instance of <see cref="Coordinate"/>.
        /// </summary>
        /// <param name="length">The number of coordinates (components) that a mesh node has.</param>
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

        /// <summary>
        /// Whether <paramref name="left"/> and <paramref name="right"/> references the same area of memory.
        /// </summary>
        /// <param name="left">One instance of <see cref="Coordinate"/>.</param>
        /// <param name="right">Another instance of <see cref="Coordinate"/>.</param>
        /// <returns>Whether the two instances are equal.</returns>
        public static bool operator ==(Coordinate left, Coordinate right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Whether <paramref name="left"/> and <paramref name="right"/> references different memory areas.
        /// </summary>
        /// <param name="left">One instance of <see cref="Coordinate"/>.</param>
        /// <param name="right">Another instance of <see cref="Coordinate"/>.</param>
        /// <returns>Whether the two instances are unequal.</returns>
        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        /// <seealso cref="operator ==(Coordinate, Coordinate)"/>
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

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Storage.GetHashCode();
        }

        /// <inheritdoc/>
        /// <seealso cref="operator ==(Coordinate, Coordinate)"/>
        public bool Equals(Coordinate other)
        {
            return Storage.Equals(other.Storage);
        }
    }
}
