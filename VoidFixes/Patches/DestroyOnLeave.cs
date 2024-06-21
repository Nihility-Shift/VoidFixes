using BufferedEvents.Impacts;
using CG.Game.SpaceObjects.Controllers.ImpulseJumping;
using CG.Space;
using Gameplay.NPC.AI;
using HarmonyLib;
using Photon.Pun;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace VoidFixes.Patches
{
    //Patch goal: Cleanup leftover objects on leave sector.


    [HarmonyPatch(typeof(GameSessionSector), "Unload")]
    class DestroyOnLeave
    {
        static FieldInfo actJumpToSectorActionFI = AccessTools.Field(typeof(BehJumpToSector), "actJumpToSectorAction");
        static FieldInfo sectorEntryObjectFI = AccessTools.Field(typeof(ActJumpToSector), "sectorEntryObject");


        static void Postfix(bool asHost)
        {
            //The SFX and VFX processors attempt to re-use sounds, but often forget to re-use sounds after jumping. These end up staying until the client leaves the session. This method will request the pools dispose.
            if (BepinPlugin.Bindings.DestroImpactFXOnLeavePatch.Value)
            {
                ClearFinishedSharedVFX();
            }

            if (!asHost) return;

            Object[] FoundObjects;

            //Spawners are not deleted after leaving a sector, and stay active. this leads to unnecessary network trafic for Spawners from many jumps in the past.
            if (BepinPlugin.Bindings.DestroySpawnersOnLeavePatch.Value)
            {
                FoundObjects = GameObject.FindObjectsOfType(typeof(Spawner));
                foreach (Object FO in FoundObjects)
                {
                    Spawner ConvertedFO = (Spawner)FO;
                    if (BepinPlugin.Bindings.DebugLogging.Value) BepinPlugin.Log.LogInfo("Attempting to photon destroy a spawner");
                    PhotonNetwork.Destroy(ConvertedFO.gameObject);
                }
            }


            if (BepinPlugin.Bindings.DestroyJumpingShipsOnLeavePatch.Value)
            {
                //Objects warping into the sector such as supply drops, normal ships, and summoned swarms do not cleanup after leaving a sector.
                GameSessionSector CurrentSector = GameSessionSectorManager.Instance.ActiveSector;
                FoundObjects = GameObject.FindObjectsOfType(typeof(AbstractSpaceCraft), true);
                foreach (Object FO in FoundObjects)
                {
                    AbstractSpaceCraft convertedFO = (AbstractSpaceCraft)FO;
                    //Targeted objects are almost always inactive. They are also usually not in the current sector do to jumping. (I accidentally destroyed wreckages lol)
                    if (!convertedFO.isActiveAndEnabled && convertedFO.Sector != CurrentSector)
                    {
                        if (BepinPlugin.Bindings.DebugLogging.Value) BepinPlugin.Log.LogInfo("Attempting to photon destroy " + convertedFO.DisplayName);


                        NpcImpulseJumper jumper = convertedFO.GetComponent<NpcImpulseJumper>();
                        if (jumper != null) //find and destroy entry object from Impulse Jumper
                        {
                            if (BepinPlugin.Bindings.DebugLogging.Value) BepinPlugin.Log.LogInfo("Found ImpulseJumper");
                            PhotonNetwork.Destroy(jumper.state.SectorEntryObject);
                        }
                        else //else find and destroy entry object from NPCAI
                        {
                            NpcAI jumperNPC = convertedFO.GetComponent<NpcAI>();
                            if (jumperNPC != null)
                            {
                                BehJumpToSector JTSBeh = jumperNPC.ActiveBehaviour as BehJumpToSector;
                                if (JTSBeh != null)
                                {
                                    if (BepinPlugin.Bindings.DebugLogging.Value) BepinPlugin.Log.LogInfo("Found NpcAI Beh");
                                    ActJumpToSector JTSAct = (ActJumpToSector)actJumpToSectorActionFI.GetValue(JTSBeh);
                                    PhotonNetwork.Destroy((GameObject)sectorEntryObjectFI.GetValue(JTSAct));
                                }
                            }
                        }

                        //Finally network destroy spacecraft gameobject.
                        PhotonNetwork.Destroy(convertedFO.GameObject);
                    }
                }
            }
        }


        static FieldInfo VPFI = AccessTools.Field(typeof(VFXImpactsProcessor), "_visualPools");
        static FieldInfo SPFI = AccessTools.Field(typeof(SFXImpactsProcessor), "_soundPools");

        static void ClearFinishedSharedVFX()
        {
            VFXImpactsProcessor IP = GameObject.FindObjectOfType<VFXImpactsProcessor>();
            if (IP != null)
            {
                Dictionary<PoolableImpactFX, ImpactFXPool> VPs = (Dictionary<PoolableImpactFX, ImpactFXPool>)VPFI.GetValue(IP);

                foreach (ImpactFXPool thing in VPs.Values)
                {
                    thing.Dispose();
                }
            }

            SFXImpactsProcessor SP = GameObject.FindObjectOfType<SFXImpactsProcessor>();
            if (SP != null)
            {
                Dictionary<PoolableImpactFX, ImpactFXPool> SPs = (Dictionary<PoolableImpactFX, ImpactFXPool>)SPFI.GetValue(SP);

                foreach (ImpactFXPool thing in SPs.Values)
                {
                    thing.Dispose();
                }
            }
        }

        //Seems to occasionally not get destroyed, unsure why. The active sector should have the care package added as soon as it warps in.
        //Might not be destroyed because the player left the sector while the care package was still warping in.
        //Edit: this is an older patch attempt, and should get deleted in later commits.
        /*[HarmonyPatch(typeof(SpawnUtils), "SpawnCarePackage")]
        class SpawnCarePackage
        {
            static void Postfix(OrbitObject __result)
            {
                GameSessionManager.ActiveSession.ActiveSector.AddOrbitObject(__result, false);
            }
        }*/
    }
}
