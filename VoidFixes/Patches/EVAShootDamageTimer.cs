using CG.Game.SpaceObjects.Controllers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using static VoidManager.Utilities.HarmonyHelpers;

namespace VoidFixes.Patches
{
    //Currently not included due to not being finished. This particular patch only removes the damage timer, leading to fighters not damaging EVA players.
    [HarmonyPatch(typeof(EVAShootingController), "Fire")]
    internal class EVAShootDamageTimer
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //float num = this.CalculateTimeBurstStrikesPlayer(evaTarget);
            //base.StartCoroutine(this.DamagePlayer(num, evaTarget));
            CodeInstruction[] TargetSequence = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call),
                new CodeInstruction(OpCodes.Stloc_0),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call),
                new CodeInstruction(OpCodes.Call),
                new CodeInstruction(OpCodes.Pop)
            };

            CodeInstruction[] PatchSequence = new CodeInstruction[]
            {

            };

            return PatchBySequence(instructions, TargetSequence, PatchSequence, PatchMode.REPLACE, CheckMode.NEVER);
        }
    }
}
