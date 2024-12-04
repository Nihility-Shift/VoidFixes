using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UI.AstralMap;
using UnityEngine;
using static VoidManager.Utilities.HarmonyHelpers;

namespace VoidFixes.Patches
{
    [HarmonyPatch(typeof(AstralMapController), "OnDisable")]
    internal class AstralMapDisablePatch
    {
        //Astral map co-routine to load the map for the current sector/void is stopped early. Couroutine is stopped on UI close, leading to the map getting stuck on it's void entries.
        //Fix: Remove OnDisable StopAllCouroutines() call. Devs stated it was moved to OnDestroy() for their fix. It should be auto called by unity making a manual call redundant.



        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstruction[] targetSequence = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MonoBehaviour), "StopAllCoroutines"))
            };

            //Remove StopAllCoroutines call and replace with pop (ldarg_0 has a label, blocking me from removing it easily. Best fix is to pop the stack.)
            CodeInstruction[] patchSequence = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Pop)
            };

            return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.REPLACE, CheckMode.ALWAYS);
        }
    }
}
