#!/bin/bash
set -e

# Script to push NuGet packages to NuGet.org
# Usage: ./push-nuget-packages.sh [--mock]

MOCK_MODE=false
[[ "$1" == "--mock" ]] && MOCK_MODE=true

if ! ls ./artifacts/*.nupkg 1> /dev/null 2>&1; then
    echo "No packages found to upload"
    exit 1
fi

echo "=== Found packages to upload ==="
ls ./artifacts/*.nupkg | while read -r pkg; do
    echo "$(basename "$pkg")"
done
echo "=================================="

UPLOADED_COUNT=0
CONFLICT_COUNT=0
FAILED_COUNT=0

for package in ./artifacts/*.nupkg; do
    PACKAGE_NAME=$(basename "$package")
    echo "Attempting to push: $PACKAGE_NAME"

    if [[ "$MOCK_MODE" == "true" ]]; then
        echo "🟢 Successfully uploaded (mocked): $PACKAGE_NAME"
        UPLOADED_COUNT=$((UPLOADED_COUNT + 1))
    elif output=$(dotnet nuget push "$package" \
        --api-key "$NUGET_API_KEY" \
        --source "https://api.nuget.org/v3/index.json" 2>&1); then
        echo "🟢 Successfully uploaded: $PACKAGE_NAME"
        UPLOADED_COUNT=$((UPLOADED_COUNT + 1))
    else
        if echo "$output" | grep -q "409 (Conflict"; then
            echo "🟡 Conflict (already exists): $PACKAGE_NAME"
            CONFLICT_COUNT=$((CONFLICT_COUNT + 1))
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
echo "Conflicts (409): $CONFLICT_COUNT packages"
echo "Failed uploads: $FAILED_COUNT packages"

# Fail if any errors were encountered
TOTAL_FAILED=$((FAILED_COUNT + CONFLICT_COUNT))

if [ $TOTAL_FAILED -gt 0 ]; then
    echo ""
    if [ $CONFLICT_COUNT -gt 0 ]; then
        echo "❌ ERROR: $TOTAL_FAILED package(s) failed to upload (of which $CONFLICT_COUNT were conflicts)"
        echo "Note: Conflicts should not happen as build-and-pack-nuget.sh filters existing packages."
    else
        echo "❌ ERROR: $FAILED_COUNT package(s) failed to upload"
    fi
    exit 1
fi

if [ $UPLOADED_COUNT -eq 0 ]; then
    echo ""
    echo "❌ ERROR: No packages were successfully uploaded"
    exit 1
fi

echo ""
echo "✅ Successfully pushed $UPLOADED_COUNT package(s) to NuGet.org"
exit 0