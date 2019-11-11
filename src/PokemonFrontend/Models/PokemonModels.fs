namespace Api

    module PokemonModels =

        type SelectedPokemonType =
            | NotSelected
            | Selected of int

        [<CLIMutable>]
        type PokemonType =
            {
                Id : int
                Name : string
                Color : string
            }

        [<CLIMutable>]
        type PokemonTypes = PokemonType list

        [<CLIMutable>]
        type PokemonRelations =
            {
                StrongAgainst : PokemonTypes
                NotEffective : PokemonTypes
                WeakAgainst : PokemonTypes
                ResistantAgainst : PokemonTypes
                DoesNotEffect: PokemonTypes
                ImmuneAgainst: PokemonTypes
            }