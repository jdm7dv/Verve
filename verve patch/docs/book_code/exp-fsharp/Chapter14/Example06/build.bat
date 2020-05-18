@ECHO OFF
SET param=%1
IF (%param%) == (clean) GOTO CLEAN

:BUILD
IF NOT EXIST WeatherForecast.cs wsdl /namespace:WebReferences http://webservicex.net/WeatherForecast.asmx
IF NOT EXIST WeatherForecast.dll csc /target:library WeatherForecast.cs
IF NOT EXIST TerraService.cs wsdl /namespace:WebReferences http://terraservice.net/terraservice.asmx
IF NOT EXIST TerraService.dll csc /target:library TerraService.cs
fsc -r WeatherForecast.dll -r TerraService.dll -o AsyncCall.exe main.fs
exit

:CLEAN
del WeatherForecast.cs WeatherForecast.dll
del TerraService.cs TerraService.dll
del *.exe
