[![](https://img.shields.io/badge/-Void_Crew_Modding_Team-111111?style=just-the-label&logo=github&labelColor=24292f)](https://github.com/Void-Crew-Modding-Team)
![](https://img.shields.io/badge/Game%20Version-0.26.1-111111?style=flat&labelColor=24292f&color=111111)
[![](https://img.shields.io/discord/1180651062550593536.svg?&logo=discord&logoColor=ffffff&style=flat&label=Discord&labelColor=24292f&color=111111)](https://discord.gg/g2u5wpbMGu "Void Crew Modding Discord")

# VoidFixes

Version 1.0.0  
For Game Version 0.26.1  
Developed by Dragon, 18107  
Requires VoidManager 1.1.2


---------------------

### 💡 Function - **Provides bug fixes, Log Error eliminations, and Optimizations**

- Log Error Eliminations
 - CameraAttachment - Occurs when a player character is loaded.
 - WaitDurationNull - Occurs if a reclaimer timer starts while jumping out of the current sector.
 - FinalizePerkTree - Occurs when a player character is loaded.
 - CharacterJoinCasts - occur when a player character is loaded.
- Fixes
 - AtmosphereFix - Host-Side, Fixes O2 and pressure not filling and dropping at the same rate, leading to pressure being lower than oxygen.
 - SetCarriableActive - Attempts to fix picking up invisible items which then locks pickup ability.
 - FrigateEngineeringDoorFix - Fixes frigate engineering door being closed when joining a game.
 - DeltaTime - Fixes FPS effecting Drone EVA Player targetting and KPD barrel spin.
- Optimizations
 - DestroyJumpingShipsOnLeave - Host-side, effects networking. Destroys ships jumping in on sector leave (they don't normally get destroyed, just hidden). leads to various enemy ships, care packages, and Reclaimers being left in a hidden state.
 - DestroySpawnersOnLeave - Host-side, effects networking. Destroys spawners on sector leave (they don't normally get destroyed, just hidden).
 - DestroyImpactFXOnLeave - Destroys old impactFX on sector leave (they don't normally get destroyed until there's 1024 of a given effect or the session ends.).

### 🎮 Client Usage

- Customize fixes enabled via GUI at F5 > Mod Settings > VoidFixes

### 👥 Multiplayer Functionality

- ✅ Client
- ✅ Host
  - Various fixes effective for clients and/or hosts.

---------------------

## 🔧 Install Instructions - **Install following the normal BepInEx procedure.**

Ensure that you have [BepInEx 5](https://thunderstore.io/c/void-crew/p/BepInEx/BepInExPack/) (stable version 5 **MONO**) and [VoidManager](https://thunderstore.io/c/void-crew/p/VoidCrewModdingTeam/VoidManager/) installed.

#### ✔️ Mod installation - **Unzip the contents into the BepInEx plugin directory**

Drag and drop `VoidFixes.dll` into `Void Crew\BepInEx\plugins`
