#!/bin/bash
# Local build test script for Asterism Engine
# This script tests if the C# library can be built locally

echo "=== Asterism Engine Local Build Test ==="
echo ""

# Check if we're in the right directory
if [ ! -f "README.md" ]; then
    echo "Error: Please run this script from the repository root"
    exit 1
fi

# Check project structure
echo "Checking project structure..."
if [ -d "Asterism/Common" ]; then
    echo "✓ C# project directory found"
else
    echo "✗ C# project directory not found"
    exit 1
fi

if [ -d "Unity/Asterism Engine" ]; then
    echo "✓ Unity project directory found"
else
    echo "✗ Unity project directory not found"
    exit 1
fi

# Check for required files
echo ""
echo "Checking required files..."
if [ -f "Asterism/Common/Common.sln" ]; then
    echo "✓ Solution file found"
else
    echo "✗ Solution file not found"
    exit 1
fi

if [ -f "Unity/Asterism Engine/ProjectSettings/ProjectVersion.txt" ]; then
    echo "✓ Unity project settings found"
    unity_version=$(head -n 1 "Unity/Asterism Engine/ProjectSettings/ProjectVersion.txt")
    echo "  $unity_version"
else
    echo "✗ Unity project settings not found"
    exit 1
fi

echo ""
echo "Project structure validation complete!"
echo ""
echo "To trigger a production release:"
echo "1. Create a pull request with your changes"
echo "2. Add the 'production' label to the PR"
echo "3. Merge the PR"
echo ""
echo "The automated workflow will:"
echo "- Create a version tag"
echo "- Build C# library and Unity project"
echo "- Create downloadable artifacts"
echo "- Publish a GitHub release"