module App

    open Fable.React
    open Elmish
    open Elmish.React
    module c = Fable.React.ReactBindings
    module R = Fable.React.Helpers
    open Feliz
    open Zanaptak.TypedCssClasses
    open Fable.SimpleHttp

    type FA = CssClasses<"https://use.fontawesome.com/releases/v5.8.1/css/all.css", Naming.PascalCase>
    type Bulma = CssClasses<"https://cdn.jsdelivr.net/npm/bulma@0.8.0/css/bulma.min.css", Naming.PascalCase>
    


    type RemoteData<'a> =
        | HasNotLoaded
        | Loading
        | FinishedLoading of 'a

    type Msg = 
     | Increment 
     | Decrement
     | IncrementTwice
     | IncrementDelayed
     | GetData
     | DataRecieved of string
     | DataErrorWhileReceivingData of string

    type State = 
        {
            Counter : int
            ResponseText : RemoteData<Result<string, string>>
        }
        
    let getSomeData () =
        async {
            do! Async.Sleep 4000
            let! (statusCode, responseText ) = Http.get "http://localhost:7071/api/immuneAgainst/1"
            let ret = 
                match statusCode with 
                | 200 -> DataRecieved responseText
                | _ -> DataErrorWhileReceivingData (sprintf "Status code: %A" statusCode)
            return ret
        }

    
    let withGetDataCommand model =
        {model with ResponseText = Loading}, Cmd.OfAsync.perform getSomeData () id

    let withoutCommands model =
        model, Cmd.none

    let init () =
        {
            Counter = 0
            ResponseText = HasNotLoaded
            
        }
        |> withGetDataCommand

    let delayed timeout (msg: Msg) =
        async {
            do! Async.Sleep timeout
            return msg
        }


    let update (msg:Msg) (state:State) : State * Cmd<Msg>=
      match msg with
      | Increment -> { state with Counter = state.Counter + 1} |> withoutCommands
      | Decrement -> { state with Counter = state.Counter - 1} |> withoutCommands
      | IncrementTwice -> state, Cmd.batch [Cmd.ofMsg Increment; Cmd.ofMsg Increment]
      | IncrementDelayed -> state, Cmd.OfAsync.perform (fun timeout -> delayed timeout Increment) 2000 id // id spot : function to make a message out of the response from the async
      | GetData -> state |> withGetDataCommand
      | DataRecieved data -> {state with ResponseText = FinishedLoading (Ok data)} |> withoutCommands
      | DataErrorWhileReceivingData e ->  {state with ResponseText = FinishedLoading (Error e)} |> withoutCommands
      


    let renderSimpleH1 (text:string) (color:string) =
        Html.h1 [
            prop.text text
            prop.style [style.color color]
        ]
    let private renderResponseText (responseText: RemoteData<Result<string, string>>) =
        match responseText with
        | HasNotLoaded -> Html.none
        | Loading -> Html.span "Imagin fancy spinner"
        | FinishedLoading (Ok data) -> renderSimpleH1 data "#0000ff"
        | FinishedLoading (Error e) -> renderSimpleH1 e "#ff0000"

        
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
                Html.button 
                    [
                        prop.classes [ Bulma.Button; Bulma.IsMedium ]
                        prop.onClick (fun _ -> dispatch IncrementTwice)
                        prop.text "2x"
                    ]
                Html.button 
                    [
                        prop.classes [ Bulma.Button; Bulma.IsMedium ]
                        prop.onClick (fun _ -> dispatch IncrementDelayed)
                        prop.text "delayed"
                    ]
                Html.button 
                    [
                        prop.classes [ Bulma.Button; Bulma.IsMedium ]
                        prop.onClick (fun _ -> dispatch GetData)
                        prop.text "Get Data"
                    ]
                renderResponseText state.ResponseText
                ]
             ]

    Program.mkProgram init update render
    |> Program.withReactBatched "app"
    |> Program.run