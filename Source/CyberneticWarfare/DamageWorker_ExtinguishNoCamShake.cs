using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CyberneticWarfare;

public class DamageWorker_ExtinguishNoCamShake : DamageWorker
{
    private const float DamageAmountToFireSizeRatio = 0.01f;

    public override DamageResult Apply(DamageInfo dinfo, Thing victim)
    {
        var damageResult = new DamageResult();
        if (victim is not Fire { Destroyed: false } fire)
        {
            return damageResult;
        }

        base.Apply(dinfo, victim);
        fire.fireSize -= dinfo.Amount * DamageAmountToFireSizeRatio;
        if (fire.fireSize <= 0.1f)
        {
            fire.Destroy();
        }

        return damageResult;
    }

    public override void ExplosionStart(Explosion explosion, List<IntVec3> cellsToAffect)
    {
        if (def.explosionHeatEnergyPerCell > float.Epsilon)
        {
            GenTemperature.PushHeat(explosion.Position, explosion.Map,
                def.explosionHeatEnergyPerCell * cellsToAffect.Count);
        }

        FleckMaker.Static(explosion.Position, explosion.Map, FleckDefOf.ExplosionFlash, explosion.radius * 6f);
    }
}