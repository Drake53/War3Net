#!/bin/bash
set -e

# Script to update package versions in Directory.Build.props based on semver
# Usage: ./update-versions.sh "package1:breaking,package2:feature,package3:fix"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
BUILD_PROPS_FILE="$SCRIPT_DIR/Directory.Build.props"

# Check if Directory.Build.props exists
if [ ! -f "$BUILD_PROPS_FILE" ]; then
    echo "Error: Directory.Build.props not found at $BUILD_PROPS_FILE"
    exit 1
fi

# Function to increment version based on semver
increment_version() {
    local version=$1
    local change_type=$2

    # Parse version components
    local major=$(echo "$version" | cut -d'.' -f1)
    local minor=$(echo "$version" | cut -d'.' -f2)
    local patch=$(echo "$version" | cut -d'.' -f3)

    case "$change_type" in
        "breaking")
            major=$((major + 1))
            minor=0
            patch=0
            ;;
        "feature")
            minor=$((minor + 1))
            patch=0
            ;;
        "fix")
            patch=$((patch + 1))
            ;;
        *)
            echo "Error: Invalid change type '$change_type'. Use: breaking, feature, or fix"
            exit 1
            ;;
    esac

    echo "$major.$minor.$patch"
}

# Function to get version property name for a package
get_version_property() {
    local package_id=$1

    # Convert package ID to version property format by removing dots and adding Version suffix
    # War3Net.Build.Core -> War3NetBuildCoreVersion
    # War3Net.CSharpLua -> War3NetCSharpLuaVersion
    local property_name=$(echo "$package_id" | sed 's/\.//g')
    property_name="${property_name}Version"
    echo "$property_name"
}

# Function to get current version for a package
get_current_version() {
    local package_id=$1
    local version_property=$(get_version_property "$package_id")

    # Extract version from Directory.Build.props
    local version=$(grep "<$version_property>" "$BUILD_PROPS_FILE" | sed "s/.*<$version_property>\(.*\)<\/$version_property>.*/\1/")

    if [ -z "$version" ]; then
        echo "Error: Could not find version for property '$version_property' (package: $package_id)"
        exit 1
    fi

    echo "$version"
}

# Function to update version in Directory.Build.props
update_version_in_props() {
    local package_id=$1
    local new_version=$2
    local version_property=$(get_version_property "$package_id")

    # Update the version in Directory.Build.props using sed
    sed -i "s|<$version_property>.*</$version_property>|<$version_property>$new_version</$version_property>|" "$BUILD_PROPS_FILE"

    echo "Updated $package_id ($version_property) to $new_version"
}

# Caches to avoid repeated expensive operations
declare -A project_dependencies_cache
declare -A project_package_id_cache

# Preload all project dependencies to avoid repeated dotnet calls
preload_all_dependencies() {
    local projects=$(get_all_projects)
    while IFS= read -r project_path; do
        if [ -n "$project_path" ] && [ -z "${project_dependencies_cache[$project_path]}" ]; then
            local deps_array=()
            # Get project references and resolve to package IDs
            while IFS= read -r line; do
                if [[ "$line" == *.csproj ]]; then
                    # Normalize Windows-style paths to Unix-style
                    local ref_project_path=$(echo "$line" | sed 's/^[[:space:]]*//' | sed 's/[[:space:]]*$//' | sed 's/\\/\//g')
                    # Resolve relative paths
                    if [ ! -f "$ref_project_path" ]; then
                        ref_project_path="$(dirname "$project_path")/$ref_project_path"
                    fi

                    if [ -f "$ref_project_path" ]; then
                        local package_id=$(get_project_package_id "$ref_project_path")
                        if [ -n "$package_id" ] && [[ "$package_id" == War3Net* ]]; then
                            deps_array+=("$package_id")
                        fi
                    fi
                fi
            done <<< "$(dotnet list "$project_path" reference 2>/dev/null)"

            local deps_result=$(printf '%s\n' "${deps_array[@]}" | sort -u | tr '\n' ' ' | sed 's/ $//')
            project_dependencies_cache["$project_path"]="$deps_result"
        fi
    done <<< "$projects"
}

# Get package ID for a project (cached)
get_project_package_id() {
    local project_path=$1

    if [ -n "${project_package_id_cache[$project_path]}" ]; then
        echo "${project_package_id_cache[$project_path]}"
        return
    fi

    if [ ! -f "$project_path" ]; then
        project_package_id_cache["$project_path"]=""
        return
    fi

    # Extract PackageId from project file or derive from directory name
    local package_id=$(grep "<PackageId>" "$project_path" | sed 's/.*<PackageId>\(.*\)<\/PackageId>.*/\1/' | head -n1)
    if [ -z "$package_id" ]; then
        local project_name=$(basename "$(dirname "$project_path")")
        if [[ "$project_name" == War3Net* ]]; then
            package_id="$project_name"
        fi
    fi

    project_package_id_cache["$project_path"]="$package_id"
    echo "$package_id"
}

