// <copyright file="MphtxtWriterTests.cs" company="Huang, Zhaoquan">
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
    public class MphtxtWriterTests
    {
        [TestMethod]
        public void RoundtripTest()
        {
            var mphtxtStream = new MemoryStream();
            var writer = new MphtxtWriter(mphtxtStream);

            const int pointCount = 100;
            const int tetCount = 50;

            var coordinates = new CoordinateCollection(3, pointCount);
            var random = new Random(8269684);
            for (int i = 0; i < pointCount; ++i)
            {
                coordinates[i][0] = random.NextDouble();
                coordinates[i][1] = random.NextDouble();
                coordinates[i][2] = random.NextDouble();
            }

            var tets = new GeometryElementCollection(tetCount, 4);
            for (int i = 0; i < tetCount; ++i)
            {
                tets[i][0] = random.Next(pointCount);
                tets[i][1] = random.Next(pointCount);
                tets[i][2] = random.Next(pointCount);
                tets[i][3] = random.Next(pointCount);
                tets[i].EntityIndex = 1;
            }

            var mesh = new MphtxtMesh();
            mesh.Coordinates = coordinates;
            mesh.Elements["tet"] = tets;

            var objs = new Dictionary<string, MphtxtObject>();
            objs["mesh1"] = mesh;

            writer.Write(objs);

            mphtxtStream.Seek(0, SeekOrigin.Begin);
            var reader = new MphtxtReader(mphtxtStream);
            reader.Read();
        }
    }
}