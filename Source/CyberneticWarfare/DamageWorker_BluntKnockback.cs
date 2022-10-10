using Verse;

namespace CyberneticWarfare;

public class DamageWorker_BluntKnockback : DamageWorker_Blunt
{
    protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo,
        DamageResult result)
    {
        base.ApplySpecialEffectsToPart(pawn, totalDamage, dinfo, result);
        var mapHeld = pawn.MapHeld;
        var damageDefExtension =
            dinfo.Def.GetModExtension<DamageDefExtension>() ?? DamageDefExtension.defaultValues;
        var num = GenMath.RoundRandom(totalDamage * damageDefExtension.knockbackDistancePerDamageDealt *
                                      (damageDefExtension.scaleKnockbackWithBodySize ? 1f / pawn.BodySize : 1f));
        var b = CyberneticWarfareUtility.IntVec3FromDirection8Way(
            CyberneticWarfareUtility.Direction8WayFromAngle((pawn.PositionHeld - dinfo.Instigator.PositionHeld)
                .AngleFlat));
        if (num <= 0)
        {
            return;
        }

        float num2 = damageDefExtension.stunDuration;
        for (var i = 0; i < num; i++)
        {
            var c = pawn.PositionHeld + b;
            if (!c.InBounds(mapHeld))
            {
                break;
            }

            if (c.Impassable(mapHeld))
            {
                num2 *= damageDefExtension.hitBuildingStunDurationFactor;
                break;
            }

            var parentHolder = pawn.ParentHolder;
            if (parentHolder is Corpse corpse)
            {
                corpse.Position += b;
            }

            pawn.Position += b;
        }

        if (!pawn.Dead)
        {
            pawn.Notify_Teleported(true, false);
        }

        if (num2 > 0f)
        {
            pawn.stances?.stunner.StunFor(GenMath.RoundRandom(num2), dinfo.Instigator);
        }
    }
}