using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CyberneticWarfare
{
    // Token: 0x0200000E RID: 14
    public class DamageWorker_ExtinguishNoCamShake : DamageWorker
    {
        // Token: 0x04000007 RID: 7
        private const float DamageAmountToFireSizeRatio = 0.01f;

        // Token: 0x06000016 RID: 22 RVA: 0x000024B8 File Offset: 0x000006B8
        public override DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
            var damageResult = new DamageResult();
            DamageResult result;
            if (!(victim is Fire fire) || fire.Destroyed)
            {
                result = damageResult;
            }
            else
            {
                base.Apply(dinfo, victim);
                fire.fireSize -= dinfo.Amount * 0.01f;
                if (fire.fireSize <= 0.1f)
                {
                    fire.Destroy();
                }

                result = damageResult;
            }

            return result;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00002530 File Offset: 0x00000730
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