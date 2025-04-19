[![](https://img.shields.io/badge/-Nihility_Shift-111111?style=just-the-label&logo=github&labelColor=24292f)](https://github.com/Nihility-Shift)
![](https://img.shields.io/badge/Game%20Version-[GameVersion]-111111?style=flat&labelColor=24292f&color=111111)
[![](https://img.shields.io/discord/1180651062550593536.svg?&logo=discord&logoColor=ffffff&style=flat&label=Discord&labelColor=24292f&color=111111)](https://discord.gg/g2u5wpbMGu "Void Crew Modding Discord")

# [UserModName]

Version [ModVersion]  
For Game Version [GameVersion]  
Developed by [Authors]  
Requires: [Dependencies]


---------------------

### 💡 Functions - **Provides bug fixes, Log Error eliminations, and Optimizations**

- Log Error Eliminations
  - CameraAttachment - Occurs when a player character is loaded.
  - WaitDurationNull - Occurs if a reclaimer timer starts while jumping out of the current sector.
  - FinalizePerkTree - Occurs when a player character is loaded.
  - CharacterJoinCasts - occur when a player character is loaded.
- Fixes
  - AtmosphereFix - Host-Side, Fixes O2 and pressure not filling and dropping at the same rate, leading to pressure being lower than oxygen.
  - SetCarriableActive - Attempts to fix picking up invisible items which then locks pickup ability.
  - DeltaTime - Fixes FPS effecting Drone EVA Player targetting.
  - PrivateGameEntries - Client-Side, Disables creation of private room entries in the matchmaking lists.
  - AstralMapSoftlock - Client-Side, Fixes astral map softlocking when accessing during void enter/exit.
- Optimizations
  - DestroyJumpingShipsOnLeave - Host-side, effects networking. Previously Destroyed ships jumping in on sector leave, which was patched in vanilla 1.0.0. A side effect of the change is the entrypoints being left beyhind, which will be removed by this patch.
  - DestroySpawnersOnLeave - Host-side, effects networking. Destroys spawners on sector leave (they don't normally get destroyed, just hidden).
	- Edit: Largly patched in 1.0.0, a spawner is created and never destroyed in the starting sector.
  - DestroyImpactFXOnLeave - Destroys old impactFX on sector leave (they don't normally get destroyed until there's 1024 of a given effect or the session ends).
	- Edit: Post Void Crew 1.0.0, this issue has largly been fixed. Only enable this setting if you deem it necessary.
  - FullRoomEntries - Client-Side, Disables showing of full rooms in matchmaking lists.

### 🎮 Client Usage

- Customize fixes enabled via GUI at F5 > Mod Settings > VoidFixes
- Command "/fix pickup" - Attempts to fix pickup interaction issues.

### 👥 Multiplayer Functionality

- ✅ Client
  - Various fixes while client.
- ✅ Host
  - Various fixes while hosting.

---------------------

## 🔧 Install Instructions - **Install following the normal BepInEx procedure.**

Ensure that you have [BepInEx 5](https://thunderstore.io/c/void-crew/p/BepInEx/BepInExPack/) (stable version 5 **MONO**) and [VoidManager](https://thunderstore.io/c/void-crew/p/NihilityShift/VoidManager/) installed.

#### ✔️ Mod installation - **Unzip the contents into the BepInEx plugin directory**

Drag and drop `[ModName].dll` into `Void Crew\BepInEx\plugins`