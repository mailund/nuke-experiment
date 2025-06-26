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
        .DependsOn(CleanExchange).DependsOn(CleanNativeInterface).DependsOn(CleanExchangeApp);

    Target Restore => targetDefinition => targetDefinition
        .DependsOn(RestoreExchange).DependsOn(RestoreNativeInterface).DependsOn(RestoreExchangeApp);

    Target Compile => targetDefinition => targetDefinition
        .DependsOn(Restore)
        .DependsOn(CompileExchange).DependsOn(CompileNativeInterface).DependsOn(CompileExchangeApp);

    Target Test => targetDefinition => targetDefinition
        .DependsOn(Compile)
        .DependsOn(TestExchange).DependsOn(TestNativeInterface);

    Target Run => targetDefinition => targetDefinition
    .DependsOn(CompileExchangeApp)
    .DependsOn(CompileCPlugin)
    .DependsOn(CompileRustPlugin)
    .Executes(() =>
    {
        // FIXME: This is probably not the best way to run the app, but it works for now.
        var dynLibPaths = new[] {
            System.IO.Path.GetFullPath("./CPlugin/build/libcplugin.dylib"),
            System.IO.Path.GetFullPath("./rust_plugin/target/release/librust_plugin.dylib")
        };
        System.Diagnostics.Process.Start("dotnet", $"run --project ./ExchangeApp/ExchangeApp.csproj -- {string.Join(" ", dynLibPaths)}");
    });

}