using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using static VoidFixes.BepinPlugin.Bindings;

namespace VoidFixes
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.USERS_PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Void Crew.exe")]
    [BepInDependency(VoidManager.MyPluginInfo.PLUGIN_GUID)]
    public class BepinPlugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private void Awake()
        {
            Log = Logger;
            DebugLogging = Config.Bind("General", "DebugLogging", true, "Enables Logging");
            DestroyJumpingShipsOnLeavePatch = Config.Bind("Patches", "DestroyJumpingShipsOnLeave", true, "Host-side, effects networking. Previously Destroyed ships jumping in on sector leave, which was patched in 1.0.0. A side effect of the change is the entrypoints being left beyhind, which will be removed by this patch.");
            DestroySpawnersOnLeavePatch = Config.Bind("Patches", "DestroySpawnersOnLeave", true, "Host-side, effects networking. Destroys spawners on sector leave (they don't normally get destroyed, just hidden).\nEdit: Largly patched in Vanilla 1.0.0, a spawner is created and never destroyed in the starting sector.");
            DestroImpactFXOnLeavePatch = Config.Bind("Patches", "DestroyImpactFXOnLeave", false, "Client-side, Destroys old impactFX on sector leave (they don't normally get destroyed until there's 1024 of a given effect or the session ends)\nEdit: Post Void Crew 1.0.0, this issue has largly been fixed. Only enable this setting if you deem it necessary.");
            FinalizePerkTreeNullPatch = Config.Bind("Patches", "FinalizePerkTreeNull", true, "Client-Side, Fixes exception which occurs when a player character is loaded.");
            SetCarriableActivePatch = Config.Bind("Patches", "SetCarriableActive", true, "Client-Side, Attempts to fix picking up invisible items which then locks pickup ability. Try '/fix pickup' command if a similar issue is found.");
            CharacterJoinCastsPatch = Config.Bind("Patches", "CharacterJoinCasts", true, "Client-Side, Fixes exceptions which occur when a player character is loaded.");
            PrivateGameEntriesPatch = Config.Bind("Patches", "PrivateGameEntries", true, "Client-Side, Disables creation of private room entries in the matchmaking lists.");
            FullRoomEntriesPatch = Config.Bind("Patches", "FullRoomEntries", true, "Client-Side, Disables showing of full rooms in matchmaking lists");
            WaitDurationNullPatch = Config.Bind("Patches", "WaitDurationNull", true, "Client-Side, Fixes exception which occurs if a reclaimer timer starts while jumping out of the current sector.");
            CameraAttachPatch = Config.Bind("Patches", "CameraAttach", true, "Client-Side, Fixes exception which occurs when a player character is loaded.");
            DeltaTimePatch = Config.Bind("Patches", "DeltaTime", true, "Multiple patches on both client and host. Currently affects EVATargetting. Requires Restart.");
            AtmosphereFix = Config.Bind("Patches", "AtmosphereFix", true, "Host-Side, Fixes O2 and pressure not filling and dropping at the same rate, leading to pressure being lower than oxygen.");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        internal class Bindings
        {
            internal static ConfigEntry<bool> DestroyJumpingShipsOnLeavePatch;
            internal static ConfigEntry<bool> DestroySpawnersOnLeavePatch;
            internal static ConfigEntry<bool> DestroImpactFXOnLeavePatch;
            internal static ConfigEntry<bool> FinalizePerkTreeNullPatch;
            internal static ConfigEntry<bool> SetCarriableActivePatch;
            internal static ConfigEntry<bool> CharacterJoinCastsPatch;
            internal static ConfigEntry<bool> PrivateGameEntriesPatch;
            internal static ConfigEntry<bool> FullRoomEntriesPatch;
            internal static ConfigEntry<bool> WaitDurationNullPatch;
            internal static ConfigEntry<bool> CameraAttachPatch;
            internal static ConfigEntry<bool> DeltaTimePatch;
            internal static ConfigEntry<bool> AtmosphereFix;
            internal static ConfigEntry<bool> DebugLogging;
        }
    }
}