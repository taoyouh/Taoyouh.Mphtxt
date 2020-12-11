// <copyright file="MphtxtReader.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// </copyright>

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Taoyouh.Mphtxt.Resources;

namespace Taoyouh.Mphtxt
{
    /// <summary>
    /// A reader that parses an mphtxt file.
    /// </summary>
    public sealed class MphtxtReader : IDisposable
    {
        private readonly StreamReader reader;
        private int lineId = 0;

        /// <summary>
        /// Initialzes an instance of <see cref="MphtxtReader"/>.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="leaveOpen">Whether <paramref name="stream"/> should be left open when the reader is disposed of.</param>
        public MphtxtReader(Stream stream, bool leaveOpen = false)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            reader = new StreamReader(stream, Encoding.UTF8, false, 1024, leaveOpen);
        }

        /// <inheritdoc/>
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
            Span<int> version = stackalloc int[2];
            ReadInts(version);

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

            return ParseInt(line.AsSpan());
        }

        private void ReadInts(Span<int> buffer)
        {
            using (var stringsMemory = MemoryPool<ReadOnlyMemory<char>>.Shared.Rent(buffer.Length))
            {
                var stringsSpan = stringsMemory.Memory.Span.Slice(0, buffer.Length);
                ReadNumbersInString(stringsSpan);
                for (int i = 0; i < buffer.Length; ++i)
                {
                    buffer[i] = ParseInt(stringsSpan[i].Span);
                }
            }
        }

        private void ReadDoubles(Span<double> buffer)
        {
            using (var stringsMemory = MemoryPool<ReadOnlyMemory<char>>.Shared.Rent(buffer.Length))
            {
                var stringsSpan = stringsMemory.Memory.Span.Slice(0, buffer.Length);
                ReadNumbersInString(stringsSpan);
                for (int i = 0; i < buffer.Length; ++i)
                {
                    buffer[i] = ParseDouble(stringsSpan[i].Span);
                }
            }
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

        private void ReadNumbersInString(Span<ReadOnlyMemory<char>> buffer)
        {
            string line = ReadLine();
            if (line == null)
            {
                throw new EndOfStreamException($"Expected content at line {lineId}.");
            }

            int segmentIndex = 0;
            int charIndex = 0;
            var chars = line.AsMemory();
            for (int i = 0; i < line.Length; ++i)
            {
                if (line[i] == ' ')
                {
                    if (i != charIndex)
                    {
                        if (segmentIndex >= buffer.Length)
                        {
                            throw new InvalidDataException(
                                $"Number format incorrect at line {lineId}. Expected {buffer.Length} numbers but has got more.");
                        }

                        buffer[segmentIndex] = chars.Slice(charIndex, i - charIndex);
                    }

                    ++segmentIndex;
                    charIndex = i + 1;
                }
            }

            if (segmentIndex != buffer.Length)
            {
                throw new InvalidDataException($"Number format incorrect at line {lineId}. Expected {buffer.Length} numbers but there are only {segmentIndex}");
            }
        }

        private int ParseInt(ReadOnlySpan<char> intString)
        {
#if NETSTANDARD2_1
            if (int.TryParse(intString, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }
            else
            {
                throw new FormatException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Cannot parse string \"{0}\" at line {1} into an integer.",
                    intString.ToString(),
                    lineId));
            }
#else
            int result = 0;
            foreach (char c in intString)
            {
                if (c >= '0' && c <= '9')
                {
                    result = (result * 10) + (c - '0');
                }
                else
                {
                    throw new FormatException(string.Format(
                        CultureInfo.CurrentCulture,
                        "Cannot parse string \"{0}\" at line {1} into an integer.",
                        intString.ToString(),
                        lineId));
                }
            }

            return result;
#endif
        }

        private double ParseDouble(ReadOnlySpan<char> doubleString)
        {
#if NETSTANDARD2_1
            if (double.TryParse(doubleString, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
#else
            if (double.TryParse(doubleString.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
#endif
            {
                return result;
            }
            else
            {
                throw new FormatException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Cannot parse string \"{0}\" at line {1} into a floating-point number.",
                    doubleString.ToString(),
                    lineId));
            }
        }

        private string ReadString()
        {
            string line = ReadLine();
            if (line == null)
            {
                throw new EndOfStreamException();
            }

            var spaceIndex = line.IndexOf(" ", StringComparison.Ordinal);
            int length = int.Parse(line.Substring(0, spaceIndex), CultureInfo.InvariantCulture);
            var bytes = Encoding.UTF8.GetBytes(line.Substring(spaceIndex + 1));
            return Encoding.UTF8.GetString(bytes, 0, length).Trim('\0');
        }

        private string ReadLine()
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                ++lineId;
                var commentIndex = line.IndexOf("#", StringComparison.Ordinal);
                if (commentIndex >= 0)
                {
                    line = line.Substring(0, commentIndex);
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
            Span<int> temp = stackalloc int[3];
            ReadInts(temp);

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
                ReadDoubles(coordinates[i].Storage.Span);
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
                    ReadInts(elements[i].NodesStorage.Span);
                    for (int j = 0; j < nodePerEle; ++j)
                    {
                        elements[i][j] -= lowestMeshPointIndex;
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
