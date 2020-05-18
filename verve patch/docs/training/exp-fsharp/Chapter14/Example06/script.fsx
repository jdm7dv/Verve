#light
open System
open WebReferences

// ----------------------------

#r @"C:\fsharp\WeatherForecast.dll"
#r @"C:\fsharp\TerraService.dll"

let ws = new WeatherForecast()
let weather = ws.GetWeatherByPlaceName("Los Angeles");;
let today = weather.Details.[0];;
printf "Temperature: %sF/%sC\n" today.MaxTemperatureF today.MaxTemperatureC;;
let ts = new TerraService();;
let place = new Place(City="Los Angeles", State="CA", Country="USA");;
let facts = ts.GetPlaceFacts(place);;
printfn "Lat/Lon: %f/%f" facts.Center.Lat facts.Center.Lon;;

// ----------------------------

type WebReferences.WeatherForecast with 
    member ws.GetWeatherByPlaceNameAsyncr(placeName) =
        Async.BuildPrimitive(placeName,
                             ws.BeginGetWeatherByPlaceName,
                             ws.EndGetWeatherByPlaceName)
        
type WebReferences.TerraService with 
    member ws.GetPlaceFactsAsyncr(place) =
        Async.BuildPrimitive(place,
                             ws.BeginGetPlaceFacts,
                             ws.EndGetPlaceFacts)

let getWeather(city,state,country) = 
    async { let ws = new WeatherForecast()
            let ts = new TerraService()
            let place = new Place(City=city, State=state, Country=country)
            let! weather,facts = 
                Async.Parallel2 
                    (ws.GetWeatherByPlaceNameAsyncr(city),
                     ts.GetPlaceFactsAsyncr(place))
            let today = weather.Details.[0]
            return (today.MinTemperatureF,today.MaxTemperatureC,
                    facts.Center.Lat,facts.Center.Lon) }

Async.Run (async { let! (maxF,maxC,lat,lon) = getWeather("Los Angeles","CA","USA")
                   do printfn "Temperature: %sF/%sC" maxF maxC 
                   do printfn "Lat/Lon: %f/%f" lat lon })
