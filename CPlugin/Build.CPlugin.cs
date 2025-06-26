using Nuke.Common;
using Nuke.Common.Utilities.Collections;
using Nuke.Common.Tooling;

public partial class Build
{
    Target CompileCPlugin => _ => _
        .Executes(() =>
        {
            var pluginDir = RootDirectory / "CPlugin";
            // Configure
            ProcessTasks.StartProcess("cmake", "-S . -B build", pluginDir).AssertZeroExitCode();
            // Build
            ProcessTasks.StartProcess("cmake", "--build build", pluginDir).AssertZeroExitCode();
        });
}