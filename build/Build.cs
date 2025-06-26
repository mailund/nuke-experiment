using JetBrains.Annotations;
using Nuke.Common;

partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft Visual Studio    https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Test); // Testing by default (we always want to ensure tests pass)

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [UsedImplicitly]
    Target Clean => targetDefinition => targetDefinition
        .DependsOn(CleanExchange);

    Target Restore => targetDefinition => targetDefinition
        .DependsOn(RestoreExchange);

    Target Compile => targetDefinition => targetDefinition
        .DependsOn(Restore)
        .DependsOn(CompileExchange);

    Target Test => targetDefinition => targetDefinition
        .DependsOn(Compile)
        .DependsOn(TestExchange);
}