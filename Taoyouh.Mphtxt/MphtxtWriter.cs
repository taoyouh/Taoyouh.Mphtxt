// <copyright file="MphtxtWriter.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Taoyouh.Mphtxt
{
    public sealed class MphtxtWriter : IDisposable
    {
        private readonly StreamWriter writer;

        public MphtxtWriter(Stream stream, bool leaveOpen = false)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            writer = new StreamWriter(stream, new UTF8Encoding(), 1024, leaveOpen);
        }

        public void Dispose()
        {
            writer.Dispose();
        }

        public void Write(IDictionary<string, MphtxtObject> objects)
        {
            if (objects == null)
            {
                throw new ArgumentNullException(nameof(objects));
            }

            Span<int> version = stackalloc int[2]
            {
                0, 1,
            };
            WriteInts(version);

            var tagCount = objects.Count;
            WriteInt(tagCount);
            foreach (var pair in objects)
            {
                WriteString(pair.Key);
            }

            var typeCount = objects.Count;
            WriteInt(typeCount);
            foreach (var pair in objects)
            {
                var type = "obj";
                WriteString(type);
            }

            foreach (var pair in objects)
            {
                if (pair.Value is MphtxtMesh mesh)
                {
                    WriteMesh(mesh);
                }
                else if (pair.Value is MphtxtSelection selection)
                {
                    WriteSelection(selection);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            writer.Flush();
        }

        private void WriteInt(int value)
        {
            writer.WriteLine(value);
        }

        private void WriteInts(Span<int> values)
        {
            foreach (var item in values)
            {
                writer.Write(item);
                writer.Write(' ');
            }

            writer.WriteLine();
        }

        private void WriteDoubles(Span<double> values)
        {
            foreach (var item in values)
            {
                writer.Write(item);
                writer.Write(' ');
            }

            writer.WriteLine();
        }

        private void WriteString(string str)
        {
            var length = Encoding.UTF8.GetByteCount(str);
            writer.Write(length);
            writer.Write(' ');
            writer.WriteLine(str);
        }

        private void WriteMesh(MphtxtMesh mesh)
        {
            Span<int> maybeVersion = stackalloc int[3]
            {
                0, 0, 1,
            };
            WriteInts(maybeVersion);

            var @class = "Mesh";
            WriteString(@class);
            var version = 4;
            WriteInt(version);
            WriteInt(mesh.Coordinates.Dimension);
            WriteInt(mesh.Coordinates.Count);
            WriteInt(0); // Lowest mesh point index.

            foreach (var point in mesh.Coordinates)
            {
                WriteDoubles(point.Storage.Span);
            }

            WriteInt(mesh.Elements.Count);
            foreach (var elementType in mesh.Elements)
            {
                WriteString(elementType.Key);
                WriteInt(elementType.Value.ElementNodeCount);
                WriteInt(elementType.Value.Count);
                foreach (var element in elementType.Value)
                {
                    WriteInts(element.NodesStorage.Span);
                }

                WriteInt(elementType.Value.Count);
                foreach (var element in elementType.Value)
                {
                    WriteInt(element.EntityIndex);
                }
            }
        }

        private void WriteSelection(MphtxtSelection selection)
        {
            Span<int> maybeVersion = stackalloc int[3]
            {
                0,
                0,
                1,
            };
            WriteInts(maybeVersion);

            var @class = "Selection";
            WriteString(@class);

            var version = 0;
            WriteInt(version);
            WriteString(selection.Label);
            WriteString(selection.MeshTag);
            WriteInt(selection.Dimension);
            WriteInt(selection.Entities.Count());
            foreach (var item in selection.Entities)
            {
                WriteInt(item);
            }
        }
    }
}