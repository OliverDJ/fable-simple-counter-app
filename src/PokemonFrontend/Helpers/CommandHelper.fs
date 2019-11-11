
namespace Api
    module CommandHelper =
        open Elmish
        open Elmish.SweetAlert


        let withoutCommands state = state, Cmd.none
        
        let withAlert text state = 
            state, text |> SimpleAlert |> SweetAlert.Run
            


