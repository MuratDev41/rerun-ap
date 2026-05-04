#!/bin/bash

# RE:RUN paths
MANAGED_DIR="/Users/muratkaantekeli/Downloads/RERUN_mac/RERUN.app/Contents/Resources/Data/Managed"
BEPINEX_CORE_DIR="/Users/muratkaantekeli/Downloads/RERUN_mac/BepInEx/core"

# Output
OUT="RerunArchipelago.dll"

echo "Building $OUT..."

mcs -t:library \
    -r:$MANAGED_DIR/Assembly-CSharp.dll \
    -r:$MANAGED_DIR/UnityEngine.dll \
    -r:$MANAGED_DIR/UnityEngine.CoreModule.dll \
    -r:$MANAGED_DIR/UnityEngine.IMGUIModule.dll \
    -r:$MANAGED_DIR/UnityEngine.InputLegacyModule.dll \
    -r:$MANAGED_DIR/UnityEngine.TextRenderingModule.dll \
    -r:$MANAGED_DIR/netstandard.dll \
    -r:$BEPINEX_CORE_DIR/BepInEx.dll \
    -r:$BEPINEX_CORE_DIR/0Harmony.dll \
    -r:Archipelago.MultiClient.Net.dll \
    -r:Newtonsoft.Json.dll \
    -out:$OUT \
    RerunArchipelago.cs

if [ $? -eq 0 ]; then
    echo "Build successful!"
    echo "To install the mod, run:"
    echo "cp $OUT Archipelago.MultiClient.Net.dll Newtonsoft.Json.dll /Users/muratkaantekeli/Downloads/RERUN_mac/BepInEx/plugins/"
else
    echo "Build failed."
    exit 1
fi
