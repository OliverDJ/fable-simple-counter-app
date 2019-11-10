namespace Api

    module ProgramModels =
        open PokemonModels
        open HttpModels

        type Msg =
            | SetCurrentPokemonId of int
            | GetPokemonTypes
            | PokemonTypesReceived of PokemonTypes
            | ErrorWhileReceivingData of string


        type State = 
            {
                PokemonTypes : RemoteData<Result<PokemonTypes, string>>
                CurrentPokemonTypeId : int
            }