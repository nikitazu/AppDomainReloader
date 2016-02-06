#r "packages/FAKE/tools/FakeLib.dll"
open Fake

// Targets
//
Target "test" (fun _ ->
    trace "Testing stuff..."
)

Target "test-app" (fun _ ->
    let result =
        ExecProcess (fun info ->
            info.FileName <- "TestApp.Launcher/bin/Debug/TestApp.Launcher.exe"
            info.WorkingDirectory <- "TestApp.Launcher/bin/Debug/"
            info.Arguments <- ""
        )(System.TimeSpan.FromSeconds 15.)
    if result <> 0 then failwith ("Test app failed")
)

Target "build-test-app" (fun _ ->
    !! "TestApp.Library/*.csproj"
    ++ "TestApp.Launcher/*.csproj"
    |> MSBuildDebug "TestApp.Launcher/bin/Debug/" "Build"
    |> Log "test-app-out: "
)

Target "build" (fun _ ->
    !! "AppDomainReloader/*.csproj"
    |> MSBuildDebug "AppDomainReloader/bin/Debug/" "Build"
    |> Log "build-out: "
)

Target "release" (fun _ ->
    !! "AppDomainReloader/*.csproj"
    |> MSBuildRelease "AppDomainReloader/bin/Release/" "Build"
    |> Log "build-release-out: "
)

Target "pack" (fun _ ->
    let result =
        ExecProcess (fun info ->
            info.FileName <- ".paket/paket.exe"
            info.WorkingDirectory <- "./"
            info.Arguments <- "pack output nugets"
        )(System.TimeSpan.FromSeconds 15.)
    if result <> 0 then failwith ("pack failed")
)

Target "push" (fun _ ->
    let latestPackage =
        !! "nugets\*.nupkg"
        |> Seq.last
    let result =
        ExecProcess (fun info ->
            info.FileName <- ".paket/paket.exe"
            info.WorkingDirectory <- "./"
            info.Arguments <- "push url https://www.nuget.org file " + latestPackage
        )(System.TimeSpan.FromSeconds 15.)
    if result <> 0 then failwith ("push failed")
)

// Dependencies
//
"test"
    ==> "build"

"build-test-app"
    ==> "test-app"

"release"
    ==> "pack"

RunTargetOrDefault "build"
