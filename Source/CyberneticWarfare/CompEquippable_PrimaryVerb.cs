using HarmonyLib;
using Verse;

namespace CyberneticWarfare;

[HarmonyPatch(typeof(CompEquippable), nameof(CompEquippable.PrimaryVerb), MethodType.Getter)]
public static class CompEquippable_PrimaryVerb
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