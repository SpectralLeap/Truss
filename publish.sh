#!/bin/bash

output_directory=".nuget_output"
mkdir -p "$output_directory"
rm -rf $output_directory/refs
rm $output_directory/*

projects=(
    "Truss.Monads.Results"
    "Truss.Monads.Results.Protos"
    "Truss.Monads.Results.Extensions.Fluent"
    "Truss.Modeling.Domain"
    "Truss.Modeling.Application"
    "Truss.Testing"
    "Truss.Testing.AspNetCore"
)

for project in "${projects[@]}"
do
    echo "Building $project..."
    dotnet build "$project/$project.csproj" --configuration Release --output $output_directory
    
    if [ $? -ne 0 ]; then
        echo "Build failed for $project"
        exit 1
    fi
done

api_key=$NUGET_API_KEY
server_url=$NUGET_SERVER_URL

find $output_directory -name '*.nupkg' | while read -r package_file
do
    echo "Pushing $package_file to $server_url..."
    dotnet nuget push "$package_file" --source "$server_url" --api-key "$api_key"
done

echo "Build and push process for latest packages completed."