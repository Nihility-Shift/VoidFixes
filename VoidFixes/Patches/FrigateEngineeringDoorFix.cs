using CG.Ship.Hull;
using HarmonyLib;
using UnityEngine;

namespace VoidFixes.Patches
{
    [HarmonyPatch(typeof(AbstractDoor), "Start")]
    internal class AbstractDoorPatch
    {
        static void Postfix(AbstractDoor __instance)
        {
            if (BepinPlugin.Bindings.FrigateEngineeringDoorFix.Value && __instance.name == "default_RoundDouble_4x4_v1" && __instance.IsOpen)
            {
                __instance.gameObject.GetComponentInChildren<Animator>().Play("open");
            }
        }
    }
}
