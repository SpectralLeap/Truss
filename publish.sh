#!/bin/bash

output_directory=".nuget_output"
mkdir -p "$output_directory"
rm $output_directory/*

if uname -r | grep -q Microsoft; then
    echo "Running on WSL"
    powershell.exe dotnet build --configuration Release
    powershell.exe dotnet pack -o $output_directory
else
    echo "Running on Linux"
    dotnet build --configuration Release
    dotnet pack -o $output_directory
fi

api_key=$NUGET_API_KEY
server=$NUGET_SERVER

find $output_directory -name '*.nupkg' | while read -r package_file
do
    echo "Pushing $package_file to $server..."
    dotnet nuget push "$package_file" --source "$server" --api-key "$api_key"
done

echo "Build and push process for latest packages completed."
