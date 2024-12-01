using CG.Objects;
using Gameplay.Carryables;
using HarmonyLib;

namespace VoidFixes.Patches
{
    //Client cannot pick up item:
    // Unsure exactly what starts the bug, somewhere in CarryableInteract.StartCarryableInteraction
    // Looking at the issue, it appears to be the promise never getting resolved. The local client sends a request to change the owner of the object, but never gets a response.
    // Further looking at issue: Game object is inactive *Expletive*
    //
    // Cause update: found a wierd issue where any client releasing a carryable in the right side of the destroyer noze.
    //
    // Here's a little more detail: at some point during load, abstractCarriables are set to inactive. exact cause is unknown.
    // Due to being inactive, objects are not visible and can't be seen on the ground.
    //
    // I've seen several cases where dropping carryables in the nose of the destroyer lead to them being set as inactive. I also saw a video clip where a user carrying a
    // carryable and walking through the ship airlock lead to the carryable toggling active based on which side the user was on. In my own testing I produced the issue
    // for myself as a client, wherein I dropped two carryables into the nose of the destroyer while in warp when they immediately disappeared. I couldn't reproduce
    // this issue in the next jump of the same session.
    // This leads to my current hypothesis: Environment-based object culling. Either somehow a portion of the ship no longer differentiates items dropped in it from being dropped
    // in void, or dropping items while the host doesn't see them causes the issue.
    //
    //
    // Old reproduction steps, unreliable.
    // Reproduction:
    // have lots of items
    // play mission with lots of drops (container hunt?) and don't pick up drops
    // Void jump
    // have player join, they should have bug.
    //
    //
    // Old hypotheses, I don't beleive in it (still possible though):
    // I beleive the cause is having objects left over in space, warping out, then getting new objects. Objects in space never get destroyed, rather set to inactive, so the host never stops seeing them.
    // On player join, the host tells the joining player that the objects at the given id are inactive, and the joining player thinks the invis objects are objects on the ship.
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
        static void Prefix(CarryableObject carryable)
        {
            if (carryable == null)
            {
                BepinPlugin.Log.LogError("SetCarriableActive:Prefix - Carryable is null!");
            }

            if (!BepinPlugin.Bindings.SetCarriableActivePatch.Value) return;
            if (!carryable.gameObject.activeSelf)
            {
                carryable.gameObject.SetActive(true);
            }
        }
    }
}
