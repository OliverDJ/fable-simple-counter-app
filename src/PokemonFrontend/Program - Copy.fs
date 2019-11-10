module App

    open Fable.React
    open Elmish
    open Elmish.React
    module c = Fable.React.ReactBindings
    module R = Fable.React.Helpers
    open Feliz
    open Zanaptak.TypedCssClasses
    //open Fetch
    open Fable.Core
    open Fable.SimpleJson

    open Fable.PowerPack
    open Fable.PowerPack.Fetch

    type FA = CssClasses<"https://use.fontawesome.com/releases/v5.8.1/css/all.css", Naming.PascalCase>
    type Bulma = CssClasses<"https://cdn.jsdelivr.net/npm/bulma@0.8.0/css/bulma.min.css", Naming.PascalCase>
    type _CSS = CssClasses<"C:\\private-projects\\fable\\fable-pokemon-chart\\styles.css">

    

    type PokemonType =
        {
            Id : int
            Name : string
            Color : string
        }
    [<CLIMutable>]
    type Relations =
        {
            StrongAgainst : PokemonType array
            NotEffective : PokemonType array
            WeakAgainst : PokemonType array
            ResistantAgainst : PokemonType array
            DoesNotEffect: PokemonType array
            ImmuneAgainst: PokemonType array
        }

    type State = 
        {
            PokemonTypes : PokemonType list
            CurrentPokemonTypeId : int
        }
        
    type Msg =
    | SetCurrentPokemonId of int
    
    
    let normal = 
        {
            Id = 1
            Name = "Normal"
            Color = "#b2956b"
        }

    let fire = 
        {
            Id = 10
            Name = "Fire"
            Color = "#ff7654"
        }

    let water = 
        {
            Id = 11
            Name = "Water"
            Color = "#4fc8d9"
        }

    let grass = 
        {
            Id = 12
            Name = "Grass"
            Color = "#77c95b"
        }

    let electric = 
        {
            Id = 13
            Name = "Electric"
            Color = "#ffc60f"
        }

    let psychic = 
        {
            Id = 14
            Name = "Psychic"
            Color = "#ff6189"
        }

    let init () =
        {
            PokemonTypes = [normal; fire; water; grass; electric; psychic]
            CurrentPokemonTypeId = 0
        }

    let update (msg:Msg) (state:State) =
        match msg with
        | SetCurrentPokemonId id -> { state with CurrentPokemonTypeId = id}

    
    let boo f (x : Browser.Types.Event) = 
        let id = x.Value |> int
        SetCurrentPokemonId id |> f


    let createPokemonOption (p: PokemonType) =
        Html.option[
            prop.style [style.backgroundColor p.Color; style.color "#ffffff"; ]
            prop.text p.Name
            prop.value p.Id
        ]

    let createOptionList (li: PokemonType list) =
        li
        |> List.map createPokemonOption

    //let powerFetch (url: string) =
    //    fetch url []
    //    |> Promise.bind (fun res:Re -> res.text())
    //    |> Promise.map (fun txt ->
    //         Access here your ressource
    //        printfn "%A" text
    //    )

    let powerFetch2 url =
        promise {
            let! res = Fable.PowerPack.Fetch.fetch "http://fable.io"
            let! txt = res.text()
            // Access here your ressource
            printfn "%A" text
        }


    //let getRequest url = 
    //    promise {
    //        let! r = 
    //            Fetch.fetch 
    //                url
    //                [ 
    //                    RequestProperties.Method HttpMethod.GET
    //                    requestHeaders [ContentType "application/json"]
    //                    //RequestProperties.Credentials RequestCredentials.Sameorigin
    //                ]
    //        let! text = r.text()
    //        let ret = Json.parseAs<Relations> text 
    //        return ret
    //    }


    

    let render (state: State) dispatch =  
        let optionLi = state.PokemonTypes |> createOptionList
        Html.div[
            prop.classes [ Bulma.Control]
            prop.style [style.textAlign.center; style.marginTop 50]
            prop.children[
                Html.h1 [
                    prop.classes [Bulma.Heading]
                    prop.text (sprintf "Pokemon Types")
                ]
                Html.div [
                    prop.style [style.marginTop 5; style.marginBottom 10]
                    prop.classes [Bulma.Select; Bulma.IsPrimary]
                    prop.children [
                        Html.select[
                            prop.onChange (boo dispatch)
                            prop.children optionLi
                        ]
                    ]
                ]
                Html.h1 [
                    prop.text (sprintf "Currently: %A" state.CurrentPokemonTypeId)
                ]
                Html.button[
                    prop.classes [ Bulma.Button; Bulma.IsMedium ]
                    prop.text "Fetch"
                    prop.onClick 
                        (fun _ ->
                            let r = powerFetch2 "http://fable.io" |>  Async.AwaitPromise |> Async.RunSynchronously
                            //let kkk = getRequest "http://localhost:7071/api/relations/16" |> Async.AwaitPromise |> Async.RunSynchronously
                            //printfn "---> %A" kkk
                            ()
                        )
                ]
            ]
        ]

        

    Program.mkSimple init update render
    |> Program.withReactBatched "app"
    |> Program.run
    