using HarmonyLib;
using UI.Codex;
using UI.Token;

namespace VoidFixes.Patches
{
    [HarmonyPatch(typeof(LogBookManager), "FinalizePerkTree")]
    internal class FinalizePerkTreeNull
    {
        //Occurs when a different player is first loaded.
        //
        //The logbooks for other players is loaded. We never access the logbooks of otherplayers, so preferable this would be stopped early. Sadly, I cannot patch IEnumerables.
        //
        //Reproduction?: join a game with 1 or more players
        //
        //NullReferenceException: Object reference not set to an instance of an object
        //at UI.Codex.LogBookManager.FinalizePerkTree() [0x00000] in <939d71a410104d17a02c3d2d0912a345>:0 
        //at(wrapper delegate-invoke) <Module>.invoke_void()
        //at UI.Token.PerkTreeController.OnTreeLoaded(UI.Token.PerkTreeVE tree) [0x0009c] in <939d71a410104d17a02c3d2d0912a345>:0 
        //at UI.Token.PerkTreeVE.DelayedSetup() [0x0003d] in <939d71a410104d17a02c3d2d0912a345>:0 

        static bool Prefix(LogBookManager __instance, PerkTreeController ___treeController)
        {
            //this.treeController = new PerkTreeController(ResourceAssetContainer<PerkContainer, Perk, PerkDef>.Instance.AssetDescriptions, this.doc.rootVisualElement, this.Events, base.GetComponent<ILocalizationProvider>(), true);

            if (BepinPlugin.Bindings.FinalizePerkTreeNullPatch.Value && ___treeController == null)
            {
                if (Commands.DEBUG) BepinPlugin.Log.LogWarning("LogBookManager TreeController is null, stopping early.");
                //GameObject.Destroy(__instance.gameObject);
                return false;
            }
            return true;
        }
    }


    [HarmonyPatch(typeof(TokenTerminal), "UpdateTrees")]
    class patchclass
    {
        static bool Prefix(PerkTreeController ___treeController)
        {
            if (BepinPlugin.Bindings.FinalizePerkTreeNullPatch.Value && ___treeController == null)
            {
                if (Commands.DEBUG) BepinPlugin.Log.LogWarning("TokenTerminal TreeController is null, stopping early.");
                return false;
            }
            return true;
        }
    }
}
