using Nuke.Common;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using Nuke.Common.Tools.DotNet;

public partial class Build
{
    Target CleanNativeInterface => targetDefinition => targetDefinition
        .Executes(() =>
        {
            DotNetClean(s => s.SetProject("NativeInterface/NativeInterface.csproj"));
            DotNetClean(s => s.SetProject("NativeInterface/NativeInterface.Tests/NativeInterface.Tests.csproj"));
        });

    Target RestoreNativeInterface => targetDefinition => targetDefinition
        .DependsOn(CleanNativeInterface)
        .Executes(() =>
        {
            DotNetRestore(s => s.SetProjectFile("NativeInterface/NativeInterface.csproj"));
            DotNetRestore(s => s.SetProjectFile("NativeInterface/NativeInterface.Tests/NativeInterface.Tests.csproj"));
        });

    Target CompileNativeInterface => targetDefinition => targetDefinition
        .DependsOn(RestoreNativeInterface)
        .DependsOn(CompileExchange)
        .Executes(() =>
        {
            DotNetBuild(s => s.SetProjectFile("NativeInterface/NativeInterface.csproj")
                .SetConfiguration(Configuration));
        });

    Target TestNativeInterface => targetDefinition => targetDefinition
        .DependsOn(CompileNativeInterface)
        .Executes(() =>
        {
            DotNetTest(s => s.SetProjectFile("NativeInterface/NativeInterface.Tests/NativeInterface.Tests.csproj"));
        });
}