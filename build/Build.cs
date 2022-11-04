using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

[GitHubActions("ci",
    GitHubActionsImage.UbuntuLatest,
    AutoGenerate = true,
    FetchDepth = 0,
    OnPushBranches = new[] { "main" },
    OnPullRequestBranches = new[] { "main" },
    InvokedTargets = new[] { nameof(PushNuget) },
    ImportSecrets = new[] { "NUGET_API_KEY" })]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.PushNuget);

    [Parameter] string NugetApiUrl = "https://api.nuget.org/v3/index.json"; //default
    [Parameter] string NugetApiKey;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion(Framework = "net6.0")] readonly GitVersion GitVersion;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath OutputDirectory => RootDirectory / "output";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";


    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(s => s.SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetTasks.DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(s => s.SetProjectFile(Solution));
        });

    Target CompileNuget => _ => _
        .DependsOn(Test)
    .Executes(() =>
    {
        DotNetTasks.DotNetBuild(s =>
            s.SetProjectFile(SourceDirectory / "Lib" / "Lib.csproj")
            .SetConfiguration(Configuration)
                    .SetAssemblyVersion(GitVersion.AssemblySemVer)
                    .SetFileVersion(GitVersion.AssemblySemFileVer)
                    .SetInformationalVersion(GitVersion.InformationalVersion)
                    .EnableNoRestore()
            );
    });

    Target PublishNuget => _ => _
        .DependsOn(CompileNuget)
    .Executes(() =>
    {
        DotNetTasks.DotNetPublish(s =>
            s.SetProject(SourceDirectory / "Lib" / "Lib.csproj")
            .SetOutput(OutputDirectory)
            );
    });

    Target PackNuget => _ => _
        .DependsOn(PublishNuget)
        .Executes(() =>
        {
            DotNetTasks.DotNetPack(s => s
                .SetProject(SourceDirectory / "Lib" / "Lib.csproj")
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetVersion(GitVersion.NuGetVersionV2)
                .SetIncludeSymbols(true)
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                .SetOutputDirectory(ArtifactsDirectory)
            );
        });

    Target PushNuget => _ => _
        .DependsOn(PackNuget)
        .Requires(() => NugetApiUrl)
        .Requires(() => NugetApiKey)
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(() =>
        {

            GlobFiles(ArtifactsDirectory, "*.nupkg")
           .Where(x => !x.EndsWith("symbols.nupkg"))
               .ForEach(x =>
               {
                   DotNetTasks.DotNetNuGetPush(s => s
                       .SetTargetPath(x)
                       .SetSource(NugetApiUrl)
                       .SetApiKey(NugetApiKey)
                   );
               });
        });
}
