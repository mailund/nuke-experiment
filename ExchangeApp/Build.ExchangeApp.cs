using Nuke.Common;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using Nuke.Common.Tools.DotNet;

public partial class Build
{
    Target CleanExchangeApp => _ => _
        .Executes(() =>
        {
            DotNetClean(s => s.SetProject("ExchangeApp/ExchangeApp.csproj"));
        });

    Target RestoreExchangeApp => _ => _
        .DependsOn(CleanExchangeApp)
        .Executes(() =>
        {
            DotNetRestore(s => s.SetProjectFile("ExchangeApp/ExchangeApp.csproj"));
        });

    Target CompileExchangeApp => _ => _
        .DependsOn(RestoreExchangeApp)
        .DependsOn(CompileExchange)
        .DependsOn(CompileNativeInterface)
        .Executes(() =>
        {
            DotNetBuild(s => s.SetProjectFile("ExchangeApp/ExchangeApp.csproj")
                .SetConfiguration(Configuration));
        });
}
