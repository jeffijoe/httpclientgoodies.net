nuget pack ../HttpClientGoodies.nuspec
nuget push *.nupkg
del /q *.nupkg
