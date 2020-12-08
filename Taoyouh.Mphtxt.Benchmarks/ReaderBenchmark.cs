using System;
using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Taoyouh.Mphtxt;

[EventPipeProfiler(0)]
public class ReaderBenchmark
{
    private Stream mphtxtStream;

    [GlobalSetup]
    public void GlobalSetup()
    {
        mphtxtStream = new MemoryStream();
        var writer = new MphtxtWriter(mphtxtStream);

        const int pointCount = 1000000;
        const int tetCount = 500000;

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
    }

    [IterationSetup]
    public void IterationSetup()
    {
        mphtxtStream.Seek(0, SeekOrigin.Begin);
    }

    [Benchmark]
    public IDictionary<string, MphtxtObject> Read()
    {
        var reader = new MphtxtReader(mphtxtStream);
        return reader.Read();
    }
}