@echo off
:: Windows Build Script

:: --- CONFIGURATION ---
:: Path to the game's Managed folder (usually RERUN_Data\Managed)
set MANAGED="C:\Path\to\RERUN\RERUN_Data\Managed"
:: Path to the BepInEx core folder
set BEPINEX="C:\Path\to\BepInEx\core"
:: ---------------------

echo Packaging apworld...
:: Using PowerShell to zip since it's built-in to Windows
powershell -Command "if (Test-Path 'rerun.apworld') { Remove-Item 'rerun.apworld' }; Compress-Archive -Path 'apworld/rerun' -DestinationPath 'rerun.apworld' -Force"

echo Compiling RerunArchipelago.dll...
:: csc.exe is usually in C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe 
:: or available in the PATH if using Visual Studio/MSBuild
csc /target:library /out:RerunArchipelago.dll ^
    /r:%BEPINEX%\BepInEx.dll ^
    /r:%BEPINEX%\0Harmony.dll ^
    /r:%MANAGED%\UnityEngine.dll ^
    /r:%MANAGED%\UnityEngine.CoreModule.dll ^
    /r:%MANAGED%\UnityEngine.IMGUIModule.dll ^
    /r:%MANAGED%\UnityEngine.InputLegacyModule.dll ^
    /r:%MANAGED%\UnityEngine.PhysicsModule.dll ^
    /r:%MANAGED%\UnityEngine.TextRenderingModule.dll ^
    /r:%MANAGED%\UnityEngine.UI.dll ^
    /r:%MANAGED%\netstandard.dll ^
    /r:%MANAGED%\Assembly-CSharp.dll ^
    /r:Archipelago.MultiClient.Net.dll ^
    /r:Newtonsoft.Json.dll ^
    RerunArchipelago.cs

if %errorlevel% equ 0 (
    echo Build successful!
) else (
    echo Build failed.
)
pause
