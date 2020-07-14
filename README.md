# Taoyouh.Mphtxt

Taoyouh.Mphtxt is a .NET library for parsing mphtxt (COMSOL mesh) files.

## Features
- Parse all types of elements (vertex, edge, triangle, tetrahedron, hexahedron, etc.)
- Parse selections exported from COMSOL 5.4 or higher
- Parse multiple meshes
- Support .NET Standard 2.0 (can run on .NET Core and .NET Framework)

## Usage
NuGet packages are not available yet. You can use the source code directly:

1. Clone the repository
2. Add the project Taoyouh.Mphtxt as reference

Code sample:
```
using var stream = File.OpenRead("Test Data/ETS Sample 4.mphtxt");
using var reader = new MphtxtReader(stream);
var result = reader.Read();
var meshes = result.Values.Select(v => v as MphtxtMesh).Where(v => v != null);
var selections = result.Values.Select(v => v as MphtxtSelection).Where(v => v != null);
```