#!/bin/bash
# Generate GlobalUsings.cs for every non-submodule project in the solution.
# All unique using directives found in a project are promoted to global usings.

REPO_ROOT="$(cd "$(dirname "$0")/.." && pwd)"

# Find all .csproj files, excluding submodules
find "$REPO_ROOT/src" "$REPO_ROOT/tests" "$REPO_ROOT/perf" -name '*.csproj' 2>/dev/null | sort | while read -r csproj; do
    proj_dir="$(dirname "$csproj")"
    proj_name="$(basename "$proj_dir")"

    # Find .cs files (excluding GlobalUsings.cs itself and obj/bin dirs)
    cs_files=$(find "$proj_dir" -name '*.cs' -not -name 'GlobalUsings.cs' -not -path '*/obj/*' -not -path '*/bin/*' 2>/dev/null)
    file_count=$(echo "$cs_files" | grep -c .)

    if [ "$file_count" -eq 0 ]; then
        echo "SKIP $proj_name (no .cs files)"
        continue
    fi

    # Extract all unique top-level using directives:
    # - Strip BOM (\xEF\xBB\xBF) so lines at the start of files are matched correctly.
    # - Skip usings inside #if blocks (conditional usings should not be promoted).
    # - Exclude static usings, aliases (=), and already-global usings.
    # Sort: System.* first, then rest. Within each group, hierarchical sort (A.B before A.B.C).
    global_usings=$(echo "$cs_files" | xargs awk '
        FNR == 1 { depth = 0 }
        /^\xEF\xBB\xBF/ { sub(/\xEF\xBB\xBF/, "") }
        /^[[:space:]]*#[[:space:]]*(if|ifdef|ifndef)([[:space:]]|$)/ { depth++ }
        /^[[:space:]]*#[[:space:]]*endif([[:space:]]|$)/ { if (depth > 0) depth-- }
        depth == 0 && /^[[:space:]]*using [A-Z]/ && !/^[[:space:]]*using static / && !/=/ && !/^[[:space:]]*global using / {
            line = $0; sub(/^[[:space:]]*/, "", line); print line
        }
    ' 2>/dev/null \
        | sort -u \
        | awk '{
            ns = $0; sub(/^using /, "", ns); sub(/;$/, "", ns);
            if (ns ~ /^System(\..*)?$/) { group = 0 } else { group = 1 }
            print group "\t" ns "\t" $0
        }' | sort -t$'\t' -k1,1n -k2,2 | cut -f3)

    if [ -z "$global_usings" ]; then
        echo "SKIP $proj_name (no usings found)"
        continue
    fi

    # Write GlobalUsings.cs
    output_file="$proj_dir/GlobalUsings.cs"
    echo -n "" > "$output_file"
    while IFS= read -r line; do
        echo "global $line" >> "$output_file"
    done <<< "$global_usings"

    count=$(echo "$global_usings" | wc -l)
    echo "WROTE $proj_name/GlobalUsings.cs ($count global usings from $file_count files)"
done
