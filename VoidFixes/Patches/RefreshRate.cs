using CG;
using HarmonyLib;
using System;
using UnityEngine;

namespace VoidFixes.Patches
{
    /*[HarmonyPatch(typeof(ResolutionsAndRefreshRates), "FindUnityResolution")]
    class RefreshRate
    {
        //An attempt at fixing an issue related to the avaialble refreh rates not getting updated, I found that unity will not even attempt to assign the given refresh rate if it is not supported.
        //The original intention was to allow these refresh rates to be applied anyways, but it is both unnecessary for most people, and impossible in general.
        //It is still possible to update the displayed refresh rates, but it's too much work for not enough fix.
        //
        
        static void Prefix(ResolutionsAndRefreshRates __instance, int resolutionIndex, ref int refreshRateIndex, ref int __state)
        {
            if (BepinPlugin.Bindings.DebugLogging.Value)
            {
                BepinPlugin.Log.LogInfo($"Targeted Resolution: {resolutionIndex}, Targeted RefreshRate: {refreshRateIndex}, Available Resolutions: {__instance.Resolutions.Count}");

                foreach (var thing in __instance.Resolutions)
                {
                    string rates = string.Empty;
                    foreach (double thingy in thing.RefreshRates)
                    {
                        rates += $" {thingy.ToString()}";
                    }
                    BepinPlugin.Log.LogInfo($"Height: {thing.height}, Width: {thing.width}, RefreshRates:{rates}");
                }
            }

            //If refresh rate is out of bounds, send to postfix and assign in-range value for vanilla. 
            if (refreshRateIndex >= __instance.Resolutions[resolutionIndex].RefreshRates.Count)
            {
                __state = refreshRateIndex;
                refreshRateIndex = 0;
            }
        }

        static void Postfix(ResolutionsAndRefreshRates __instance, int __state, ref Resolution __result)
        {
            if (__state != 0)
            {
                BepinPlugin.Log.LogInfo("Refresh Rate not in list, attempting to fix...");
                var RRR = GetRefreshRateFromIndex(__instance, __state);
                __result.refreshRateRatio = RRR;
                BepinPlugin.Log.LogInfo("Refresh Rate assigned as: " + RRR.value.ToString());
            }
        }

        static UnityEngine.RefreshRate GetRefreshRateFromIndex(ResolutionsAndRefreshRates __instance, int RefreshRateIndex)
        {
            int DefaultResIndex = __instance.ResolutionNames.Count - 1;
            if (ServiceBase<SystemPlatform>.Instance.DetectedSystemOS() == SystemOS.STEAM_OS)
            {
                DefaultResIndex = __instance.FindResolutionIndex(1280, 800);
            }

            return __instance.FindUnityResolution(DefaultResIndex, RefreshRateIndex).refreshRateRatio;
        }
        
    }*/
}
