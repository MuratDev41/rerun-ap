#!/bin/bash
# MacOS Build Script

# --- CONFIGURATION ---
# Path to the game's Managed folder (inside the .app bundle)
MANAGED="/Users/muratkaantekeli/Downloads/RERUN_mac/RERUN.app/Contents/Resources/Data/Managed"
# Path to the BepInEx core folder
BEPINEX="/Users/muratkaantekeli/Downloads/RERUN_mac/BepInEx/core"
# ---------------------

echo "Packaging apworld..."
cd apworld && zip -rq ../rerun.apworld rerun/ && cd ..

echo "Compiling RerunArchipelago.dll..."
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
