namespace Api

    module Dropdown =
        open Feliz
        open Css
        open ProgramModels
        open Fable.React
        open Feliz
        open Css
        open ComponentHelper


        let createOption (text: string) (value: int) (styles: IStyleAttribute list option) =
            Html.option[
                prop.style (styles |> maybeComponent)
                prop.text text
                prop.value value
            ]
        //let createOptionList (li: PokemonType list) =
        //    li
        //    |> List.map createPokemonOption

        //let boo f (x : Browser.Types.Event) = 
        //    let id = x.Value |> int
        //    SetCurrentPokemonId id |> f

        //let renderDropdown (state: State) (dispatch: Msg -> unit)=
        //    Html.div [
        //        prop.style [style.marginTop 5; style.marginBottom 10]
        //        prop.classes [Bulma.Select; Bulma.IsPrimary]
        //        prop.children [
        //            Html.select[
        //                prop.onChange (boo dispatch)
        //                prop.children optionLi
        //            ]
        //        ]
        //    ]

