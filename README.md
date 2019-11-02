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
*NOTE!* `<div id="app"></div>`; remember the Id="app", we need this name for later


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
            h1 [] [str "Hello world"]
            button [ OnClick (fun _ -> dispatch Decrement) ] [ R.str "-" ]
            h1 [] [R.str (state.Counter.ToString())]
            button [ OnClick (fun _ -> dispatch Increment) ] [ R.str "+" ]
        ]
            
    Program.mkSimple init update render
    |> Program.withReactBatched "app"
    |> Program.run
```
NOTE: The string "app" in `Program.withReactBatched "app"`must correspond with the id="app" in index.html


## Run package managment installations
```
npm install
paket install
```

## Run you application (defaults to port 8080)
```
npm start
```