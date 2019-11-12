
namespace Api


    module PokemonTypesButtons =
        open PokemonModels
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


        let setCurrentId f (x: Browser.Types.MouseEvent) = 
            let id = x.Value |> int
            SetCurrentPokemonId id |> f

        let renderTypeButton (dispatch: Msg -> unit) (pokemonType: PokemonType) =
            Html.button[
                prop.value pokemonType.Id
                prop.style [style.backgroundColor pokemonType.Color; style.color "#ffffff"]
                prop.classes[Bulma.Button; Bulma.IsRounded; Bulma.IsFamilySecondary]
                prop.onClick (setCurrentId dispatch)
                prop.text pokemonType.Name
            ]



        let renderPokemonTypeButtons (dispatch: Msg -> unit) (pokemonTypes: PokemonTypes) = 
            Html.ul [
                prop.children [ for p in pokemonTypes -> renderTypeButton dispatch p]
            ]

        let renderPokemonButtons(pokemonTypeState: RemoteData<Result<PokemonTypes, string>>) (dispatch: Msg -> unit)=
            match pokemonTypeState with
            | FinishedLoading (Ok data) -> data |> renderPokemonTypeButtons dispatch
            | _ -> div[][]


        let tryFindPokemon (id:int) (pokemonTypes : PokemonTypes) =
            pokemonTypes |> List.tryFind (fun x -> x.Id = id)

        
        let renderSelected (p: PokemonTypes) (id: int) =
            let r = (id, p ) ||> tryFindPokemon
            let s = r |> Option.map (fun x -> x.Name) |> Option.defaultValue "None"
            Html.h2 [
                   prop.style [style.marginTop 20; style.marginBottom 10]
                   prop.classes [Bulma.Heading]
                   prop.text ( sprintf "Selected Pokemon: %s" s)
                ]
            
           

        let renderSelectedPokemon (p:RemoteData<Result<PokemonTypes, string>>) (s: SelectedPokemonTypeId) =
            match p, s with
            | FinishedLoading (Ok data), Selected id ->  (data, id) ||> renderSelected
            | _ ,_ -> Html.h2 "Oops"
            
        