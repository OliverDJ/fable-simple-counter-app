namespace Api
    
    module H1 =
        open Feliz
        open Css
        open ComponentHelper


        let renderSimpleH1 (text: string) (styles: IStyleAttribute list option) =
            Html.h1 [
                prop.style (styles |> maybeComponent)
                prop.classes [Bulma.Heading]
                prop.text text
            ]


