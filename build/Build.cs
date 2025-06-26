using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Test); // Testing by default (we always want to ensure tests pass)

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    Target Clean => _ => _
        .DependsOn(CleanExchange);

    Target Restore => _ => _
        .DependsOn(RestoreExchange);

    Target Compile => _ => _
        .DependsOn(Restore)
        .DependsOn(CompileExchange);

    Target Test => _ => _
        .DependsOn(Compile)
        .DependsOn(TestExchange);

}
