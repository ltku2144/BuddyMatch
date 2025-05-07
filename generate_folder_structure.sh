#!/bin/bash

# Root directory
ROOT_DIR="."
OUTPUT_FILE="folder_structure.txt"

# Folders to exclude
EXCLUDE_DIRS=("node_modules" "dist" ".git" ".next" "__pycache__" "venv")

# Create the exclusion pattern for grep
EXCLUDE_PATTERN=$(printf "|%s" "${EXCLUDE_DIRS[@]}")
EXCLUDE_PATTERN=${EXCLUDE_PATTERN:1}  # remove leading pipe

> "$OUTPUT_FILE"  # Clear previous file

print_structure() {
    local DIR="$1"
    local INDENT="$2"

    echo "${INDENT}$(basename "$DIR")" >> "$OUTPUT_FILE"

    local ITEM
    for ITEM in "$DIR"/*; do
        [ -e "$ITEM" ] || continue  # Skip broken symlinks or empty folders
        local BASENAME=$(basename "$ITEM")
        if [[ " ${EXCLUDE_DIRS[*]} " =~ " ${BASENAME} " ]]; then
            continue
        fi
        if [ -d "$ITEM" ]; then
            print_structure "$ITEM" "  $INDENT"
        elif [ -f "$ITEM" ]; then
            echo "  $INDENT$BASENAME" >> "$OUTPUT_FILE"
        fi
    done
}

print_structure "$ROOT_DIR" ""

echo "✅ Clean structure saved to $OUTPUT_FILE (excluding: ${EXCLUDE_DIRS[*]})"
