namespace Api

    module Serde =
        open Fable.SimpleJson

        let bindJson<'a> (str: string) = str |> Json.parseAs<'a>
        let tryBindJson<'a> (str: string) = str |> Json.tryParseAs<'a>

