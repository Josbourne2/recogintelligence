param(
    [Parameter(Mandatory)][string] $Version,
    [Parameter(Mandatory)][string] $OutputDirectory
)
$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
$projects = @('Shiny.FaceIntelligence', 'Shiny.FaceIntelligence.DocumentDb',
    'Shiny.FaceIntelligence.DocumentDb.Sqlite', 'Shiny.FaceIntelligence.Onnx', 'Shiny.FaceIntelligence.Maui')
foreach ($project in $projects) {
    $path = Join-Path $repo "src/$project/$project.csproj"
    & dotnet pack $path -c Release "-p:NuGetPackageVersion=$Version" "-p:PackageVersion=$Version" `
        -p:GeneratePackageOnBuild=false -o $OutputDirectory --verbosity quiet
    if ($LASTEXITCODE -ne 0) { throw "Packing $project failed." }
}
