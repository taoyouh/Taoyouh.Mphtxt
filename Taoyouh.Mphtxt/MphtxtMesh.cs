// <copyright file="MphtxtMesh.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Taoyouh.Mphtxt
{
    /// <summary>
    /// An mphtxt mesh object that contains node coordinates and elements.
    /// </summary>
    public class MphtxtMesh : MphtxtObject
    {
        public MphtxtMesh(CoordinateCollection coordinates, IDictionary<string, GeometryElementCollection> elements)
        {
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
            Elements = elements ?? throw new ArgumentNullException(nameof(elements));
        }

        public CoordinateCollection Coordinates { get; }

        public IDictionary<string, GeometryElementCollection> Elements { get; }
    }
}
