using CG.Game.Player;
using Gameplay.Carryables;
using HarmonyLib;
using RSG;
using System;
using System.Reflection;
using VoidManager.Chat.Router;
using VoidManager.Utilities;

namespace VoidFixes
{
    class Commands : ChatCommand
    {
        List<Argument> arguments;

        public Commands()
        {
            arguments = new List<Argument>()
            {
                new Argument(new string[] { "drop", "pickup"})
            };
        }

        public override string[] CommandAliases()
        {
            return new string[] { "fix" };
        }

        public override string Description()
        {
            return "Provides input controls for fixing bugs. Subcommands: " + arguments[0].names;
        }

        static string subcommands = "pickup, drop";

        //pickup fix
        static FieldInfo LockInteractionFI = AccessTools.Field(typeof(CarryableInteract), "lockInteraction");
        static FieldInfo CarryableInsertPromiseFI = AccessTools.Field(typeof(CarrierCarryableHandler), "carryableInsertPromise");
        //static PropertyInfo PromiseCurStatePI = AccessTools.Property(typeof(Promise), "CurState");
        //static FieldInfo insertingCarryableFI = AccessTools.Field(typeof(CarrierCarryableHandler), "insertingCarryable");
        static MethodInfo CarryableOwnerChangeMI = AccessTools.Method(typeof(CarrierCarryableHandler), "CarryableOwnerChange");

        public override void Execute(string arguments)
        {
            string[] args = arguments.Split(' ');
            switch (args[0].ToLower())
            {
                case "drop":
                case "release":
                case "throw":
                case "eject":
                case "pickup":

                    //Client cannot pick up item:
                    // Unsure exactly what starts the bug, somewhere in CarryableInteract.StartCarryableInteraction
                    // Looking at the issue, it appears to be the promise never getting resolved. The local client sends a request to change the owner of the object, but never gets a response.
                    // Further looking at issue: Game object is inactive yuo fucks
                    //
                    // Reproduction:
                    // have lots of items
                    // play mission with lots of drops (container hunt?) and don't pick up drops
                    // Void jump
                    // have player join, they should have bug.
                    //
                    //- CarryableInteract.LockInteraction == true
                    //- After unlocking lockInteraction and trying to grab a carriable, got exception 'System.Exception: another request pending'
                    //- Occurs in CarrierCarryableHandler.TryInsertCarryable when Promise.CurState == PromiseState.Pending
                    //- After setting lockInteraction and promise curstate to accepted values, pickup was unlocked for a short period.

                    //this is a working solution, not necessarily the best solution.
                    //LockInteractionFI.SetValue(LocalPlayer.Instance.Locomotion.GetAbility<CarryableInteract>(), false);
                    //PromiseCurStatePI.SetValue((Promise)CarryableInsertPromiseFI.GetValue(LocalPlayer.Instance.CarryableHandler), PromiseState.Resolved);

                    Messaging.Echo("Attempting to fix pickup/drop capabilities");
                    if (LocalPlayer.Instance.Payload == null)
                    {
                        CarryableOwnerChangeMI.Invoke(LocalPlayer.Instance.CarryableHandler, new object[] { null });
                    }
                    else
                    {
                        if (!LocalPlayer.Instance.Payload.gameObject.activeSelf)
                        {
                            LocalPlayer.Instance.Payload.gameObject.SetActive(true);
                        }
                        LockInteractionFI.SetValue(LocalPlayer.Instance.Locomotion.GetAbility<CarryableInteract>(), false);
                        ((Promise)CarryableInsertPromiseFI.GetValue(LocalPlayer.Instance.CarryableHandler)).Reject(new Exception("forced reject"));
                    }
                    break;
                case "debug":
                    {
                        BepinPlugin.Bindings.DebugLogging.Value = !BepinPlugin.Bindings.DebugLogging.Value;
                        Messaging.Echo("Debug now " + BepinPlugin.Bindings.DebugLogging.Value.ToString());
                    }
                    break;
                default:
                    Messaging.Echo($"Subcommand '{args[0]}' not found. Subcommands: {subcommands}");
                    break;
            }
        }

        public override string[] UsageExamples()
        {
            return new string[] { "/fix [subcommand]"};
        }

        public override List<Argument> Arguments()
        {
            return arguments;
        }
    }
}
