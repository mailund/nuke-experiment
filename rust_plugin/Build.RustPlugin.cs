using Nuke.Common;
using Nuke.Common.Tooling;

public partial class Build
{
    Target CompileRustPlugin => _ => _
        .Executes(() =>
        {
            var pluginDir = RootDirectory / "rust_plugin";
            ProcessTasks.StartProcess("cargo", "build --workspace --release ", pluginDir, logOutput: false, logInvocation: false).AssertZeroExitCode();
        });
}
