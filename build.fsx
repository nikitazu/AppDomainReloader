#r "packages/FAKE/tools/FakeLib.dll"
open Fake

Target "test" (fun _ ->
    trace "Testing stuff..."
)

Target "test-app" (fun _ ->
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

"test"
   ==> "build"

RunTargetOrDefault "build"
