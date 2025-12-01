param (
    [Parameter(Mandatory=$true)]
    [string]$day,
    [string]$year = "2025"
)

$dayPadded = "{0:D2}" -f [int]$day
$basePath = "$year/$dayPadded"
$csPath = "$basePath/cs"

# Create the folder structure
New-Item -ItemType Directory -Path $basePath -Force
New-Item -ItemType Directory -Path $csPath -Force

# Create the input.txt and sample.txt files
New-Item -ItemType File -Path "$basePath/input.txt" -Force
New-Item -ItemType File -Path "$basePath/sample.txt" -Force

# Navigate to the cs folder and run dotnet new console
Set-Location -Path $csPath
dotnet new console

# Set the contents of program.cs
$programCsContent = @"
//https://adventofcode.com/$year/day/$day
var input = await File.ReadAllTextAsync("../sample.txt");
//var input = await File.ReadAllTextAsync("../input.txt");
"@

Set-Content -Path "Program.cs" -Value $programCsContent

# Return to the original location
Set-Location -Path (Get-Location).Path
