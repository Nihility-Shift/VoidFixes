using HarmonyLib;
using Opsive.UltimateCharacterController.AddOns.Multiplayer.PhotonPun.Character;
using Photon.Pun;

namespace VoidFixes.Patches
{
    [HarmonyPatch(typeof(PunCharacterAnimatorMonitor), "OnPhotonSerializeView")]
    class PCAMPatch
    {
        //Occurs when a player character is first loaded.
        //
        //Description: First object recived is not a short. From testing was an int32
        //
        //Reproduction: have a player join your session, or join a session with at least one player.
        //
        //InvalidCastException: Specified cast is not valid.
        //at Opsive.UltimateCharacterController.AddOns.Multiplayer.PhotonPun.Character.PunCharacterAnimatorMonitor.OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info) [0x0025c] in <e7e26722948349b2ac53fc64f547ecca>:0 
        //at Photon.Pun.PhotonView.DeserializeComponent(UnityEngine.Component component, Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info) [0x0000a] in <a4d4573dd81f44e7b730ea3bc7b3ace2>:0 
        //at Photon.Pun.PhotonView.DeserializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info) [0x00030] in <a4d4573dd81f44e7b730ea3bc7b3ace2>:0

        static bool Prefix(PhotonStream stream)
        {
            if (!BepinPlugin.Bindings.CharacterJoinCastsPatch.Value) return true;
            if (stream.IsWriting) return true;

            //Not the issue
            //if(stream.Count == 0)
            //{
            //    BepinPlugin.Log.LogWarning("Caught bad stream count in PunCharacterAnimatorMonitor");
            //    return false;
            //}

            if (stream.PeekNext().GetType() != typeof(short))
            {
                if (Commands.DEBUG) BepinPlugin.Log.LogWarning("Not starting with a short, stopping early.");
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(CustomPunCharacterTransformMonitor), "OnPhotonSerializeView")]
    class CPCTMPatch
    {
        //Occurs when joining a session?
        //
        //Description: First object recived is not a byte
        //
        //Reproduction?: Join session with more than 1 player.
        //
        //InvalidCastException: Specified cast is not valid.
        //at CustomPunCharacterTransformMonitor.OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info) [0x002a1] in <939d71a410104d17a02c3d2d0912a345>:0 
        //at Photon.Pun.PhotonView.DeserializeComponent(UnityEngine.Component component, Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info) [0x0000a] in <a4d4573dd81f44e7b730ea3bc7b3ace2>:0 
        //at Photon.Pun.PhotonView.DeserializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info) [0x00030] in <a4d4573dd81f44e7b730ea3bc7b3ace2>:0 

        static bool Prefix(PhotonStream stream)
        {
            if (!BepinPlugin.Bindings.CharacterJoinCastsPatch.Value) return true;
            if (stream.IsWriting) return true;

            if (stream.PeekNext().GetType() != typeof(byte))
            {
                if (Commands.DEBUG) BepinPlugin.Log.LogWarning("Not starting with a byte, stopping early.");
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(PunLookSource), "OnPhotonSerializeView")]
    class PLSPatch
    {
        //Occurs when joining a session?
        //
        //Description: First object recived is not a byte
        //
        //Reproduction?: 
        //
        //InvalidCastException: Specified cast is not valid.
        //at Opsive.UltimateCharacterController.AddOns.Multiplayer.PhotonPun.Character.PunLookSource.OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info) [0x00132] in <e7e26722948349b2ac53fc64f547ecca>:0 
        //at Photon.Pun.PhotonView.DeserializeComponent(UnityEngine.Component component, Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info) [0x0000a] in <a4d4573dd81f44e7b730ea3bc7b3ace2>:0 
        //at Photon.Pun.PhotonView.DeserializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info) [0x00030] in <a4d4573dd81f44e7b730ea3bc7b3ace2>:0 

        static bool Prefix(PhotonStream stream)
        {
            if (!BepinPlugin.Bindings.CharacterJoinCastsPatch.Value) return true;
            if (stream.IsWriting) return true;

            if (stream.PeekNext().GetType() != typeof(byte))
            {
                if (Commands.DEBUG) BepinPlugin.Log.LogWarning("Not starting with a byte, stopping early.");
                return false;
            }
            return true;
        }
    }
}
