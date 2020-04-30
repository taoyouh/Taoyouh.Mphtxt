// <copyright file="Coordinate.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;

namespace Taoyouh.Mphtxt
{
    public class Coordinate
    {
        private readonly CoordinateCollection parent;
        private readonly int index;

        internal Coordinate(CoordinateCollection parent, int index)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            this.index = index;
        }

        public double this[int i]
        {
            get => parent.Storage[(index * parent.Dimension) + i];
            set => parent.Storage[(index * parent.Dimension) + i] = value;
        }
    }
}
