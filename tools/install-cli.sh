#!/bin/bash
set -e

# Builds the w3n CLI locally and links it onto PATH via ~/bin.
# Usage: ./tools/install-cli.sh

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CLI_PROJECT="$REPO_ROOT/src/War3Net.Tools.Cli/War3Net.Tools.Cli.csproj"
OUT_DIR="$REPO_ROOT/artifacts/w3n-net10.0"
BIN_DIR="$HOME/bin"

if [ ! -f "$CLI_PROJECT" ]; then
  echo "ERROR: CLI project not found: $CLI_PROJECT"
  exit 1
fi

echo "==> Publishing w3n CLI to $OUT_DIR"
dotnet publish "$CLI_PROJECT" -p:CheckEolTargetFramework=false -c Release -f net10.0 -o "$OUT_DIR"

mkdir -p "$BIN_DIR"
ln -sf "$OUT_DIR/w3n" "$BIN_DIR/w3n"
echo "==> Linked $BIN_DIR/w3n -> $OUT_DIR/w3n"

if [[ ":$PATH:" != *":$BIN_DIR:"* ]]; then
  echo "WARNING: $BIN_DIR is not currently on PATH for this shell."
  echo "         Add 'export PATH=\"$BIN_DIR:\$PATH\"' to your shell profile."
fi

echo "==> Done. Run 'w3n --version' to verify."
