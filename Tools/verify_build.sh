#!/usr/bin/env bash
set -e

echo "=== LOVEVERSE Verification Pipeline ==="

# 1. Verify Backend TypeScript Compilation & Tests
echo "[1/2] Verifying Backend TypeScript & Tests..."
cd Backend
npm run build
npm test
cd ..

# 2. Check Assembly Definitions and File Hierarchy
echo "[2/2] Verifying Unity Assembly Definitions..."
ASMDEF_COUNT=$(find Assets/_Project -name "*.asmdef" | wc -l)
echo "Found $ASMDEF_COUNT Assembly Definitions."
if [ "$ASMDEF_COUNT" -lt 8 ]; then
    echo "ERROR: Expected at least 8 asmdefs, found $ASMDEF_COUNT"
    exit 1
fi

echo "=== LOVEVERSE Verification Passed Successfully ==="
