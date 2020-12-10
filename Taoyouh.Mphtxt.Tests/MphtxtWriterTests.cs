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
            using var writer = new MphtxtWriter(mphtxtStream);

            Dictionary<string, MphtxtObject> objs = CreateMesh();

            writer.Write(objs);

            mphtxtStream.Seek(0, SeekOrigin.Begin);
            using var reader = new MphtxtReader(mphtxtStream);
            reader.Read();
        }

        [TestMethod]
        public void WriteTest()
        {
            var objs = CreateMesh();

            using (var mphtxtStream = File.OpenWrite(Path.GetTempFileName()))
            using (var writer = new MphtxtWriter(mphtxtStream))
            {
                writer.Write(objs);
            }
        }

        private static Dictionary<string, MphtxtObject> CreateMesh()
        {
            const int division = 5;
            var coordinates = new CoordinateCollection(3, (int)Math.Pow(division + 1, 3));
            for (int i = 0; i <= division; ++i)
            {
                int baseI = i * (int)Math.Pow(division + 1, 2);
                for (int j = 0; j <= division; ++j)
                {
                    int baseJ = baseI + (j * (division + 1));
                    for (int k = 0; k <= division; ++k)
                    {
                        coordinates[baseJ + k][0] = (double)i / division;
                        coordinates[baseJ + k][1] = (double)j / division;
                        coordinates[baseJ + k][2] = (double)k / division;
                    }
                }
            }

            var tets = new GeometryElementCollection((int)Math.Pow(division, 3) * 5, 4);
            for (int i = 0; i < division; ++i)
            {
                int pointBaseI = i * (int)Math.Pow(division + 1, 2);
                int tetBaseI = i * (int)Math.Pow(division, 2);
                for (int j = 0; j < division; ++j)
                {
                    int pointBaseJ = pointBaseI + (j * (division + 1));
                    int tetBaseJ = tetBaseI + (j * division);
                    for (int k = 0; k < division; ++k)
                    {
                        int deltaI = (int)Math.Pow(division + 1, 2);
                        int deltaJ = division + 1;
                        int p0 = pointBaseJ + k;
                        int p1 = pointBaseJ + k + deltaI;
                        int p2 = pointBaseJ + k + deltaI + deltaJ;
                        int p3 = pointBaseJ + k + deltaJ;
                        int p4 = p0 + 1;
                        int p5 = p1 + 1;
                        int p6 = p2 + 1;
                        int p7 = p3 + 1;

                        int firstTet = (tetBaseJ + k) * 5;
                        tets[firstTet][0] = p1;
                        tets[firstTet][1] = p6;
                        tets[firstTet][2] = p4;
                        tets[firstTet][3] = p5;

                        tets[firstTet + 1][0] = p3;
                        tets[firstTet + 1][1] = p1;
                        tets[firstTet + 1][2] = p4;
                        tets[firstTet + 1][3] = p0;

                        tets[firstTet + 2][0] = p3;
                        tets[firstTet + 2][1] = p6;
                        tets[firstTet + 2][2] = p1;
                        tets[firstTet + 2][3] = p2;

                        tets[firstTet + 3][0] = p3;
                        tets[firstTet + 3][1] = p4;
                        tets[firstTet + 3][2] = p6;
                        tets[firstTet + 3][3] = p7;

                        tets[firstTet + 4][0] = p1;
                        tets[firstTet + 4][1] = p3;
                        tets[firstTet + 4][2] = p4;
                        tets[firstTet + 4][3] = p6;

                        for (int l = 0; l < 5; ++l)
                        {
                            tets[firstTet + l].EntityIndex = 1;
                        }
                    }
                }
            }

            var bottomSelection = new MphtxtSelection()
            {
                Dimension = 3,
                Label = "选择 1",
                Entities = new[] { 1 },
                MeshTag = "mesh1",
            };

            var mesh = new MphtxtMesh();
            mesh.Coordinates = coordinates;
            mesh.Elements["tet"] = tets;

            var objs = new Dictionary<string, MphtxtObject>();
            objs["mesh1"] = mesh;
            objs["sel1"] = bottomSelection;
            return objs;
        }
    }
}