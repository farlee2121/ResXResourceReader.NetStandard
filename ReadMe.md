[![nuget](https://img.shields.io/nuget/v/ResXResourceReader.NetStandard)](https://www.nuget.org/packages/ResXResourceReader.NetStandard/)
![build](https://github.com/farlee2121/ResXResourceReader.NetStandard/workflows/Build/badge.svg)
![downloads](https://img.shields.io/nuget/dt/ResXResourceReader.NetStandard)

Install
-------
via Nuget: https://www.nuget.org/packages/ResXResourceReader.NetStandard/

Why would you want this library?
--------------------------------
If you want to programmatically read/update a ResX file, the standard is to use the `System.Resources.ResXResourceReader` and `System.Resources.ResXResourceWriter`. However these libraries are part of System.Windows.Forms. That is either a annoying extra dependency or a deal breaker for .Net Standard.

This library separates those two classes from System.Windows.Forms and packages them for .Net Standard so you can easily include them across
frameworks.

If you don't know a resx file is:
 - It is a way of shipping resources, like strings and files, with an assembly (project).
 -  It simplifies messy file copy on build and relative runtime file references
 - Is commonly used for managing translations and localized resources

Sources
-------
This library is almost a direct copy of files from the open sourced winforms respository  
https://github.com/dotnet/winforms/tree/b666dc7a94d8ac87a7d300cfb4fa86332fb79bae/src/System.Windows.Forms/src/System/Resources

However, I did revert [this change](https://github.com/dotnet/winforms/commit/f9f414d72a4d00da3f709ec3b76521d2859e6d49) in order to remove the dependency on System.Numerics, which was limiting .Net Standard compatibility.

I've also changed the namespace to System.Resources.NetStandard to avoid potential naming conflicts.  

Examples
--------
See how to use `ResXResourceReader` and `ResXResourceWriter` at
- https://docs.microsoft.com/en-us/dotnet/api/system.resources.resxresourcereader?view=netframework-4.8
- https://docs.microsoft.com/en-us/dotnet/api/system.resources.resxresourcewriter?view=netframework-4.8
- https://stackoverflow.com/questions/676312/modifying-resx-file-in-c-sharp


Gotchas
-------

- Bitmaps are only supported on windows or if you [separately install libgdiplus](https://www.ryadel.com/en/asp-net-core-linux-unable-to-load-dll-libgdiplus-gdiplus-dll-gdi-fix/). [Here](https://docs.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/system-drawing-common-windows-only#reason-for-change) is Microsoft's reasoning for why this issue won't be fixed.
