using Gameplay.Ship;
using Gameplay.SpacePlatforms;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using static VoidManager.Utilities.HarmonyHelpers;

namespace VoidFixes.Patches
{
    internal class PhysicsOrder
    {
        static bool IsCalledByPatch = false;

        public static void StartDebug()
        {
            MSPDebug = true;
            SEDebug = true;
        }

        static bool MSPDebug = false;
        static bool SEDebug = false;

        [HarmonyPatch(typeof(MovingSpacePlatform), "FixedUpdate")]
        class MSPPatch
        {
            static bool Prefix()
            {
                if (MSPDebug)
                {
                    BepinPlugin.Log.LogDebug("Running MSP Fixed Update");
                    MSPDebug = false;
                }
                if (!BepinPlugin.Bindings.PhysicsOrderPatch.Value) return true;

                return IsCalledByPatch;
            }

            static MethodInfo PreMoveMI = AccessTools.Method(typeof(MovingSpacePlatform), "PreMove");


            //Pre move should get called by normal simuation, but doesn't so I have to call it manually. It's got stuff that should be run after the physics simulation,
            //so I'm just calling it after for the patch.
            static void PreMovePatchMethod(MovingSpacePlatform MSP, bool FirstCall)
            {
                //If patch not applied and is vanilla time for first call, run. If patch applied and is modded timing for calling, run.
                if (BepinPlugin.Bindings.PhysicsOrderPatch.Value && !FirstCall || !BepinPlugin.Bindings.PhysicsOrderPatch.Value || FirstCall)
                {
                    PreMoveMI.Invoke(MSP, new object[] { });
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Method Declaration", "Harmony003:Harmony non-ref patch parameters modified", Justification = "N/A")]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                CodeInstruction[] TargetSequence = new CodeInstruction[] 
                { 
                    new CodeInstruction(OpCodes.Call, PreMoveMI) 
                };

                CodeInstruction[] PatchSequence = new CodeInstruction[]
                {
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MSPPatch), "PreMovePatchMethod"))
                };

                instructions = PatchBySequence(instructions, TargetSequence, PatchSequence, PatchMode.REPLACE);



                TargetSequence = new CodeInstruction[]
                {
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MovingSpacePlatform), "SimulatePhysicsScene")),
                };

                PatchSequence = new CodeInstruction[]
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MSPPatch), "PreMovePatchMethod"))
                };

                return PatchBySequence(instructions, TargetSequence, PatchSequence, PatchMode.AFTER); ;
            }

            static void Postfix()
            {
                IsCalledByPatch = false;
            }
        }

        [HarmonyPatch(typeof(ShipEngine), "FixedUpdate")]
        class EnginePatch
        {
            static void Postfix(ShipEngine __instance)
            {
                if (SEDebug)
                {
                    BepinPlugin.Log.LogDebug("Running SE Fixed Update");
                    SEDebug = false;
                }

                if (!BepinPlugin.Bindings.PhysicsOrderPatch.Value) return;

                if (__instance.ShipMovementController != null)
                {
                    IsCalledByPatch = true;
                    __instance.ShipMovementController.FixedUpdate();
                }
            }
        }
    }
}
