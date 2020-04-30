// <copyright file="MphtxtReaderTests.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Taoyouh.Mphtxt;

namespace Taoyouh.Mphtxt.Tests
{
    [TestClass]
    public class MphtxtReaderTests
    {
        [TestMethod]
        public void ReadTest()
        {
            using var stream = File.OpenRead("Test Data/ETS Sample 4.mphtxt");
            using var reader = new MphtxtReader(stream);
            var result = reader.Read();

            var selections = result.Values.Select(v => v as MphtxtSelection).Where(v => v != null);
            Assert.IsTrue(selections.Any());
            foreach (var selection in selections)
            {
                var mesh = result[selection.MeshTag] as MphtxtMesh;
                Assert.IsNotNull(mesh);
                switch (selection.Dimension)
                {
                    case 3:
                        CollectionAssert.IsSubsetOf(selection.Entities.ToArray(), mesh.Elements["tet"].Select(e => e.EntityIndex).ToArray());
                        break;
                    case 2:
                        CollectionAssert.IsSubsetOf(selection.Entities.ToArray(), mesh.Elements["tri"].Select(e => e.EntityIndex).ToArray());
                        break;
                    case 1:
                        CollectionAssert.IsSubsetOf(selection.Entities.ToArray(), mesh.Elements["edg"].Select(e => e.EntityIndex).ToArray());
                        break;
                    case 0:
                        CollectionAssert.IsSubsetOf(selection.Entities.ToArray(), mesh.Elements["vtx"].Select(e => e.EntityIndex).ToArray());
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
        }
    }
}