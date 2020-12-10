// <copyright file="MphtxtSelection.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Taoyouh.Mphtxt
{
    /// <summary>
    /// An mphtxt selection (a list of geometry entities).
    /// </summary>
    public class MphtxtSelection : MphtxtObject
    {
        private string _label;
        private string _meshTag;
        private int _dimension;
        private IEnumerable<int> _entities;

        public MphtxtSelection()
        {
        }

        public MphtxtSelection(string label, string meshTag, int dimension, IEnumerable<int> entities)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            MeshTag = meshTag ?? throw new ArgumentNullException(nameof(meshTag));
            Dimension = dimension;
            Entities = entities ?? throw new ArgumentNullException(nameof(entities));
        }

        /// <summary>
        /// The label (the display name defined in COMSOL) of the selection.
        /// </summary>
        public string Label
        {
            get => _label;
            set => _label = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// The tag (key) of the mesh that this selection corresponds to.
        /// </summary>
        public string MeshTag
        {
            get => _meshTag;
            set => _meshTag = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// The dimension of the selection.
        /// </summary>
        public int Dimension
        {
            get => _dimension;
            set => _dimension = value;
        }

        /// <summary>
        /// The indices of entities in this selection.
        /// </summary>
        public IEnumerable<int> Entities
        {
            get => _entities;
            set => _entities = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
