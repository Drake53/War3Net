#!/bin/bash
set -e

# Script to build and pack NuGet packages in dependency order
# Usage: ./build-and-pack-nuget.sh [--skip-version-check]

SKIP_VERSION_CHECK=false
[[ "$1" == "--skip-version-check" ]] && SKIP_VERSION_CHECK=true

# Create artifacts directory for local NuGet feed
mkdir -p ./artifacts

# Get all packable projects from the solution filter
# Extract project paths from the solution filter and convert Windows paths to Unix paths
PROJECTS=$(jq -r '.solution.projects[]' War3NetPublish.slnf | sed 's/\\/\//g' | grep -v "Tests" | tr '\n' ';')

echo "=== Found projects to publish ==="
echo "$PROJECTS" | tr ';' '\n'
echo "=================================="

# Create a temporary file to track which projects have been built
# Using semicolon delimiters for exact matching
BUILT_PROJECTS=";"

# Function to check if all dependencies of a project are built
can_build_project() {
  local project=$1
  # Get the dependencies - dotnet list outputs them with paths like ..\ProjectName\ProjectName.csproj
  # We need to extract just the project name from the filename
  local deps=$(dotnet list "$project" reference 2>/dev/null | grep '\.csproj$' | while read -r line; do
    # Remove everything up to the last backslash, then remove .csproj extension
    echo "$line" | sed 's/.*\\//' | sed 's/\.csproj$//'
  done | tr '\n' ' ')
  
  for dep in $deps; do
    if ! echo "$BUILT_PROJECTS" | grep -q ";$dep;"; then
      return 1
    fi
  done
  return 0
}

# Build projects in dependency order
REMAINING_PROJECTS="$PROJECTS"
ITERATION=0
MAX_ITERATIONS=20

