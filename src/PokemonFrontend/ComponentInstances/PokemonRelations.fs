
namespace Api
    
    module PokemonRelations =
        

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
        open H1


        let getRelations typeId =
            async{
                let! (statusCode, responseText ) = Http.get (sprintf "http://localhost:7071/api/relations/%d" typeId)
                let result = responseText |> Json.tryParseAs<PokemonRelations>
                let ret =
                    match statusCode, result with
                    | 200, Ok relations -> PokemonRelationsReceived relations
                    | 200, Error e -> ErrorWhileReceivingData e
                    | _ -> ErrorWhileReceivingData (sprintf "Status code: %A" statusCode)
                return ret
            }

        let withGetRelationsCommand state =
            match state.CurrentPokemonTypeId with
            | NotSelected -> {state with PokemonRelations = Loading}, Cmd.OfAsync.perform getRelations 0 id
            | Selected pId -> {state with PokemonRelations = Loading}, Cmd.OfAsync.perform getRelations pId id

        let renderPokemonType (pokemonType: PokemonType) =
            Html.div[
                prop.style [style.margin 5; style.backgroundColor pokemonType.Color]
                prop.classes [Bulma.HasShadow;]
                prop.children [
                     Html.label [
                        prop.style[style.color "#ffffff"]
                        prop.text pokemonType.Name
                    ]
                ]
            ]

        let renderPokemonTypeList (pokemonTypes: PokemonTypes) = 
            Html.ul [
                prop.children [ for p in pokemonTypes -> renderPokemonType p]
            ]

        let renderPokemonRelations title (pokemonTypes: PokemonTypes) =
            let k = Some [style.color "#ffffff"]
            Html.div [
                prop.style [style.maxWidth 150;style.padding 10; style.backgroundColor "#eeeeee"]
                prop.classes [Bulma.Container;]
                prop.children[
                    renderSimpleH1 title None//(Some [style.color "#ffffff"])
                    renderPokemonTypeList pokemonTypes   
                ]
            ]

            

        let renderAllRelations (pokemonRelationsState: RemoteData<Result<PokemonRelations, string>>) =
            printfn "Data:  %A" data
            match pokemonRelationsState with
                | FinishedLoading (Ok data) ->
                    Html.div[
                        prop.style [ style.marginTop 20]
                        prop.classes [Bulma.Level;]
                        prop.children[
                            renderPokemonRelations "Strong Against" data.StrongAgainst
                            renderPokemonRelations "Not Effective Against" data.NotEffective
                            renderPokemonRelations "Weak Against" data.WeakAgainst
                            renderPokemonRelations "Resistant Against" data.ResistantAgainst
                            renderPokemonRelations "Does Not Effect" data.DoesNotEffect
                            renderPokemonRelations "Immune Against" data.ImmuneAgainst
                        ]
                    ]

                | _ -> div[][]


            //Html.div [
            //    prop.style [style.maxWidth 150;style.padding 10; style.backgroundColor "#333333"]
            //    prop.classes [Bulma.Container]
            //    match pokemonRelationsState with
            //    | FinishedLoading (Ok data) ->
            //        prop.children[
            //            renderPokemonRelations data.StrongAgainst
            //            renderPokemonRelations data.WeakAgainst
                        
            //        ]
            //    | _ -> ()
            
