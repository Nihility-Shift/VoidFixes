using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using static VoidManager.Utilities.HarmonyHelpers;

namespace VoidFixes.Patches
{
    [HarmonyPatch(typeof(MatchmakingController), "GetRooms")]
    internal class PrivateGameEntries
    {
        //If Enabled, disables showing of private rooms.
        static bool PatchMethod1(bool ShowFull)
        {
            if (BepinPlugin.Bindings.FullRoomEntriesPatch.Value)
            {
                return false;
            }
            else
            {
                return ShowFull;
            }
        }

        //If enabled, sets limit of private rooms to create as 0;
        static int PatchMethod2(int max)
        {
            if(BepinPlugin.Bindings.PrivateGameEntriesPatch.Value)
            {
                return 0;
            }
            else
            {
                return max;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Method Declaration", "Harmony003:Harmony non-ref patch parameters modified", Justification = "N/A")]
        static IEnumerable<CodeInstruction> Transpiler (IEnumerable<CodeInstruction> instructions)
        {
            
            CodeInstruction[] targetSequence = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_1)
            };

            CodeInstruction[] patchSequence = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PrivateGameEntries), "PatchMethod1"))
            };

            instructions = PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.AFTER, CheckMode.NEVER);

            targetSequence = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Mathf), "Max", new Type[] { typeof(int), typeof(int) }))
            };

            patchSequence = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PrivateGameEntries), "PatchMethod2"))
            };

            return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.AFTER, CheckMode.ALWAYS);
        }
    }
}
