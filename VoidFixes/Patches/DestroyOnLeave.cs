using BufferedEvents.Impacts;
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
            //Update for Vanilla 1.0.0 - VFX processors now handle old sounds much better.
            //The SFX and VFX processors attempt to re-use sounds, but often forget to re-use sounds after jumping. These end up staying until the client leaves the session. This method will request the pools dispose.
            if (BepinPlugin.Bindings.DestroImpactFXOnLeavePatch.Value)
            {
                ClearFinishedSharedVFX();
            }

            if (!asHost) return;

            Object[] FoundObjects;

            //Update for Vanilla 1.0.0 - Most spawners are now taken care of, with the exception of the spawner in the starting sector (I beleive. Not 100% sure where it came from.)
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


            //Modified for Vanilla 1.0.0 - While the ships are now deleted in vanilla, their jump arrival positions are not.
            if (BepinPlugin.Bindings.DestroyJumpingShipsOnLeavePatch.Value)
            {
                //Objects warping into the sector such as supply drops, normal ships, and summoned swarms do not cleanup after leaving a sector.
                GameSessionSector CurrentSector = GameSessionSectorManager.Instance.ActiveSector;
                FoundObjects = GameObject.FindObjectsOfType<PhotonView>();
                foreach (Object FO in FoundObjects)
                {
                    if(FO.name == "VoidJumpArrivalPosition(Clone)")
                    {
                        PhotonView pv = (PhotonView)FO;
                        PhotonNetwork.Destroy(pv);
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
    }
}
