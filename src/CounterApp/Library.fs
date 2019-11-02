module App
    open Fable.React
    open Fable.React.Props
    open Elmish
    open Elmish.React
    module c = Fable.React.ReactBindings
    module R = Fable.React.Helpers

    type State = 
        {
            Counter : int
        }
        
    type Msg = Increment | Decrement

    let init () =
        {
            Counter = 0
        }

    let update (msg:Msg) (state:State) =
      match msg with
      | Increment -> { state with Counter = state.Counter + 1}
      | Decrement -> { state with Counter = state.Counter - 1}
        
    let render (state: State) dispatch =
        div [ClassName "foo"][//div
            h1 [] [str "Hello world"]
            button [ OnClick (fun _ -> dispatch Decrement) ] [ R.str "-" ]
            h1 [] [R.str (state.Counter.ToString())]
            button [ OnClick (fun _ -> dispatch Increment) ] [ R.str "+" ]
        ]
            
    Program.mkSimple init update render
    |> Program.withReactBatched "app"
    |> Program.run