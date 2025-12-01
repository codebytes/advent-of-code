#!/bin/sh

# Check if the day parameter is provided
if [ -z "$1" ]; then
  echo "Usage: $0 <day> [year]"
  exit 1
fi

day=$1
year=${2:-2025}

# Format the day to be two digits
dayPadded=$(printf "%02d" $day)
basePath="$year/$dayPadded"
csPath="$basePath/cs"

# Create the folder structure
mkdir -p "$basePath"
mkdir -p "$csPath"

# Create the input.txt and sample.txt files
touch "$basePath/input.txt"
touch "$basePath/sample.txt"

# Navigate to the cs folder and run dotnet new console
cd "$csPath"
dotnet new console

# Set the contents of Program.cs
programCsContent=$(cat <<EOF
//https://adventofcode.com/$year/day/$day
var input = await File.ReadAllTextAsync("../sample.txt");
//var input = await File.ReadAllTextAsync("../input.txt");
EOF
)

echo "$programCsContent" > Program.cs

# Return to the original location
cd - > /dev/null