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
        private CoordinateCollection _collection = new CoordinateCollection(0, 0);

        public MphtxtMesh(CoordinateCollection coordinates, IDictionary<string, GeometryElementCollection> elements)
        {
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
            Elements = elements ?? throw new ArgumentNullException(nameof(elements));
        }

        public MphtxtMesh()
        {
        }

        public CoordinateCollection Coordinates
        {
            get => _collection;
            set => _collection = value ?? throw new ArgumentNullException(nameof(value));
        }

        public IDictionary<string, GeometryElementCollection> Elements { get; } = new Dictionary<string, GeometryElementCollection>();
    }
}
