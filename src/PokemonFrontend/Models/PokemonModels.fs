namespace Api

    module PokemonModels =


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
        type SelectedPokemonTypeId =
            | NotSelected
            | Selected of int