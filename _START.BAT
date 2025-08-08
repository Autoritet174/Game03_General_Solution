dotnet publish -c Release -o ./publish
cd ./publish
dotnet Server.dll
pause