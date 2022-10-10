using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace CyberneticWarfare;

internal static class Patch_DamageWorker_AddInjury
{
    [HarmonyPatch(typeof(DamageWorker_AddInjury))]
    [HarmonyPatch("ApplyToPawn")]
    [UsedImplicitly]
    private static class Patch_ApplyToPawn
    {
        private static void Postfix(DamageInfo dinfo, Pawn pawn)
        {
            if (pawn.stances == null)
            {
                return;
            }

            var damageDefExtension = dinfo.Def.GetModExtension<DamageDefExtension>() ??
                                     DamageDefExtension.defaultValues;
            if (damageDefExtension.stunDuration > 0)
            {
                pawn.stances.stunner.StunFor(damageDefExtension.stunDuration, dinfo.Instigator);
            }
        }
    }
}