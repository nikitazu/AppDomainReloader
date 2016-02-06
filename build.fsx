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

// Dependencies
//
"test"
    ==> "build"

"build-test-app"
    ==> "test-app"

RunTargetOrDefault "build"
