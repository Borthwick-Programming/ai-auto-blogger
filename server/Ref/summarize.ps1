param(
    # By default: directory that contains WorkflowEngine.sln
    [string]$RootDir      = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path,

    # Output folder *inside* the root (change if you like)
    [string]$OutDir       = 'docs\primer',

    [int]   $SwaggerPort  = 5189
)

# ---------- helpers --------------------------------------------------
function Ensure-Dir([string]$Path) {
    if (-not (Test-Path $Path)) {
        New-Item -ItemType Directory -Path $Path | Out-Null
    }
}

function Dump-Packages([string]$Proj, [string]$Out) {
    $name = [System.IO.Path]::GetFileNameWithoutExtension($Proj)
    dotnet list "$Proj" package > "$Out\package-list-$name.txt"
}

Push-Location $RootDir    # ← all dotnet / tree calls now run at solution root
# make $OutDir an absolute path so it lands under the root
$OutDir = Join-Path -Path $RootDir -ChildPath $OutDir

# ---------- begin ----------------------------------------------------
Ensure-Dir $OutDir
Write-Host "Dumping two-level file tree ..."
tree /F /A > "$OutDir\solution-structure.txt"

Write-Host "Listing projects in solution ..."
dotnet sln list > "$OutDir\sln-projects.txt"

Write-Host "Gathering NuGet package lists ..."
Get-ChildItem -Recurse -Filter *.csproj |
    ForEach-Object { Dump-Packages $_.FullName $OutDir }

Write-Host "Attempting to grab Swagger JSON (if Api project exists) ..."
$apiProj = Get-ChildItem -Recurse -Filter *Api.csproj | Select-Object -First 1
if ($apiProj) {

    $runArgs = @(
        "run"
        "--project", $apiProj.FullName
        "--no-build"
        "--urls", "http://localhost:$SwaggerPort"
    )

    $proc = Start-Process "dotnet" -ArgumentList $runArgs `
            -PassThru -NoNewWindow `
            -RedirectStandardOutput "$OutDir\api-run.log" `
            -RedirectStandardError  "$OutDir\api-run.err"

    Start-Sleep 5   # give the API a moment to spin up

    try {
        Invoke-WebRequest "http://localhost:$SwaggerPort/swagger/v1/swagger.json" `
            -OutFile "$OutDir\api-summary.json" -UseBasicParsing
        Write-Host "Swagger JSON saved to api-summary.json"
    } catch {
        Write-Warning "Swagger download failed (no Swashbuckle?)"
    } finally {
        if ($proc -and !$proc.HasExited) { Stop-Process -Id $proc.Id -Force }
    }

} else {
    Write-Host "No *Api.csproj found -- skipping Swagger step."
}

Write-Host "Primer files generated in $OutDir"
