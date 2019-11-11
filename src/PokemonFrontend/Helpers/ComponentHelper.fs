namespace Api
    module ComponentHelper =
        
        let maybeComponent a = (a |> Option.map id |> Option.defaultValue [])

        