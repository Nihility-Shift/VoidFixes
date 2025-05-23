﻿using BepInEx.Configuration;
using VoidManager.CustomGUI;
using static UnityEngine.GUILayout;
using static VoidFixes.BepinPlugin.Bindings;
using static VoidManager.Utilities.GUITools;

namespace VoidFixes
{
    internal class GUI : ModSettingsMenu
    {
        public override string Name()
        {
            return MyPluginInfo.USERS_PLUGIN_NAME;
        }

        bool DebugLoggingGUIValue;

        bool DestroyJumpingShipsOnLeavePatchGUIValue;
        bool DestroySpawnersOnLeavePatchGUIValue;
        bool DestroImpactFXOnLeavePatchGUIValue;
        bool FinalizePerkTreeNullPatchGUIValue;
        bool FrigateEngineeringDoorFixGUIValue;
        bool SetCarriableActivePatchGUIValue;
        bool CharacterJoinCastsPatchGUIValue;
        bool PrivateGameEntriesPatchGUIValue;
        bool FullRoomEntriesPatchGUIValue;
        bool AstralMapSoftlockPatchGUIValue;
        bool WaitDurationNullPatchGUIValue;
        bool CameraAttachPatchGUIValue;
        bool DeltaTimePatchGUIValue;
        bool AtmosphereFixGUIValue;

        public override void Draw()
        {
            Label("Configuration for various fixes. Some fixes require a restart to take effect.");
            Label(string.Empty);
            DrawValueConfig(DebugLogging, ref DebugLoggingGUIValue);

            DrawValueConfig(DestroyJumpingShipsOnLeavePatch, ref DestroyJumpingShipsOnLeavePatchGUIValue);
            DrawValueConfig(DestroySpawnersOnLeavePatch, ref DestroySpawnersOnLeavePatchGUIValue);
            DrawValueConfig(DestroImpactFXOnLeavePatch, ref DestroImpactFXOnLeavePatchGUIValue);
            DrawValueConfig(FinalizePerkTreeNullPatch, ref FinalizePerkTreeNullPatchGUIValue);
            DrawValueConfig(SetCarriableActivePatch, ref SetCarriableActivePatchGUIValue);
            DrawValueConfig(CharacterJoinCastsPatch, ref CharacterJoinCastsPatchGUIValue);
            DrawValueConfig(PrivateGameEntriesPatch, ref PrivateGameEntriesPatchGUIValue);
            DrawValueConfig(FullRoomEntriesPatch, ref FullRoomEntriesPatchGUIValue);
            DrawValueConfig(WaitDurationNullPatch, ref WaitDurationNullPatchGUIValue);
            DrawValueConfig(CameraAttachPatch, ref CameraAttachPatchGUIValue);
            DrawValueConfig(DeltaTimePatch, ref DeltaTimePatchGUIValue);
            DrawValueConfig(AtmosphereFix, ref AtmosphereFixGUIValue);
        }

        void DrawValueConfig(ConfigEntry<bool> entry, ref bool GUIvalue)
        {
            if (DrawCheckbox(entry.Definition.Key, ref GUIvalue))
            {
                entry.Value = GUIvalue;
            }
            Label(entry.Description.Description);
            Label(string.Empty);
        }

        public override void OnOpen()
        {
            DebugLoggingGUIValue = DebugLogging.Value;
            DestroyJumpingShipsOnLeavePatchGUIValue = DestroyJumpingShipsOnLeavePatch.Value;
            DestroySpawnersOnLeavePatchGUIValue = DestroySpawnersOnLeavePatch.Value;
            DestroImpactFXOnLeavePatchGUIValue = DestroImpactFXOnLeavePatch.Value;
            FinalizePerkTreeNullPatchGUIValue = FinalizePerkTreeNullPatch.Value; 
            SetCarriableActivePatchGUIValue = SetCarriableActivePatch.Value;
            CharacterJoinCastsPatchGUIValue = CharacterJoinCastsPatch.Value;
            PrivateGameEntriesPatchGUIValue = PrivateGameEntriesPatch.Value;
            FullRoomEntriesPatchGUIValue = FullRoomEntriesPatch.Value;
            WaitDurationNullPatchGUIValue = WaitDurationNullPatch.Value;
            CameraAttachPatchGUIValue = CameraAttachPatch.Value;
            DeltaTimePatchGUIValue = DeltaTimePatch.Value;
            AtmosphereFixGUIValue = AtmosphereFix.Value;
        }
    }
}
