using CG.Game.Missions;
using HarmonyLib;
using UnityEngine;

namespace VoidFixes.Patches
{
    [HarmonyPatch(typeof(WaitDuration), "Run")]
    internal class WaitDurationNullReference
    {
        //Reclaimer countdown fails due to null reference exc. after entering warp
        //
        //The active sector is null
        //
        //
        //
        //Mission 0 'DANGER!' set to Accepted from Generic_Reclaimer_CountdownStartNew
        //Starting mission with id 0
        //Mission 0 'DANGER!' started
        //NullReferenceException: Object reference not set to an instance of an object
        //at CG.Game.Missions.WaitDuration.Run() [0x0002d] in <939d71a410104d17a02c3d2d0912a345>:0 
        //at CG.Game.Missions.MissionBehaviour.Run(CG.Game.Missions.Mission mission, CG.Game.Missions.MissionBehaviourState behaviourState) [0x0000e] in <939d71a410104d17a02c3d2d0912a345>:0 
        //at CG.Game.Missions.Mission.StartMission() [0x00060] in <939d71a410104d17a02c3d2d0912a345>:0 
        //at CG.Game.Missions.MissionManager.NotifyMissionStarted(System.Int32 missionId) [0x0005a] in <939d71a410104d17a02c3d2d0912a345>:0 
        

        static bool Prefix()
        {
            if (!BepinPlugin.Bindings.WaitDurationNullPatch.Value || GameSessionSectorManager.Instance.ActiveSector != null) return true;

            if (Commands.DEBUG) BepinPlugin.Log.LogInfo("Intercepting WaitDuration.Run");
            //This is effectively what would normally happen, but the exception is avoided. I attempted setting the timer to the default duration, but it would keep the countdown running until the next jump.
            return false;
        }
    }
}
