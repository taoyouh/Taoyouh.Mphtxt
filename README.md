# Taoyouh.Mphtxt

Taoyouh.Mphtxt is a .NET library for parsing mphtxt (COMSOL mesh) files.

## Features
- Parse all types of elements (vertex, edge, triangle, tetrahedron, hexahedron, etc.)
- Parse selections exported from COMSOL 5.4 or higher
- Parse multiple meshes
- Support .NET Standard 2.0 (can run on .NET Core and .NET Framework)

## Usage
The library is available on [nuget](https://www.nuget.org/packages/Taoyouh.Mphtxt/). Just search for "Taoyouh.Mphtxt" in Visual Studio NuGet package manager and install the latest version. To install using dotnet CLI, use the following command:
```
dotnet add package Taoyouh.Mphtxt
```

Code sample:
```C#
using (var stream = File.OpenRead("Test Data/ETS Sample 4.mphtxt"))
using (var reader = new MphtxtReader(stream))
{
    // "result" is a dictionary that contains all objects in the mphtxt file.
    var result = reader.Read();

    // Mesh objects are of the class "MphtxtMesh".
    var meshes = result.Values.Select(v => v as MphtxtMesh).Where(v => v != null);

    // Selection objects are of the class "MphtxtSelection".
    var selections = result.Values.Select(v => v as MphtxtSelection).Where(v => v != null);

    // Or you can get the particular object by its name.
    var mesh1 = result["mesh1"] as MphtxtMesh;
}
```