module App

    open Fable.React
    open Elmish
    open Elmish.React
    module c = Fable.React.ReactBindings
    module R = Fable.React.Helpers
    open Feliz
    open Zanaptak.TypedCssClasses
    
    type FA = CssClasses<"https://use.fontawesome.com/releases/v5.8.1/css/all.css", Naming.PascalCase>
    type Bulma = CssClasses<"https://cdn.jsdelivr.net/npm/bulma@0.8.0/css/bulma.min.css", Naming.PascalCase>
    
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
        Html.div[
            prop.classes [ Bulma.Control]
            prop.style [style.textAlign.center; style.marginTop 50]
            prop.children [
                Html.button [
                    prop.classes [ Bulma.Button; Bulma.IsPrimary; Bulma.IsMedium ]
                    prop.onClick (fun _ -> dispatch Increment)
                    prop.children [ 
                        Html.i [ prop.classes [ FA.Fa; FA.FaPlus ] ]
                    ]
                ]
                Html.h1 [
                    prop.className "title"
                    prop.style [style.marginTop 20; ]

                    prop.text state.Counter
                ]
                Html.button [
                    prop.classes [ Bulma.Button; Bulma.IsPrimary; Bulma.IsMedium ]
                    prop.onClick (fun _ -> dispatch Decrement)
                    prop.children [ 
                        Html.i [ prop.classes [ FA.Fa; FA.FaMinus ] ]
                    ]
                ]
             ]
        ]

    Program.mkSimple init update render
    |> Program.withReactBatched "app"
    |> Program.run