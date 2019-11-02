# Counter App with Fable

## Prerequisites
- npm
- paket
- dotnet (with support for netcoreapp2.2)

## Create folder for entire project
```
mkdir counter-app
cd counter-app
```

## Initialize package managers
```
npm init
paket init
```

## Create new F# library project
```
mkdir src
cd src
dotnet new classlib --language f# --framework netcoreapp2.2 --name CounterApp
```

## Add paket packages
Navigate to *src/CounterApp/*
```
paket add Fable.Core --project CounterApp.fsproj
paket add Fable.Elmish.React --project CounterApp.fsproj
paket add Fable.Elmish.Browser --project CounterApp.fsproj
paket add Fable.Elmish.Debugger --project CounterApp.fsproj
paket add Fable.Elmish.HMR --project CounterApp.fsproj
paket add Fable.Fetch --project CounterApp.fsproj
```

## Create solution file (.sln)
Navigate to */counter-app* (root)
```
dotnet new sln --name CounterApp
dotnet sln CounterApp.sln add src\CounterApp\CounterApp.fsproj
```

## Setup Index.html
Create public folder and add index.html
```
mkdir public
touch public\index.html
```

Add this code to the index.html file
```html
<!doctype html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport"
          content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Fable Counter App</title>
    <link rel="stylesheet" href="/styles.css">
</head>
<body>
    <div id="app"></div>
    <script type="text/javascript" src="/bundle.js"></script>
</body>
</html>
```
*NOTE!* `<div id="app"></div>`; remember the `id="app"`, we need this id-string for later.


## Setup npm (package.json)
Replace package.json with the following
```Json
{
  "name": "counter-app",
  "version": "1.0.0",
  "description": "",
  "main": "index.js",
  "devDependencies": {
    "@babel/core": "^7.5.5",
    "fable-compiler": "^2.3.19",
    "fable-loader": "^2.1.8",
    "remotedev": "^0.2.9",
    "webpack": "^4.39.2",
    "webpack-cli": "^3.3.6",
    "webpack-dev-server": "^3.8.0"
  },
  "scripts": {
    "start": "webpack-dev-server",
    "build": "webpack -p",
    "postinstall": "paket restore && paket generate-load-scripts -t fsx"
  },
  "dependencies": {
    "copy-webpack-plugin": "^5.0.4",
    "html-webpack-plugin": "^3.2.0",
    "mini-css-extract-plugin": "^0.8.0",
    "react": "^16.9.0",
    "react-dom": "^16.9.0",
    "react-select": "^3.0.4"
  }
}
```

## Setup webpack

