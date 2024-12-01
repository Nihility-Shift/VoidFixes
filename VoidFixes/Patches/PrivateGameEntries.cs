using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using static VoidManager.Utilities.HarmonyHelpers;

namespace VoidFixes.Patches
{
    [HarmonyPatch(typeof(MatchmakingController), "GetRooms")]
    internal class PrivateGameEntries
    {
        static void Prefix(ref bool showFullRooms)
        {
            if (BepinPlugin.Bindings.FullRoomEntriesPatch.Value)
            {
                showFullRooms = false;
            }
        }

        //If enabled, sets limit of private rooms to create as 0;
        static int PatchMethod(int max)
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

        static IEnumerable<CodeInstruction> Transpiler (IEnumerable<CodeInstruction> instructions)
        {
            CodeInstruction[] targetSequence = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Mathf), "Max", new Type[] { typeof(int), typeof(int) }))
            };

            CodeInstruction[] patchSequence = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PrivateGameEntries), "PatchMethod"))
            };

            return PatchBySequence(instructions, targetSequence, patchSequence, PatchMode.AFTER, CheckMode.ALWAYS);
        }
    }
}
