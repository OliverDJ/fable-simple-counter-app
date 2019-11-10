namespace Api
    
    module H1 =
        open Feliz
        open Css


        let renderSimpleH1 (text: string) =
            Html.h1 [
                prop.classes [Bulma.Heading]
                prop.text text
            ]


