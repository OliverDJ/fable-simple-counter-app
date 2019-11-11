
namespace Api
    module PokemonDropdown =
        
        open Feliz
        open Css
        open ProgramModels
        open Fable.React
        open Feliz
        open PokemonModels
        open Dropdown
        open HttpModels
        open Fable.SimpleJson
        open Fable.SimpleHttp
        open Elmish


        let getPokemonTypes () =
            async{
                let! (statusCode, responseText ) = Http.get "http://localhost:7071/api/allPokemonTypes"
                let result = responseText |> Json.tryParseAs<PokemonTypes>
                let ret =
                    match statusCode, result with
                    | 200, Ok pokemonTypes -> PokemonTypesReceived pokemonTypes
                    | 200, Error e -> ErrorWhileReceivingData e
                    | _ -> ErrorWhileReceivingData (sprintf "Status code: %A" statusCode)
                return ret
            }

        let withGetPokemonTypesCommand state =
            {state with PokemonTypes = Loading}, Cmd.OfAsync.perform getPokemonTypes () id

        let createPokemonOption (p: PokemonType) =
            let styles = [style.backgroundColor p.Color; style.color "#ffffff"; ]
            let k = Dropdown.createOption p.Name p.Id (Some styles)
            k

        //let createOptionList (li: Result<PokemonTypes, string>) =
        //    let r =
        //        match li with 
        //        | Ok p -> p |> List.map createPokemonOption
        //        | Error e -> []
        //    r

        let createOptionList2 (li: PokemonTypes) =
            li |> List.map createPokemonOption


        let setCurrentId f (x : Browser.Types.Event) = 
            let id = x.Value |> int
            SetCurrentPokemonId id |> f


        let rend (ptypes: PokemonTypes) dispatch =
            Html.div [
                prop.style [style.marginTop 5; style.marginBottom 10]
                prop.classes [Bulma.Select; Bulma.IsPrimary]
                prop.children [
                    Html.select[
                        prop.onChange (setCurrentId dispatch)
                        prop.children (ptypes |> createOptionList2)
                    ]
                ]
            ]



        let renderPokemonDropdown (pokemonTypeState: RemoteData<Result<PokemonTypes, string>>) (dispatch: Msg -> unit)=
            printfn "%A" pokemonTypeState
            match pokemonTypeState with
            | HasNotLoaded -> rend [] dispatch
            | Loading -> rend [] dispatch
            | FinishedLoading (Error e) -> rend [] dispatch
            | FinishedLoading (Ok data) -> rend data dispatch



            

