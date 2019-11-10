
namespace Api

    module App =

        open Fable.React
        open Elmish
        open Elmish.React
        module c = Fable.React.ReactBindings
        module R = Fable.React.Helpers
        open Feliz
        open Zanaptak.TypedCssClasses
        open Fable.Core
        open Fable.SimpleJson
        open Fable.SimpleHttp
        open PokemonDropdown
        open ProgramModels
        open Css
        open H1
        open Button
        open PokemonModels
        open Serde
        open HttpModels
        open Elmish.SweetAlert
        
        let init () =
            {
                PokemonTypes = HasNotLoaded
                CurrentPokemonTypeId = 0
            }, 
            Cmd.none

        let getPokemonTypes () =
            async{
                //let! (statusCode, responseText ) = Http.get "http://localhost:7071/api/allPokemonType"
                let! (statusCode, responseText ) = Http.get "/example.json"
                let result = responseText |> Json.tryParseAs<PokemonTypes>
                //let result = responseText|> tryBindJson<PokemonTypes>
                let ret =
                    match statusCode, result with
                    | 200, Ok pokemonTypes -> PokemonTypesReceived pokemonTypes
                    | 200, Error e -> ErrorWhileReceivingData e
                    | _ -> ErrorWhileReceivingData (sprintf "Status code: %A" statusCode)
                return ret
            }
        
        
        let withGetPokemonTypesCommand state =
            {state with PokemonTypes = Loading}, Cmd.OfAsync.perform getPokemonTypes () id

        let withoutCommands state = state, Cmd.none



        let update (msg:Msg) (state:State) : State * Cmd<Msg> =
            match msg with
            | SetCurrentPokemonId id -> { state with CurrentPokemonTypeId = id}, Cmd.none
            | GetPokemonTypes -> state |> withGetPokemonTypesCommand
            | PokemonTypesReceived pokemonTypes -> {state with PokemonTypes = FinishedLoading (Ok pokemonTypes)} |> withoutCommands
            | ErrorWhileReceivingData e ->  let alert = SimpleAlert(e)
                                            state, SweetAlert.Run(alert)
//{state with PokemonTypes = FinishedLoading (Error e)} |> withoutCommands


        let render (state: State) dispatch =  
            Html.div[
                prop.classes [ Bulma.Control]
                prop.style [style.textAlign.center; style.marginTop 50]
                prop.children[
                    renderSimpleH1 "Pokemon Types"
                    renderPokemonDropdown state.PokemonTypes dispatch
                    renderSimpleH1 (sprintf "Currently: %A" state.CurrentPokemonTypeId)
                    renderSimpleButton "Fetch" (fun _ -> dispatch GetPokemonTypes )
                ]
            ]

        Program.mkProgram init update render
        |> Program.withReactBatched "app"
        |> Program.run
    