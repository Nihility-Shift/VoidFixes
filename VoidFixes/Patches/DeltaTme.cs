using CG.Game.SpaceObjects.Controllers;
using CG.Ship.Modules.Weapons;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using static VoidManager.Utilities.HarmonyHelpers;

namespace VoidFixes.Patches
{
    class DeltaTme
    {
        internal static CodeInstruction[] DeltaSequence = new CodeInstruction[] { new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(Time), "deltaTime")) };
        internal static CodeInstruction[] FixedDeltaSequence = new CodeInstruction[] { new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(Time), "fixedDeltaTime")) };

        /*static IEnumerable<CodeInstruction> DeltaToFixedDelta(IEnumerable<CodeInstruction> instructions)
        {
            return PatchBySequence(instructions, DeltaSequence, FixedDeltaSequence, PatchMode.REPLACE);
        }
        static IEnumerable<CodeInstruction> DeltaToFixedDeltaMultiPatch(IEnumerable<CodeInstruction> instructions, int count)
        {
            for (int i = 0; i < count; i++)
            {
                instructions = PatchBySequence(instructions, DeltaSequence, FixedDeltaSequence, PatchMode.REPLACE);
            }
            return instructions;
        }*/
        static IEnumerable<CodeInstruction> FixedDeltaToDelta(IEnumerable<CodeInstruction> instructions)
        {
            if (!BepinPlugin.Bindings.DeltaTimePatch.Value) return instructions;


            return PatchBySequence(instructions, FixedDeltaSequence, DeltaSequence, PatchMode.REPLACE);
        }
        static IEnumerable<CodeInstruction> FixedDeltaToDeltaMultiPatch(IEnumerable<CodeInstruction> instructions, int count)
        {
            if (!BepinPlugin.Bindings.DeltaTimePatch.Value) return instructions;


            for (int i = 0; i < count; i++)
            {
                instructions = PatchBySequence(instructions, FixedDeltaSequence, DeltaSequence, PatchMode.REPLACE);
            }
            return instructions;
        }


        //
        //Start Delta to FixedDelta patches. Supposedly unity automatically returns the correct delta or fixed delta when delta is called.
        //
        //According to Unity: When Time.DeltaTime is called from inside MonoBehaviour.FixedUpdate, it returns Time.fixedDeltaTime, so I'm not gonna bother calling these anymore.
        //
        //

        /*
        [HarmonyPatch(typeof(CarryableAttractor), "FinalAttractionOfObject")]
        class CarryableAttractorPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return DeltaToFixedDelta(instructions);
            }
        }

        [HarmonyPatch(typeof(ConstantForwardSpaceCraftController), "FixedUpdate")]
        class CFSCCPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return DeltaToFixedDelta(instructions);
            }
        }

        [HarmonyPatch(typeof(ScanningEnemyController), "UpdateScanning")]
        class SECPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return DeltaToFixedDeltaMultiPatch(instructions, 2);
            }
        }

        [HarmonyPatch(typeof(VoidDriveModule), "FixedUpdate")]
        class VoidDriveModulePatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return DeltaToFixedDelta(instructions);
            }
        }

        [HarmonyPatch(typeof(VoidDriveModule), "UpdateOn")]
        class VoidDriveModuleUpdateOnPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return DeltaToFixedDeltaMultiPatch(instructions, 2);
            }
        }

        [HarmonyPatch(typeof(BulletMagazine), "ReloadUpdate")]
        class BulletMagazinePatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return DeltaToFixedDelta(instructions);
            }
        }

        //This was a hell of a trace, but from what I can tell gets called by SimpleFSM.UpdateFsm, which gets called by Update functions.
        [HarmonyPatch(typeof(LookForPlayerStateLogic), "UpdateLookAround")]
        class LookForPlayerStateLogicPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return DeltaToFixedDeltaMultiPatch(instructions, 2);
            }
        }


        //Roll and pitch are applied on fixed update... that should be on normal updates. It means the ship pitch and roll freezes between fixed update frames.
        [HarmonyPatch(typeof(ShipAutoTilt), "ApplyRoll")]
        class ShipAutoTiltRollPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return DeltaToFixedDelta(instructions);
            }
        }

        [HarmonyPatch(typeof(ShipAutoTilt), "ApplyPitch")]
        class ShipAutoTiltPitchPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return DeltaToFixedDelta(instructions);
            }
        }

        [HarmonyPatch(typeof(Atmosphere), "EqualizeRooms")]
        class AtmospherePatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return DeltaToFixedDelta(instructions);
            }
        }

        [HarmonyPatch(typeof(VisionCone), "UpdateVisualDistanceCovered")]
        class VisionConePatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return DeltaToFixedDelta(instructions);
            }
        }

        [HarmonyPatch(typeof(FlyJetpack), "UpdatePosition")]
        class FlyJetpackPositionPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return DeltaToFixedDeltaMultiPatch(instructions, 2);
            }
        }

        [HarmonyPatch(typeof(FlyJetpack), "SimpleAnimateBody")]
        class FlyJetpackSAMPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return DeltaToFixedDeltaMultiPatch(instructions, 2);
            }
        }*/

        //ActLockOn.Tick calls DeltaTime, but I don't know what calls it.

        //GSEndSession, GSIntro, GSStartFlow, and GSSpawn use deltatime and are called by UpdateState, but I don't know what calls UpdateState.

        //
        // Start FixedDelta to Delta patches
        //


        //WeaponControlFrame.FireRecoil contains a fixedUpdate, but runs once per frame. I would patch it, but coroutines cannot be patched by harmony and the other option is to replace it with a new coroutine.

        //These actually run on fixed Updates, as they are not monobehaviour updates.
        /*
        [HarmonyPatch(typeof(FiniteDurationClassAbility), "Update")]
        class FiniteDurationClassAbilityPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return FixedDeltaToDelta(instructions);
            }
        }
        [HarmonyPatch(typeof(ClassAbility), "Update")]
        class ClassAbilityPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return FixedDeltaToDelta(instructions);
            }
        }*/

        [HarmonyPatch(typeof(SpinningBarrelAnimator), "UpdateBarrelRoll")]
        class SpinningBarrelAnimatorPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return FixedDeltaToDelta(instructions);
            }
        }

        [HarmonyPatch(typeof(SwarmController), "FlyDroneTowardsEVAPlayer")]
        class SwarmControllerPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return FixedDeltaToDeltaMultiPatch(instructions, 2);
            }
        }
    }
}
