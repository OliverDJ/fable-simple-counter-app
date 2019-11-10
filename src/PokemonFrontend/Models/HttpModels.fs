
namespace Api

    module HttpModels =
        
        type RemoteData<'a> =
            | HasNotLoaded
            | Loading
            | FinishedLoading of 'a

