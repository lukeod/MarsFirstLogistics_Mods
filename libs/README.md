# Game Libraries

This folder contains game DLL references required to build the mods. These files are **not included in the repository** to avoid copyright issues.

## Setup Instructions

You need to copy the required DLL files from your Mars First Logistics installation to this folder.

### Default Steam Installation Path
```
C:\Program Files (x86)\Steam\steamapps\common\Mars First Logistics\
```

### Required Files

#### MelonLoader Folder
Copy these files from `<game>/MelonLoader/net6/` to `libs/MelonLoader/`:
- `MelonLoader.dll`
- `Il2CppInterop.Runtime.dll`
- `Il2CppInterop.Common.dll`

#### Il2CppAssemblies Folder
Copy these files from `<game>/MelonLoader/Il2CppAssemblies/` to `libs/Il2CppAssemblies/`:
- `Assembly-CSharp.dll`
- `Il2Cppmscorlib.dll`
- `UnityEngine.CoreModule.dll`
- `UnityEngine.PhysicsModule.dll`
- `UnityEngine.InputLegacyModule.dll`
- `Unity.TextMeshPro.dll`
- `UnityEngine.UI.dll`
- `UnityEngine.UIModule.dll`

## Adding New Mods

When creating a new mod that requires additional game assemblies:
1. Copy the required DLL to the appropriate folder (MelonLoader or Il2CppAssemblies)
2. Reference it in your `.csproj` using the relative path `../libs/<folder>/<assembly>.dll`
3. Update this README to document the new required file

## Notes

- These DLL files are excluded from Git via `.gitignore`
- Each developer must copy these files locally to their `libs` folder
- Make sure your game is up to date, as assembly changes may break mods
