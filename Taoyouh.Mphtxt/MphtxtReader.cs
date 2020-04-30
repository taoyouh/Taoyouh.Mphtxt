// <copyright file="MphtxtReader.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Taoyouh.Mphtxt.Resources;

namespace Taoyouh.Mphtxt
{
    public sealed class MphtxtReader : IDisposable
    {
        private readonly StreamReader reader;
        private int lineId = 0;

        public MphtxtReader(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            reader = new StreamReader(stream);
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        /// <summary>
        /// Read and parse the mphtxt file.
        /// </summary>
        /// <returns>The dictionary that maps object tag to object content.</returns>
        public IDictionary<string, MphtxtObject> Read()
        {
            var version = ReadInts(2);

            var tagCount = ReadInt();
            string[] tags = new string[tagCount];
            for (int i = 0; i < tagCount; i++)
            {
                tags[i] = ReadString();
            }

            var typeCount = ReadInt();
            string[] types = new string[typeCount];
            for (int i = 0; i < typeCount; i++)
            {
                types[i] = ReadString();
            }

            var objects = new Dictionary<string, MphtxtObject>();
            for (int i = 0; i < typeCount; ++i)
            {
                objects.Add(tags[i], ReadObject());
            }

            if (ReadLine() != null)
            {
                throw new InvalidDataException($"Unexpected content at line {lineId}. Expected end of file.");
            }

            return objects;
        }

        private int ReadInt()
        {
            string line = ReadLine();
            if (line == null)
            {
                throw new EndOfStreamException($"Expected content at line {lineId}.");
            }

            return int.Parse(line, CultureInfo.InvariantCulture);
        }

        private int[] ReadInts(int count)
        {
            return ReadNumbersInString(count).Select(x => int.Parse(x, CultureInfo.InvariantCulture)).ToArray();
        }

        private double[] ReadDoubles(int count)
        {
            return ReadNumbersInString(count).Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToArray();
        }

        private string[] ReadNumbersInString(int intCount)
        {
            string line = ReadLine();
            if (line == null)
            {
                throw new EndOfStreamException($"Expected content at line {lineId}.");
            }

            string[] numStrs = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (numStrs.Length != intCount)
            {
                throw new InvalidDataException($"Number format incorrect at line {lineId}. Expected {intCount} numbers but there are {numStrs.Length}");
            }

            return numStrs;
        }

        private string ReadString()
        {
            string line = ReadLine();
            if (line == null)
            {
                throw new EndOfStreamException();
            }

            int length = int.Parse(line.Substring(0, line.IndexOf(' ')), CultureInfo.InvariantCulture);
            var bytes = Encoding.UTF8.GetBytes(line.Substring(line.IndexOf(' ') + 1));
            return Encoding.UTF8.GetString(bytes, 0, length).Trim('\0');
        }

        private string ReadLine()
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                ++lineId;
                if (line.Contains("#"))
                {
                    line = line.Substring(0, line.IndexOf('#'));
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                return line;
            }

            return null;
        }

        private MphtxtObject ReadObject()
        {
            ReadInts(3);
            var @class = ReadString();
            switch (@class)
            {
                case "Mesh":
                    return ReadMeshObj();
                case "Selection":
                    return ReadSelectionObj();
                default:
                    throw new NotImplementedException();
            }
        }

        private MphtxtMesh ReadMeshObj()
        {
            var objectVersion = ReadInt();
            var dimension = ReadInt();
            if (dimension != 2 && dimension != 3)
            {
                throw new InvalidDataException(Strings.MphtxtReader_ReadMeshObj_NotSupportedDimension);
            }

            var meshPointCount = ReadInt();
            var lowestMeshPointIndex = ReadInt();

            var coordinates = new CoordinateCollection(dimension, meshPointCount);
            for (int i = 0; i < meshPointCount; i++)
            {
                var values = ReadDoubles(dimension);
                for (int j = 0; j < dimension; ++j)
                {
                    coordinates[i][j] = values[j];
                }
            }

            var elementTypeCount = ReadInt();
            var geometryElements = new Dictionary<string, GeometryElementCollection>();
            for (int typeId = 0; typeId < elementTypeCount; typeId++)
            {
                var typeName = ReadString();

                var nodePerEle = ReadInt();
                var elementCount = ReadInt();
                var elements = new GeometryElementCollection(elementCount, nodePerEle);
                geometryElements[typeName] = elements;
                for (int i = 0; i < elementCount; i++)
                {
                    var values = ReadInts(nodePerEle).Select(x => x - lowestMeshPointIndex).ToArray();
                    for (int j = 0; j < nodePerEle; ++j)
                    {
                        elements[i][j] = values[j];
                    }
                }

                var indexCount = ReadInt();
                if (indexCount != elementCount)
                {
                    throw new InvalidDataException(
                        $"Type {typeName} has different element count ({elementCount}) and index count ({indexCount}).");
                }

                for (int i = 0; i < indexCount; i++)
                {
                    elements[i].EntityIndex = ReadInt();
                }
            }

            return new MphtxtMesh(coordinates, geometryElements);
        }

        private MphtxtSelection ReadSelectionObj()
        {
            var objectVersion = ReadInt();
            var label = ReadString();
            var meshTag = ReadString();
            var dimension = ReadInt();
            var entityCount = ReadInt();
            int[] entities = new int[entityCount];
            for (int i = 0; i < entityCount; ++i)
            {
                entities[i] = ReadInt();
            }

            return new MphtxtSelection(label, meshTag, dimension, entities);
        }
    }
}
