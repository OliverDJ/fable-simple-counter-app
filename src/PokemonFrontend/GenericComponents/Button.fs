
namespace Api
    module Button =
        open Feliz
        open Css


        let renderSimpleButton (text: string) onClick =
            Html.button[
                prop.classes [ Bulma.Button; Bulma.IsMedium ]
                prop.text text
                prop.onClick onClick
            ]
            

