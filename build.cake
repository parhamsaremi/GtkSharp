#load CakeScripts\GAssembly.cs
#load CakeScripts\Settings.cs
#addin "Cake.FileHelpers"
#addin "Cake.Incubator"

// VARS

Settings.Cake = Context;
Settings.BuildTarget = Argument("BuildTarget", "Default");
Settings.Assembly = Argument("Assembly", "");

var msbuildsettings = new DotNetCoreMSBuildSettings();
var list = new List<GAssembly>();

// TASKS

Task("Init")
    .Does(() =>
{
    // Assign version
    msbuildsettings = msbuildsettings.WithProperty("Version", "3.0.0.0");
    msbuildsettings = msbuildsettings.WithProperty("Authors", "'GLibSharp Team'");

    // Add stuff to list
    foreach(var gassembly in Settings.AssemblyList)
        if(string.IsNullOrEmpty(Settings.Assembly) || Settings.Assembly == gassembly.Name)
            list.Add(gassembly);
});

Task("Prepare")
    .IsDependentOn("Clean")
    .Does(() =>
{
    // Build tools
    DotNetCoreRestore("Source/Tools/Tools.sln");
    MSBuild("Source/Tools/Tools.sln", new MSBuildSettings {
        Verbosity = Verbosity.Minimal,
        Configuration = "Release",
    });

    // Generate code and prepare libs projects
    foreach(var gassembly in list)
        gassembly.Prepare();
    DotNetCoreRestore("Source/Libs/GtkSharp.sln");
});

Task("GenerateLinuxStubs")
    .IsDependentOn("Init")
    .Does(() => 
{
    CreateDirectory("BuildOutput/LinuxStubs");
    FileWriteText("BuildOutput/LinuxStubs/empty.c", "");
    foreach(var gassembly in list)
        gassembly.GenerateLinuxStubs();
    DeleteDirectory("BuildOutput/LinuxStubs", new DeleteDirectorySettings { Recursive = true, Force = true });
});

Task("Clean")
    .IsDependentOn("Init")
    .Does(() =>
{
    foreach(var gassembly in list)
        gassembly.Clean();
});

Task("FullClean")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DeleteDirectory("BuildOutput", true);
});

Task("Build")
    .IsDependentOn("Prepare")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        MSBuildSettings = msbuildsettings
    };

    if (list.Count == Settings.AssemblyList.Count)
        DotNetCoreBuild("Source/Libs/GtkSharp.sln", settings);
    else
    {
        foreach(var gassembly in list)
            DotNetCoreBuild(gassembly.Csproj, settings);
    }
});

Task("PackageNuGet")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings
    {
        MSBuildSettings = msbuildsettings,
        Configuration = "Release",
        OutputDirectory = "BuildOutput/NugetPackages",
        NoBuild = true,

    };

    foreach(var gassembly in list)
        DotNetCorePack(gassembly.Csproj, settings);
});

Task("PackageTemplates")
    .IsDependentOn("Init")
    .Does(() =>
{
    var settings = new NuGetPackSettings
    {
        BasePath = "Source/Templates/NetCore/GtkSharp.Template.CSharp",
        OutputDirectory = "BuildOutput/NugetPackages"
    };

    NuGetPack("Source/Templates/NetCore/GtkSharp.Template.CSharp/GtkSharp.Template.CSharp.nuspec", settings);
});

// TASK TARGETS

Task("Default")
    .IsDependentOn("Build");

// EXECUTION

RunTarget(Settings.BuildTarget);
