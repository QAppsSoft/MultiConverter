dotnet publish "src\MultiConverter.Desktop\MultiConverter.Desktop.csproj" -p:DebugType=None -p:DebugSymbols=false -p:PublishSingleFile=true -r win10-x64 -c Release --self-contained true -p:PublishTrimmed=true -p:IncludeNativeLibrariesForSelfExtract=true -o "publish\"