# Rust-Harmony-Mods
> Open-source **Rust** server mods built with Harmony patching — compiled DLLs for the HarmonyMods folder, no Oxide/Carbon required.  
> Author: **Kxrse** — Non-Commercial, attribution required. See [LICENSE.txt](LICENSE.txt).
---
## Mods
| Mod | Status | Description |
|---|---|---|
| [SpawnMini](SpawnMini/) | Released | Spawn a minicopter with full fuel via `/mini`. Re-typing `/mini` despawns the previous one and spawns a new one. |
---
## Requirements
- **Rust Dedicated Server** (vanilla, no framework needed)
- **.NET Framework 4.8**
- **Rider** or **Visual Studio** for building

## Installation
1. Build the mod in **Release** configuration.
2. Copy the output `.dll` to your server's `HarmonyMods/` folder (next to `RustDedicated.exe`).
3. Start the server, or hot-reload with `harmony.load ModName` in the server console.

## Development
Each mod is a standalone Visual Studio solution. References point to DLLs in `RustDedicated_Data/Managed/`:
- `0Harmony.dll`
- `Assembly-CSharp.dll`
- `UnityEngine.CoreModule.dll`
- `UnityEngine.dll`
- `Facepunch.UnityEngine.dll`
- `Facepunch.Console.dll`
- `Facepunch.Network.dll`

## License
[Kxrse Rust Harmony Mods Non-Commercial License](LICENSE.txt)  
Free to use, modify, and redistribute **with attribution**.  
Commercial use or resale requires explicit written permission.
