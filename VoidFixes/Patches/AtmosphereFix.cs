using CG.Ship.Modules;
using Gameplay.Atmosphere;
using HarmonyLib;
using System.Reflection;

namespace VoidFixes.Patches
{
    //18107's work, credit for this goes to him
    [HarmonyPatch(typeof(LifeSupportModule))]
    internal class LifeSupportModulePatch
    {
        private static readonly MethodInfo GetAtmosphereValuesMethod = AccessTools.Method(typeof(Atmosphere), "GetAtmosphereValues");
        private static readonly MethodInfo SetAtmosphereValuesMethod = AccessTools.Method(typeof(Atmosphere), "SetAtmosphereValues");

        [HarmonyPrefix]
        [HarmonyPatch("ApplyPressureIncrease")]
        static bool ApplyPressureIncrease()
        {
            return false; //Don't run original method
        }

        [HarmonyPostfix]
        [HarmonyPatch("ApplyOxygenIncrease")]
        static void ApplyOxygenIncrease(Atmosphere ____atmosphere)
        {
            foreach (Room room in ____atmosphere.Atmospheres.keys)
            {
                AtmosphereValues atmosphereValues = (AtmosphereValues)GetAtmosphereValuesMethod.Invoke(____atmosphere, new object[] { room });
                atmosphereValues.Pressure = atmosphereValues.Oxygen;
                SetAtmosphereValuesMethod.Invoke(____atmosphere, new object[] { room, atmosphereValues });
            }
        }
    }
}
