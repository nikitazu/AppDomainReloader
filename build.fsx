#r "packages/FAKE/tools/FakeLib.dll"
open Fake

Target "test" (fun _ ->
    trace "Testing stuff..."
)

Target "build" (fun _ ->
    trace "Heavy deploy action"
)

"test"
   ==> "build"

RunTargetOrDefault "build"
