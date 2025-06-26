using Nuke.Common;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using Nuke.Common.Tools.DotNet;

public partial class Build
{
    Target CleanExchange => targetDefinition => targetDefinition
        .Executes(() =>
        {
            DotNetClean(s => s.SetProject("Exchange/Exchange.csproj"));
            DotNetClean(s => s.SetProject("Exchange/Exchange.Tests/Exchange.Tests.csproj"));
        });

    Target RestoreExchange => targetDefinition => targetDefinition
        .DependsOn(CleanExchange)
        .Executes(() =>
        {
            DotNetRestore(s => s.SetProjectFile("Exchange/Exchange.csproj"));
            DotNetRestore(s => s.SetProjectFile("Exchange/Exchange.Tests/Exchange.Tests.csproj"));
        });

    Target CompileExchange => targetDefinition => targetDefinition
        .DependsOn(RestoreExchange)
        .Executes(() =>
        {
            DotNetBuild(s => s.SetProjectFile("Exchange/Exchange.csproj")
                .SetConfiguration(Configuration));
        });

    Target TestExchange => targetDefinition => targetDefinition
        .DependsOn(CompileExchange)
        .Executes(() =>
        {
            DotNetTest(s => s.SetProjectFile("Exchange/Exchange.Tests/Exchange.Tests.csproj"));
        });
}