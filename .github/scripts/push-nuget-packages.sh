#!/bin/bash
set -e

# Script to push NuGet packages and create a .zip file with all uploaded packages
# Usage: ./push-nuget-packages.sh [--mock]

MOCK_MODE=false
[[ "$1" == "--mock" ]] && MOCK_MODE=true

if ! ls ./artifacts/*/*.nupkg 1> /dev/null 2>&1; then
    echo "No packages found to upload"
    exit 1
fi

echo "=== Found packages to upload ==="
ls ./artifacts/*/*.nupkg | while read -r pkg; do
    echo "$(basename "$pkg")"
done
echo "=================================="

UPLOADED_COUNT=0
SKIPPED_COUNT=0
FAILED_COUNT=0
UPLOADED_PACKAGES=""

for package in ./artifacts/*/*.nupkg; do
    PACKAGE_NAME=$(basename "$package")
    echo "Attempting to push: $PACKAGE_NAME"

    if [[ "$MOCK_MODE" == "true" ]]; then
        UPLOADED_PACKAGES="$UPLOADED_PACKAGES$package
"
        echo "🟢 Successfully uploaded (mocked): $PACKAGE_NAME"
        UPLOADED_COUNT=$((UPLOADED_COUNT + 1))
    elif output=$(dotnet nuget push "$package" \
        --api-key "$NUGET_API_KEY" \
        --source "https://api.nuget.org/v3/index.json" 2>&1); then
        UPLOADED_PACKAGES="$UPLOADED_PACKAGES$package
"
        echo "🟢 Successfully uploaded: $PACKAGE_NAME"
        UPLOADED_COUNT=$((UPLOADED_COUNT + 1))
    else
        if echo "$output" | grep -q "409 (Conflict"; then
            echo "⚪ Already exists: $PACKAGE_NAME"
            SKIPPED_COUNT=$((SKIPPED_COUNT + 1))
        else
            echo "🔴 Failed to upload: $PACKAGE_NAME"
            echo "  Error: $output"
            FAILED_COUNT=$((FAILED_COUNT + 1))
        fi
    fi
done

echo ""
echo "=== Upload Summary ==="
echo "Successfully uploaded: $UPLOADED_COUNT packages"
echo "Already existed: $SKIPPED_COUNT packages"
echo "Failed uploads: $FAILED_COUNT packages"

if [ $UPLOADED_COUNT -gt 0 ]; then
    echo ""
    echo "Creating release archive..."
    echo "$UPLOADED_PACKAGES" | zip -j "artifacts/Packages.zip" -@
    echo "Created: artifacts/Packages.zip"
    exit 0
else
    echo "No packages were successfully uploaded"
    exit 1
fi