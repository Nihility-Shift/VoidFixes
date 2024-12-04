using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UI.AstralMap;
using UnityEngine;
using static VoidManager.Utilities.HarmonyHelpers;

namespace VoidFixes.Patches
{
    [HarmonyPatch(typeof(AstralMapController), "ToggleUI")]
    internal class AstralMapDisablePatch
    {
        //Astral map co-routine to load the map for the current sector/void is stopped early. Couroutine is stopped on UI close, leading to the map getting stuck on it's void entries.
        //Fix: Replace Behaviour disable with behaviour enable (can't delete, as the behaviour is set to false at an earlier point).
        //     Add progressionPanel Toggle (Panel is not toggled otherwise).




        static void PatchMethod(AstralMapController instance, bool enable)
        {
            if (BepinPlugin.Bindings.AstralMapSoftlockPatch.Value)
            {
                instance.enabled = true;
                instance._progressionPanel.Toggle(enable);
            }
            else
            {
                instance.enabled = enable;
            }
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstruction[] targetSequence = new CodeInstruction[]
            {
                    new CodeInstruction(OpCodes.Call, AccessTools.PropertySetter(typeof(Behaviour), "enabled"))
            };

            CodeInstruction[] patchSequence = new CodeInstruction[]
            {
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(AstralMapDisablePatch), "PatchMethod"))
            };

            return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.REPLACE, CheckMode.ALWAYS);
        }
    }
}
