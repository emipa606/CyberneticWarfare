using HarmonyLib;
using Verse;

namespace CyberneticWarfare;

[HarmonyPatch(typeof(DamageWorker_AddInjury), "ApplyToPawn")]
public static class DamageWorker_AddInjury_ApplyToPawn
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