
namespace Api

    module App =

        open Fable.React
        open Elmish
        open Elmish.React
        module c = Fable.React.ReactBindings
        module R = Fable.React.Helpers
        open Feliz
        open PokemonDropdown
        open ProgramModels
        open Css
        open H1
        open Button
        open PokemonModels
        open HttpModels
        open CommandHelper
        open PokemonRelations
        open PokemonTypesButtons


        let init () =
            {
                PokemonTypes = HasNotLoaded
                PokemonRelations = HasNotLoaded
                CurrentPokemonTypeId = NotSelected
            }
            |> withGetPokemonTypesCommand

        

        

        let witSetSelectedCommand state =
            {state with CurrentPokemonTypeId = Selected 1}, Cmd.OfAsync.perform getRelations 1 id

        let update (msg:Msg) (state:State) : State * Cmd<Msg> =
            match msg with
            | SetCurrentPokemonId id -> { state with CurrentPokemonTypeId = Selected id } |> withGetRelationsCommand
            | GetPokemonTypes -> state |> withGetPokemonTypesCommand
            | GetPokemonRelations -> state |> withGetRelationsCommand
            | PokemonTypesReceived pokemonTypes -> {state with PokemonTypes = FinishedLoading (Ok pokemonTypes)} |> witSetSelectedCommand//withoutCommands
            | PokemonRelationsReceived relations -> {state with PokemonRelations = FinishedLoading (Ok relations)} |> withoutCommands

        let render (state: State) dispatch =  
            Html.div[
                prop.classes [ Bulma.Control]
                prop.style [style.textAlign.center; style.marginTop 50; ]
                prop.children[
                    renderSimpleH1 "Pokemon Types" None
                    renderPokemonButtons state.PokemonTypes dispatch
                    //renderSimpleH1 "Selected" None
                    renderSelectedPokemon state.PokemonTypes state.CurrentPokemonTypeId
                    renderAllRelations state.PokemonRelations
                ]
            ]

        Program.mkProgram init update render
        |> Program.withReactBatched "app"
        |> Program.run
    