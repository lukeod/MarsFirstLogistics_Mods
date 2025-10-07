# Mars First Logistics Mods

A monorepo containing multiple mods for Mars First Logistics using MelonLoader.

## Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- Mars First Logistics with [MelonLoader v0.7.1](https://github.com/LavaGang/MelonLoader) installed
- A C# IDE (Visual Studio, VS Code, or JetBrains Rider)

**Note:** These mods were developed against MelonLoader v0.7.1. Other versions may work but are untested.

## Setup

### 1. Clone the Repository

```bash
git clone https://github.com/lukeod/MarsFirstLogistics_Mods.git
cd MarsFirstLogistics_Mods
```

### 2. Copy Game DLLs

The mods require reference DLLs from your game installation. These are **not included** in the repository.

Follow the instructions in [`libs/README.md`](libs/README.md) to copy the required DLL files from your Mars First Logistics installation to the `libs` folder.

**Quick summary:**
- Copy MelonLoader DLLs from `<game>/MelonLoader/net6/` to `libs/MelonLoader/`
- Copy game assemblies from `<game>/MelonLoader/Il2CppAssemblies/` to `libs/Il2CppAssemblies/`

## Building

### Build All Mods

```bash
dotnet build
```

### Build a Specific Mod

```bash
dotnet build SpeedometerMod/SpeedometerMod.csproj
```

## Installation

After building, copy the compiled DLL from the mod's `bin/Debug/net6.0/` or `bin/Release/net6.0/` folder to your game's Mods folder:

```
<game-directory>/Mods/<ModName>.dll
```

## Available Mods

- **SpeedometerMod** - [Add description here]

## Creating a New Mod

1. Create a new folder in the repository root
2. Create a `.csproj` file (copy and modify from an existing mod)
3. Reference game DLLs using `../libs/` relative paths
4. Update this README with your new mod

## License

See [LICENSE](LICENSE) for details.
