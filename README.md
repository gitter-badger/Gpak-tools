# GPAK tools

*this project is still under development*

## Description

C# library for reading **GPAK** files (have *.gp* extension) which were utilized for storing graphical data in some old games (such as **Cossacks** and **American Conquest** from **GSC Game World** company). Library produces Bitmap objects which can be saved using ordinary image format (e.g. *PNG*).

Format specs were reverse engineered completely from scratch. There is still room for improvements since not all available GP files can be successfully recovered. 

## Dev/Run

Since it's still in development it should be run from Visual Studio (>= 2012). Currently projects are build with .NET 4.5 support, but i think they could be run with .NET 4.0 also. All dependencies should be successfully restored by NuGet on first build (in fact only **UnitTests** project uses external dependencies: NUnit and Fluent Assertions).

## Examples

Sample of recovered image:

![alt text](https://raw.githubusercontent.com/klym1/Gpak-tools/master/Example.PNG "Building from Cossacks Game")
