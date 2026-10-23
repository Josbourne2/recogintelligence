param(
    [Parameter(Mandatory)][string] $Version,
    [Parameter(Mandatory)][string] $OutputDirectory
)
$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
if ((& git -C $repo status --porcelain).Count -gt 0) { throw 'Commit the package sources before packing so repository metadata identifies the exact bytes.' }
$configuredVersion = (Get-Content -LiteralPath (Join-Path $repo 'version.json') -Raw | ConvertFrom-Json).version
if ($configuredVersion -ne $Version) { throw 'Commit the requested immutable version in version.json before packing.' }
$projects = @('Shiny.FaceIntelligence', 'Shiny.FaceIntelligence.DocumentDb',
    'Shiny.FaceIntelligence.DocumentDb.Sqlite', 'Shiny.FaceIntelligence.Onnx', 'Shiny.FaceIntelligence.Maui')
foreach ($project in $projects) {
    $path = Join-Path $repo "src/$project/$project.csproj"
    & dotnet pack $path -c Release -p:PublicRelease=true `
        -p:GeneratePackageOnBuild=false -o $OutputDirectory --verbosity quiet
    if ($LASTEXITCODE -ne 0) { throw "Packing $project failed." }
}
