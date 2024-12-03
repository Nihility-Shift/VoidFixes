using CG.GameLoopStateMachine;
using CG.GameLoopStateMachine.GameStates;
using HarmonyLib;
using Photon.Pun;

namespace VoidFixes.Patches
{
    // The start of an investigation into keeping the session intact in the event of a connection loss.
    // Deemed insanely difficult, so the next step is offline mode and saving
    [HarmonyPatch(typeof(GSPhotonDisconnected), "OnEnter")]
    internal class OfflineHostDisconnect
    {
        static bool Prefix(IState previousState)
        {
            if (previousState is GSIngame && PhotonNetwork.IsMasterClient)
            {
                return false;
            }
            return true;
        }
    }
}
