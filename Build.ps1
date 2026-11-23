$ErrorActionPreference = 'Stop'
dotnet build "$PSScriptRoot/SubnauticaOptimizationMod.csproj" -c Release --nologo
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }
