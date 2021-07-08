using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CyberneticWarfare
{
    // Token: 0x0200000D RID: 13
    public class DamageWorker_BombNoCamShake : DamageWorker_AddInjury
    {
        // Token: 0x06000014 RID: 20 RVA: 0x00002440 File Offset: 0x00000640
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
}