while [ -n "$REMAINING_PROJECTS" ] && [ $ITERATION -lt $MAX_ITERATIONS ]; do
  ITERATION=$((ITERATION + 1))
  
  PROJECTS_TO_BUILD=""
  STILL_REMAINING=""
  
  # Process each project using semicolon as delimiter
  while IFS= read -r project; do
    if [ -n "$project" ]; then
      if can_build_project "$project"; then
        PROJECTS_TO_BUILD="${PROJECTS_TO_BUILD}${project};"
      else
        STILL_REMAINING="${STILL_REMAINING}${project};"
      fi
    fi
  done < <(echo "$REMAINING_PROJECTS" | tr ';' '\n')
  
  if [ -z "$PROJECTS_TO_BUILD" ] && [ -n "$STILL_REMAINING" ]; then
    echo ""
    echo "ERROR: Circular dependency detected or unable to resolve dependencies"
    echo "Projects that cannot be built:"
    echo "$STILL_REMAINING" | tr ';' '\n' | while read -r p; do
      if [ -n "$p" ]; then
        echo "  - $(basename $(dirname "$p"))"
        echo "    Dependencies: $(dotnet list "$p" reference 2>/dev/null | grep -E "^\s+.*\.csproj" | sed 's/.*\///' | sed 's/\.csproj.*//' | tr '\n' ' ')"
      fi
    done
    echo "Already built: $BUILT_PROJECTS"
    exit 1
  fi
  
  echo ""
  echo "Projects to build in iteration $ITERATION:"
  if [ -n "$PROJECTS_TO_BUILD" ]; then
    OLD_IFS="$IFS"
    IFS=';'
    for p in $PROJECTS_TO_BUILD; do
      [ -n "$p" ] && echo "  - $(basename $(dirname "$p"))"
    done
    IFS="$OLD_IFS"
  else
    echo "  (none)"
  fi
  
  # Build and pack projects that are ready
  while IFS= read -r project; do
    if [ -z "$project" ]; then continue; fi
    PROJECT_NAME=$(basename $(dirname "$project"))

    # Check if we should skip version checking (for PR validation)
    if [ "$SKIP_VERSION_CHECK" = true ]; then
      echo "Building $PROJECT_NAME (version check skipped)..."
      SHOULD_BUILD=true
    else
      # Get the PackageId from the project file by evaluating the MSBuild property
      PACKAGE_ID=$(dotnet msbuild "$project" -getProperty:PackageId -p:Configuration=Release 2>/dev/null | tail -1)

      # If PackageId is not set, fall back to project name
      if [ -z "$PACKAGE_ID" ]; then
        PACKAGE_ID="$PROJECT_NAME"
      fi

      # Get the version - submodule projects use <Version> property, main projects use CPM
      if [[ "$project" == *submodules/* ]]; then
        # Submodule projects have their own Version property
        VERSION=$(dotnet msbuild "$project" -getProperty:Version -p:Configuration=Release -nologo 2>/dev/null | grep -v "^$" | tail -1)
      else
        # Main projects use CPM - the version is set by the SetProjectVersionsFromCentralPackageManagement target
        # We suppress stderr as GetAssemblyVersion task may not be found but PackageVersion is still set correctly
        VERSION=$(dotnet msbuild "$project" -t:SetProjectVersionsFromCentralPackageManagement -getProperty:PackageVersion -p:Configuration=Release -nologo 2>/dev/null | grep -v "^$" | tail -1)
      fi

      if [ -z "$VERSION" ]; then
        echo "❌ ERROR: Could not extract version from $PACKAGE_ID, skipping"
        if [[ "$project" == *submodules/* ]]; then
          echo "  Make sure the project has a <Version> property defined"
        else
          echo "  Make sure the project has a version defined in Directory.Packages.props"
        fi
        SHOULD_BUILD=false
      else
        echo "Checking if $PACKAGE_ID $VERSION exists on NuGet.org..."

        # Query NuGet API to check if this version exists
        API_URL="https://api.nuget.org/v3/registration5-semver1/${PACKAGE_ID,,}/index.json"

        if response=$(curl -s -f "$API_URL" 2>/dev/null); then
          # Check if the specific version exists in the response
          if echo "$response" | grep -q "\"$VERSION\""; then
            echo "  ⚪ Already exists on NuGet.org, skipping build"
            SHOULD_BUILD=false
          else
            echo "  🟢 New version, will build"
            SHOULD_BUILD=true
          fi
        else
          # Package ID doesn't exist at all, so this version is definitely new
          echo "  🟢 New package, will build"
          SHOULD_BUILD=true
        fi
      fi
    fi

    if [ "$SHOULD_BUILD" = true ]; then
      echo "Building $PROJECT_NAME..."

      # Restore with local feed for dependencies from previous iterations
      dotnet restore "$project" -p:Configuration=Release --verbosity minimal --force --no-cache

      # Build the project
      dotnet build "$project" --configuration Release --no-restore --verbosity minimal

      # Pack directly to artifacts with project name folder structure for proper NuGet feed
      dotnet pack "$project" --configuration Release --no-build --output "./artifacts/${PROJECT_NAME}" -p:PACK=true --verbosity minimal
    fi

    BUILT_PROJECTS="${BUILT_PROJECTS}${PROJECT_NAME};"
  done < <(echo "$PROJECTS_TO_BUILD" | tr ';' '\n')
  
  # Clear NuGet cache for local packages to ensure newly built packages are available
  dotnet nuget locals temp -c
  
  REMAINING_PROJECTS="$STILL_REMAINING"
done

if [ $ITERATION -eq $MAX_ITERATIONS ]; then
  echo "Error: Maximum iterations reached. Possible circular dependency."
  exit 1
fi

PACKAGE_COUNT=$(ls ./artifacts/*/*.nupkg 2>/dev/null | wc -l)

echo ""
echo "=== Build Summary ==="
echo "Successfully created $PACKAGE_COUNT package(s)"

# Only create zip and check for updates when NOT skipping version check
if [ "$SKIP_VERSION_CHECK" = false ]; then
  if [ $PACKAGE_COUNT -eq 0 ]; then
      echo ""
      echo "No new packages to release (all packages are up-to-date)"
      exit 1
  fi

  echo ""
  echo "Creating release archive with new packages..."
  find ./artifacts -name "*.nupkg" | zip -j "artifacts/Packages.zip" -@
  echo "Created: artifacts/Packages.zip"
fi