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

        /// <summary>
        /// Initializes an instance of <see cref="MphtxtMesh"/>.
        /// </summary>
        /// <param name="coordinates">The mesh nodes in the mesh.</param>
        /// <param name="elements">The element dictionary in the mesh.</param>
        public MphtxtMesh(CoordinateCollection coordinates, IDictionary<string, GeometryElementCollection> elements)
        {
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
            Elements = elements ?? throw new ArgumentNullException(nameof(elements));
        }

        /// <summary>
        /// Initializes an empty instance of <see cref="MphtxtMesh"/>.
        /// </summary>
        public MphtxtMesh()
        {
        }

        /// <summary>
        /// The mesh nodes in the mesh.
        /// </summary>
        public CoordinateCollection Coordinates
        {
            get => _collection;
            set => _collection = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// The elements in the mesh grouped by name of element type (e.g. "tet" for tetrahedrons, "edg" for edges, "tri" for triangles).
        /// </summary>
        public IDictionary<string, GeometryElementCollection> Elements { get; } = new Dictionary<string, GeometryElementCollection>();
    }
}
