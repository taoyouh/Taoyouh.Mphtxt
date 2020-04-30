// <copyright file="MphtxtSelection.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Taoyouh.Mphtxt
{
    public class MphtxtSelection : MphtxtObject
    {
        public MphtxtSelection(string label, string meshTag, double dimension, IEnumerable<int> entities)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            MeshTag = meshTag ?? throw new ArgumentNullException(nameof(meshTag));
            Dimension = dimension;
            Entities = entities ?? throw new ArgumentNullException(nameof(entities));
        }

        public string Label { get; }

        public string MeshTag { get; }

        public double Dimension { get; }

        public IEnumerable<int> Entities { get; }
    }
}
