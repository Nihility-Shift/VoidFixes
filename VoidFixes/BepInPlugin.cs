using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using static VoidFixes.BepinPlugin.Bindings;

namespace VoidFixes
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Void Crew.exe")]
    [BepInDependency("VoidManager")]
    public class BepinPlugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private void Awake()
        {
            Log = Logger;
            DestroyJumpingShipsOnLeavePatch = Config.Bind("Patches", "DestroyJumpingShipsOnLeave", true, "Host-side, effects networking, destroys ships jumping in on sector leave (they don't normally get destroyed, just hidden)");
            DestroySpawnersOnLeavePatch = Config.Bind("Patches", "DestroySpawnersOnLeave", true, "Host-side, effects networking. destroys spawners on sector leave (they don't normally get destroyed, just hidden)");
            DestroImpactFXOnLeavePatch = Config.Bind("Patches", "DestroyImpactFXOnLeave", true, "Client-side, destroys old impactFX on sector leave (they don't normally get destroyed until there's 1024 of a given type)");
            FinalizePerkTreeNullPatch = Config.Bind("Patches", "FinalizePerkTreeNull", true, "Client-Side, Fixes exception which occurs when a player character is loaded");
            FrigateEngineeringDoorFix = Config.Bind("Patches", "FrigateEngineeringDoorFix", true, "Client-Side, Fixes frigate engineering door being closed when joining a game");
            SetCarriableActivePatch = Config.Bind("Patches", "SetCarriableActive", true, "Client-Side, Attempts to fix picking up invisible items which then locks pickup ability. Try '/fix pickup' command if a similar issue is found.");
            CharacterJoinCastsPatch = Config.Bind("Patches", "CharacterJoinCasts", true, "Client-Side, Fixes exceptions which occur when a player character is loaded");
            WaitDurationNullPatch = Config.Bind("Patches", "WaitDurationNull", true, "Client-Side, Fixes exception which occurs if a reclaimer jumps into the current sector.");
            CameraAttachPatch = Config.Bind("Patches", "CameraAttach", true, "Client-Side, Fixes exception which occurs when a player character is loaded");
            AtmosphereFix = Config.Bind("Patches", "AtmosphereFix", true, "Host-Side, Fixes O2 and pressure not filling and dropping at the same rate, leading to pressure being lower than oxygen.");
            DeltaTimePatch = Config.Bind("Patches", "DeltaTime", true, "Multiple patches on both client and host. Currently affects EVATargetting and KPD spinning barrels");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        internal class Bindings
        {
            internal static ConfigEntry<bool> DestroyJumpingShipsOnLeavePatch;
            internal static ConfigEntry<bool> DestroySpawnersOnLeavePatch;
            internal static ConfigEntry<bool> DestroImpactFXOnLeavePatch;
            internal static ConfigEntry<bool> FinalizePerkTreeNullPatch;
            internal static ConfigEntry<bool> FrigateEngineeringDoorFix;
            internal static ConfigEntry<bool> SetCarriableActivePatch;
            internal static ConfigEntry<bool> CharacterJoinCastsPatch;
            internal static ConfigEntry<bool> WaitDurationNullPatch;
            internal static ConfigEntry<bool> CameraAttachPatch;
            internal static ConfigEntry<bool> DeltaTimePatch;
            internal static ConfigEntry<bool> AtmosphereFix;
        }
    }
}