# Taoyouh.Mphtxt

Taoyouh.Mphtxt is a .NET library for parsing and writing mphtxt (COMSOL mesh) files.

## Features
- Parse all types of elements (vertex, edge, triangle, tetrahedron, hexahedron, etc.)
- Parse selections exported from COMSOL 5.4 or higher
- Parse multiple meshes
- Write mphtxt files
- Support .NET Standard 2.0 (can run on .NET Core and .NET Framework)

## Latest changes
Changes from 1.1.0 to 1.2.0:
- Add AsSpan() and AsMemory() methods to access geometry element node indices storages as Memory<T> or Span<T>
- Add source link support to improve debugging experience

Changes from 1.0.0 to 1.1.0:
- Add XML documentation
- Improve performance with the use of spans
- Add mphtxt writer

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

## Documentation
The APIs have XML documentation which can be accessed in IntelliSense popups, object browser and the "go to definition" panel.