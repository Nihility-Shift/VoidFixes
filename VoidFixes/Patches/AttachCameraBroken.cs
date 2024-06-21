using HarmonyLib;
using Opsive.UltimateCharacterController.FirstPersonController.Character;

namespace VoidFixes.Patches
{
    [HarmonyPatch(typeof(FirstPersonObjects), "OnAttachCamera")]
    internal class AttachCameraBroken
    {
        //Tries to get character Transform's parent, but characterTransform is inactive.
        //
        //NullReferenceException
        //at(wrapper managed-to-native) UnityEngine.Transform.SetParent(UnityEngine.Transform, UnityEngine.Transform, bool)
        //at UnityEngine.Transform.SetParent(UnityEngine.Transform p)[0x00001] in <00d5b91f2c49467b95ced8aa41db73be>:0 
        //at UnityEngine.Transform.set_parentInternal(UnityEngine.Transform value) [0x00001] in <00d5b91f2c49467b95ced8aa41db73be>:0 
        //at UnityEngine.Transform.set_parent(UnityEngine.Transform value) [0x0001a] in <00d5b91f2c49467b95ced8aa41db73be>:0 
        //at Opsive.UltimateCharacterController.FirstPersonController.Character.FirstPersonObjects.<OnAttachCamera>b__56_0 ()[0x00030] in <e9954fab638f4a1e92d2807e0c6e8698>:0 
        //at Opsive.Shared.Game.ScheduledEvent.Invoke ()[0x00000] in <5be52286c54c47958ace04af73911405>:0 
        //at Opsive.Shared.Game.SchedulerBase.Invoke (Opsive.Shared.Game.ScheduledEventBase scheduledEvent, System.Int32 index)[0x0001d] in <5be52286c54c47958ace04af73911405>:0 
        //at Opsive.Shared.Game.SchedulerBase.Update ()[0x0002c] in <5be52286c54c47958ace04af73911405>:0 
        //

        /*
        static MethodInfo IsActiveMI = AccessTools.Method(typeof(FirstPersonObjects), "IsActive");

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Method Declaration", "Harmony003:Harmony non-ref patch parameters modified", Justification = "N/A - Classes are reference types")]
        static bool Prefix(FirstPersonObjects __instance, CameraController cameraController, Transform ___m_CharacterTransform, Transform ___m_Transform, float ___m_Pitch, float ___m_Yaw)
        {
            if (cameraController == null)
            {
                BepinPlugin.Log.LogInfo("OnAttachCamera CameraController is null!");
            }
            else if (cameraController.Transform == null)
            {
                BepinPlugin.Log.LogInfo("OnAttachCamera CameraController transform is null!");
            }

            if (___m_CharacterTransform == null)
            {
                BepinPlugin.Log.LogInfo("OnAttachCamera m_CharacterTransform is null!");
                return false;
            }
            else if(!___m_CharacterTransform.gameObject.activeSelf)
            {
                BepinPlugin.Log.LogInfo("OnAttachCamera m_CharacterTransform is inactive!");
                return false;
            }
            else if (___m_CharacterTransform.parent == null)
            {
                BepinPlugin.Log.LogInfo("OnAttachCamera CharacterTransform.Parent is null");

                if(___m_Transform == null)
                {
                    BepinPlugin.Log.LogInfo("OnAttachCamera m_Transform is null");
                    return false;
                }
                //Set Parent as Character transform, rather than the character transform parent.
                SchedulerBase.Schedule(0.001f, delegate ()
                {
                    ___m_Transform.parent = ___m_CharacterTransform;
                    ___m_Transform.localPosition = Vector3.zero;
                    ___m_Transform.localRotation = Quaternion.identity;
                });
                ___m_Pitch = (___m_Yaw = 0f);
                __instance.enabled = (bool)IsActiveMI.Invoke(__instance, new object[] { });
                return false;
            }
            return true;
        }*/

        static bool Prefix()
        {
            if (!BepinPlugin.Bindings.CameraAttachPatch.Value) return true;

            //Doesn't even get used, so might as well always stop it.
            return false;
        }
    }
}
