# RE:RUN Archipelago Mod

Integration of the game **RE:RUN** into the [Archipelago Multiworld Randomizer](https://archipelago.gg/).

## Features
- **Powerup Shuffling**: Rewind, Double Jump, and the Sword are randomized.
- **Enemy Checks**: 45 specific enemies across 11 levels serve as location checks.
- **Level Progression**: Levels are unlocked via Level Keys found in the multiworld.
- **Goal**: Complete all levels (0-10) to finish the game.

## Installation

### 1. BepInEx Mod
1. Install [BepInEx 5](https://github.com/BepInEx/BepInEx) into your RE:RUN game folder.
2. Copy `RerunArchipelago.dll` and its dependencies (`Archipelago.MultiClient.Net.dll`, `Newtonsoft.Json.dll`) to `BepInEx/plugins/`.

### 2. Archipelago APWorld
1. Copy `rerun.apworld` to your Archipelago installation's `custom_worlds` folder (usually `~/Library/Application Support/Archipelago/worlds/` on Mac or `%LocalAppData%/Archipelago/custom_worlds` on Windows).

## Development
To build the mod and package the `apworld` from source, use the provided scripts in the `build/` folder.

### Building
1.  Open the build script for your OS (`build/build_mac.sh`, `build/build_linux.sh`, or `build/build_windows.bat`).
2.  Update the `MANAGED` and `BEPINEX` paths in the script to match your local installation.
3.  Run the script from the root directory of the repository.

### Files
- `apworld/`: Source code for the Archipelago world.
- `build/`: Cross-platform build scripts.
- `RerunArchipelago.cs`: Source code for the BepInEx mod.
- `RerunArchipelago.dll`: The compiled mod.
- `rerun.apworld`: The packaged Archipelago world.