# Get cached project dependencies
get_project_dependencies() {
    echo "${project_dependencies_cache[$1]}"
}

# Get all packable projects from solution filter
get_all_projects() {
    grep '\.csproj"' War3NetPublish.slnf | sed 's/.*"\(.*\)".*/\1/' | sed 's/\\/\//g'
}

# Find projects that depend on a given project
get_dependent_projects() {
    local target_project=$1
    local dependents=""

    local projects=$(get_all_projects)
    while IFS= read -r project_path; do
        if [ -n "$project_path" ]; then
            local deps=$(get_project_dependencies "$project_path")
            if echo "$deps" | grep -q "\b$target_project\b"; then
                local package_id=$(get_project_package_id "$project_path")
                if [ -n "$package_id" ] && [[ "$package_id" == War3Net* ]]; then
                    dependents="$dependents$package_id;"
                fi
            fi
        fi
    done <<< "$projects"

    echo "$dependents"
}

# Determine version bumps for all affected projects
determine_version_bumps() {
    preload_all_dependencies

    declare -A version_bumps
    declare -A processed
    local updates_made=true
    local max_iterations=10
    local iteration=0

    # Apply direct updates from user input
    if [ -n "$DIRECT_UPDATES" ]; then
        IFS=',' read -ra UPDATE_PAIRS <<< "$DIRECT_UPDATES"
        for pair in "${UPDATE_PAIRS[@]}"; do
            IFS=':' read -ra PARTS <<< "$pair"
            local package_id="${PARTS[0]}"
            local change_type="${PARTS[1]}"
            version_bumps["$package_id"]="$change_type"
        done
    fi

    # Determine cascading version bumps
    while [ "$updates_made" = true ] && [ $iteration -lt $max_iterations ]; do
        updates_made=false
        iteration=$((iteration + 1))

        echo "=== Determining cascading updates - Iteration $iteration ===" >&2

        # Process each project with a version bump
        for project in "${!version_bumps[@]}"; do
            if [ -z "${processed[$project]}" ]; then
                processed["$project"]=true

                # Find projects that depend on this one and update their version bumps
                local dependents=$(get_dependent_projects "$project")
                while IFS= read -r dependent; do
                    if [ -n "$dependent" ]; then
                        local current_bump="${version_bumps[$dependent]}"
                        local required_bump="${version_bumps[$project]}"

                        # Determine the more significant bump type
                        local new_bump=""
                        if [ -z "$current_bump" ]; then
                            new_bump="$required_bump"
                        else
                            case "$current_bump:$required_bump" in
                                "fix:feature"|"fix:breaking"|"feature:breaking")
                                    new_bump="$required_bump"
                                    ;;
                                *)
                                    new_bump="$current_bump"
                                    ;;
                            esac
                        fi

                        if [ "$new_bump" != "$current_bump" ]; then
                            version_bumps["$dependent"]="$new_bump"
                            updates_made=true
                            echo "  → $dependent needs $new_bump update due to $project" >&2
                        fi
                    fi
                done <<< "$(echo "$dependents" | tr ';' '\n')"
            fi
        done
    done

    if [ $iteration -eq $max_iterations ]; then
        echo "Warning: Maximum iterations reached while determining version bumps." >&2
    fi

    # Return the associative array by printing it
    for project in "${!version_bumps[@]}"; do
        echo "$project:${version_bumps[$project]}"
    done
}

# Main function to process version updates
update_versions_recursive() {
    echo "=== Determining all version bumps ==="

    # Get all version bumps first
    local bump_list=$(determine_version_bumps)

    echo ""
    echo "=== Applying version updates ==="

    # Apply all version bumps
    while IFS= read -r line; do
        if [ -n "$line" ]; then
            IFS=':' read -ra PARTS <<< "$line"
            local package_id="${PARTS[0]}"
            local change_type="${PARTS[1]}"

            local current_version=$(get_current_version "$package_id")
            local new_version=$(increment_version "$current_version" "$change_type")

            update_version_in_props "$package_id" "$new_version"
        fi
    done <<< "$bump_list"
}

# Parse command line arguments
if [ $# -ne 1 ]; then
    echo "Usage: $0 \"package1:change_type,package2:change_type,...\""
    echo ""
    echo "Change types: breaking, feature, fix"
    echo "Example: $0 \"War3Net.Common:feature,War3Net.IO.Mpq:fix\""
    echo ""
    echo "Note: Use actual NuGet package IDs (e.g., War3Net.CSharpLua, not CSharp.lua)"
    exit 1
fi

DIRECT_UPDATES="$1"

echo "=== War3Net Version Update Script ==="
echo "Processing updates: $DIRECT_UPDATES"

# Process updates recursively
update_versions_recursive

echo ""
echo "=== Version Update Complete ==="
echo "All version updates have been applied to Directory.Build.props"