#!/bin/bash

# Prompt for the new project name
read -p "Enter the new project name (e.g., MyAwesomeApi): " new_name

if [ -z "$new_name" ]; then
    echo "Project name cannot be empty."
    exit 1
fi

old_name="MultiTenantApi"

echo "Renaming project from '$old_name' to '$new_name'..."

# 1. Rename solution file
mv "${old_name}.sln" "${new_name}.sln"

# 2. Rename project directories
mv "${old_name}.Api" "${new_name}.Api"
mv "${old_name}.Application" "${new_name}.Application"
mv "${old_name}.Domain" "${new_name}.Domain"
mv "${old_name}.Infrastructure" "${new_name}.Infrastructure"
mv "${old_name}.Tests" "${new_name}.Tests"

# 3. Rename project files
mv "${new_name}.Api/${old_name}.Api.csproj" "${new_name}.Api/${new_name}.Api.csproj"
mv "${new_name}.Application/${old_name}.Application.csproj" "${new_name}.Application/${new_name}.Application.csproj"
mv "${new_name}.Domain/${old_name}.Domain.csproj" "${new_name}.Domain/${new_name}.Domain.csproj"
mv "${new_name}.Infrastructure/${old_name}.Infrastructure.csproj" "${new_name}.Infrastructure/${new_name}.Infrastructure.csproj"
mv "${new_name}.Tests/${old_name}.Tests.csproj" "${new_name}.Tests/${new_name}.Tests.csproj"

# 4. Search and replace in all text files
echo "Updating file contents..."

find . -type f \( -name "*.cs" -o -name "*.sln" -o -name "*.csproj" \) -exec sed -i "s/${old_name}/${new_name}/g" {} +

echo "Project renaming complete."
echo "Please do a manual check and rebuild the solution."