Navigate to *counter-app/* (root) 
```
touch webpack.config.js
```
Add the following code
```Javascript
const path = require("path");

module.exports = {
    entry: path.join(__dirname, "src", "CounterApp/CounterApp.fsproj"),
    output: {
        path: path.join(__dirname, "public"),
        filename: "bundle.js",
        chunkFilename: "[name].bundle.js",
        publicPath: "/"
    },
    optimization: {
        splitChunks: {
            chunks: "async"
        }
    },
    devServer: {
        contentBase: path.join(__dirname, "public"),
        historyApiFallback: true
    },
    module: {
        rules: [
            {
                test: /\.fs(x|proj)?$/,
                use: "fable-loader"
            }
        ]
    }
};
```
*Note*! `entry: path.join(__dirname, "src", "CounterApp/CounterApp.fsproj")` must match with the name of you project. It can also reference a .fsx file.



## Setup F# (Fable)
In Library.fs(src/CounterApp/Library.fs)
replace content with:
```fsharp
module App
    open Fable.React
    open Fable.React.Props
    open Elmish
    open Elmish.React
    module c = Fable.React.ReactBindings
    module R = Fable.React.Helpers

    type State = 
        {
            Counter : int
        }
        
    type Msg = Increment | Decrement

    let init () =
        {
            Counter = 0
        }

    let update (msg:Msg) (state:State) =
      match msg with
      | Increment -> { state with Counter = state.Counter + 1}
      | Decrement -> { state with Counter = state.Counter - 1}
        
    let render (state: State) dispatch =
        div [ClassName "foo"][//div
            h1 [] [str "Fable Counter"]
            button [ OnClick (fun _ -> dispatch Decrement) ] [ R.str "-" ]
            h1 [] [R.str (state.Counter.ToString())]
            button [ OnClick (fun _ -> dispatch Increment) ] [ R.str "+" ]
        ]
            
    Program.mkSimple init update render
    |> Program.withReactBatched "app"
    |> Program.run
```
*NOTE*: 
In the step regarding index.html I told you to remember `id="app"`. This is where we need that id-string. 
The string `"app"` in `Program.withReactBatched "app"`must correspond with `id="app"` in the index.html file for the program to work.


## Run package managment installations
```
npm install
paket install
```

## Run you application (defaults to port 8080)
```
npm start
```



## Styling with Css

There are some issues related to the out-of-the-box css styling provided by fable.
Zaid-Ajaj has created a library that lets us write more react-style code in fable (
https://github.com/Zaid-Ajaj/Feliz). It allows to easily add styling, class-names etc. 

This alone will do just fine. However, zanaptak (https://github.com/zanaptak/TypedCssClasses) has created a strongly typed Css Type-Provider! You never have to reference classnames and classes with strings ever again (e.g. "button is-primary").



### Installing dependencies
Navigate to *src/CounterApp/*
```
paket add Feliz --project CounterApp.fsproj
paket add Zanaptak.TypedCssClasses --project CounterApp.fsproj
```

### Altering code
Some of the Fable html object (e.g. div, button) now has to be replaced with Html.div and Html.button respectively (from Felize). We examplify with Bulma, a css framework.

Example:
```fsharp
//old import is still required here
open Feliz
open Zanaptak.TypedCssClasses

type Bulma = CssClasses<"https://cdn.jsdelivr.net/npm/bulma@0.8.0/css/bulma.min.css", Naming.PascalCase>

let render (state: State) dispatch =
    Html.div[
    Html.button [ 
        prop.classes [ Bulma.Button; Bulma.IsPrimary; Bulma.IsMedium] 
        ]
    ]   
```

The Css styled Counter-app can now be written like this.

```fsharp
module App

    open Fable.React
    open Elmish
    open Elmish.React
    module c = Fable.React.ReactBindings
    module R = Fable.React.Helpers
    open Feliz
    open Zanaptak.TypedCssClasses
    
    type FA = CssClasses<"https://use.fontawesome.com/releases/v5.8.1/css/all.css", Naming.PascalCase>
    type Bulma = CssClasses<"https://cdn.jsdelivr.net/npm/bulma@0.8.0/css/bulma.min.css", Naming.PascalCase>
    
    type State = 
        {
            Counter : int
        }
        
    type Msg = Increment | Decrement

    let init () =
        {
            Counter = 0
        }

    let update (msg:Msg) (state:State) =
      match msg with
      | Increment -> { state with Counter = state.Counter + 1}
      | Decrement -> { state with Counter = state.Counter - 1}
        
    let render (state: State) dispatch =
        Html.div[
            prop.classes [ Bulma.Control]
            prop.style [style.textAlign.center; style.marginTop 50]
            prop.children [
                Html.button [
                    prop.classes [ Bulma.Button; Bulma.IsPrimary; Bulma.IsMedium ]
                    prop.onClick (fun _ -> dispatch Increment)
                    prop.children [ 
                        Html.i [ prop.classes [ FA.Fa; FA.FaPlus ] ]
                    ]
                ]
                Html.h1 [
                    prop.className "title"
                    prop.style [style.marginTop 20; ]

                    prop.text state.Counter
                ]
                Html.button [
                    prop.classes [ Bulma.Button; Bulma.IsPrimary; Bulma.IsMedium ]
                    prop.onClick (fun _ -> dispatch Decrement)
                    prop.children [ 
                        Html.i [ prop.classes [ FA.Fa; FA.FaMinus ] ]
                    ]
                ]
             ]
        ]

    Program.mkSimple init update render
    |> Program.withReactBatched "app"
    |> Program.run
```

### Index.html
Additionally, two link-references have to be added to the index.html
The first is for Bulma (css framework), and the second for FontAwesome (icon library)
``` html
<link rel="stylesheet" 
    href="https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.4/css/bulma.min.css" />

<link rel="stylesheet" 
    href="https://use.fontawesome.com/releases/v5.8.1/css/all.css" 
    integrity="sha384-50oBUHEmvpQ+1lW4y57PTFmhCaXp0ML5d60M1M7uH2+nqUivzIebhndOJK28anvf" 
    crossorigin="anonymous" />

```

The index.html page should look something like this.
```html
<!doctype html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport"
          content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Fable Counter App</title>
    <link rel="stylesheet" href="/styles.css">
    <link rel="stylesheet" 
        href="https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.4/css/bulma.min.css" />
    <link rel="stylesheet" 
        href="https://use.fontawesome.com/releases/v5.8.1/css/all.css" 
        integrity="sha384-50oBUHEmvpQ+1lW4y57PTFmhCaXp0ML5d60M1M7uH2+nqUivzIebhndOJK28anvf" 
        crossorigin="anonymous" />
    
</head>
<body>
    <div id="app"></div>
    <script type="text/javascript" src="/bundle.js"></script>
</body>
</html>
```