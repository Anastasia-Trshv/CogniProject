param(
    [string] $Password = "CogniDemo123!"
)

$ErrorActionPreference = "Stop"
$composeFile = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot "..\compose.dev.yml"))

docker compose -f $composeFile exec -e "COGNI_SEED_PASSWORD=$Password" dev_cogni dotnet run --no-launch-profile -- --seed-dev
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}