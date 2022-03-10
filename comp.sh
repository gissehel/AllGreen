rm -rf */bin */obj

dotnet.exe build AllGreen.Lib/AllGreen.Lib.csproj -c Debug -f netstandard2.0
dotnet.exe pack AllGreen.Lib/AllGreen.Lib.csproj -c Debug

