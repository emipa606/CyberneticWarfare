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
            if (toggleComp == null)
            {
                return;
            }

            var selectedVerb = __instance.AllVerbs?.FirstOrDefault(v => v.verbProps == toggleComp.ActiveVerbProps);
            if (selectedVerb != null)
            {
                __result = selectedVerb;
            }
        }
    }
}