using CG.Objects;
using Gameplay.Carryables;
using HarmonyLib;

namespace VoidFixes.Patches
{
    //Client cannot pick up item:
    // Unsure exactly what starts the bug, somewhere in CarryableInteract.StartCarryableInteraction
    // Looking at the issue, it appears to be the promise never getting resolved. The local client sends a request to change the owner of the object, but never gets a response.
    // Further looking at issue: Game object is inactive yuo fucks
    //
    // Here's a little more detail: at some point during load, abstractCarriables are set to inactive. exact cause is unknown.
    // Due to being inactive, objects are not visible and can't be seen on the ground.
    //
    // Reproduction:
    // have lots of items
    // play mission with lots of drops (container hunt?) and don't pick up drops
    // Void jump
    // have player join, they should have bug.
    //
    // I beleive the cause is having objects left over in space, warping out, then getting new objects. Objects in space never get destroyed, rather set to inactive, so the host never stops seeing them.
    // On player join, the host tells the joining player that the objects at the given id are inactive, and the joining player thinks the invis objects are objects on the ship.
    //
    //
    // Cause update: found a wierd issue where any client releasing a carryable in the right side of the destroyer noze.
    //
    //
    //
    //- CarryableInteract.LockInteraction == true
    //- After unlocking lockInteraction and trying to grab a carriable, got exception 'System.Exception: another request pending'
    //- Occurs in CarrierCarryableHandler.TryInsertCarryable when Promise.CurState == PromiseState.Pending
    //- After setting lockInteraction and promise curstate to accepted values, pickup was unlocked for a short period.

    [HarmonyPatch(typeof(CarrierCarryableHandler), "TryInsertCarryable")]
    class SetCarriableActive
    {
        static void Prefix(AbstractCarryableObject carryable)
        {
            if (!BepinPlugin.Bindings.SetCarriableActivePatch.Value) return;
            if (!carryable.gameObject.activeSelf)
            {
                carryable.gameObject.SetActive(true);
            }
        }
    }
}
