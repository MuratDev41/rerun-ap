#!/bin/bash
# Linux Build Script

# --- CONFIGURATION ---
# Path to the game's Managed folder (usually RERUN_Data/Managed)
MANAGED="path/to/RERUN_Data/Managed"
# Path to the BepInEx core folder
BEPINEX="path/to/BepInEx/core"
# ---------------------

echo "Packaging apworld..."
cd apworld && zip -rq ../rerun.apworld rerun/ && cd ..

echo "Compiling RerunArchipelago.dll..."
# Uses csc (requires Mono or .NET SDK)
csc -target:library -out:RerunArchipelago.dll \
    -r:"$BEPINEX/BepInEx.dll" \
    -r:"$BEPINEX/0Harmony.dll" \
    -r:"$MANAGED/UnityEngine.dll" \
    -r:"$MANAGED/UnityEngine.CoreModule.dll" \
    -r:"$MANAGED/UnityEngine.IMGUIModule.dll" \
    -r:"$MANAGED/UnityEngine.InputLegacyModule.dll" \
    -r:"$MANAGED/UnityEngine.PhysicsModule.dll" \
    -r:"$MANAGED/UnityEngine.TextRenderingModule.dll" \
    -r:"$MANAGED/UnityEngine.UI.dll" \
    -r:"$MANAGED/netstandard.dll" \
    -r:"$MANAGED/Assembly-CSharp.dll" \
    -r:Archipelago.MultiClient.Net.dll \
    -r:Newtonsoft.Json.dll \
    RerunArchipelago.cs

if [ $? -eq 0 ]; then
    echo "Build successful!"
else
    echo "Build failed."
fi
