namespace Api

    module ProgramModels =
        open PokemonModels
        open HttpModels

        type Msg =
            | SetCurrentPokemonId of int
            | GetPokemonTypes
            | PokemonTypesReceived of PokemonTypes
            | ErrorWhileReceivingData of string
            | GetPokemonRelations
            | PokemonRelationsReceived of PokemonRelations


        

        type State = 
            {
                PokemonTypes : RemoteData<Result<PokemonTypes, string>>
                PokemonRelations : RemoteData<Result<PokemonRelations, string>>
                CurrentPokemonTypeId : SelectedPokemonType
            }