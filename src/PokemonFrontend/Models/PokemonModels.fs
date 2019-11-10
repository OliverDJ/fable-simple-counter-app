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
        type Relations =
            {
                StrongAgainst : PokemonType array
                NotEffective : PokemonType array
                WeakAgainst : PokemonType array
                ResistantAgainst : PokemonType array
                DoesNotEffect: PokemonType array
                ImmuneAgainst: PokemonType array
            }