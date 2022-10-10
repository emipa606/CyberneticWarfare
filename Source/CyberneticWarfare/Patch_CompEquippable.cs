using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace CyberneticWarfare;

internal static class Patch_CompEquippable
{
    [HarmonyPatch(typeof(CompEquippable))]
    [HarmonyPatch("PrimaryVerb", MethodType.Getter)]
    [UsedImplicitly]
    private static class Patch_PrimaryVerb
    {
        private static void Postfix(CompEquippable __instance, ref Verb __result)
        {
            var toggleComp = __instance.parent.GetComp<CompWargearWeaponToggle>();
            if (toggleComp != null)
            {
                __result = __instance.AllVerbs.First(v => v.verbProps == toggleComp.ActiveVerbProps);
            }
        }
    }
}