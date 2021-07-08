using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CyberneticWarfare
{
    // Token: 0x0200000F RID: 15
    public class DamageWorker_FlameNoCamShake : DamageWorker_AddInjury
    {
        // Token: 0x06000019 RID: 25 RVA: 0x000025A8 File Offset: 0x000007A8
        public override DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
            var pawn = victim as Pawn;
            if (pawn != null && pawn.Faction == Faction.OfPlayer)
            {
                Find.TickManager.slower.SignalForceNormalSpeedShort();
            }

            var map = victim.Map;
            var damageResult = base.Apply(dinfo, victim);
            if (!damageResult.deflected && !dinfo.InstantPermanentInjury)
            {
                victim.TryAttachFire(Rand.Range(0.15f, 0.25f));
            }

            if (!victim.Destroyed || map == null || pawn != null)
            {
                return damageResult;
            }

            foreach (var intVec in victim.OccupiedRect())
            {
                FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_Ash);
            }

            if (victim is not Plant plant || !victim.def.plant.IsTree || plant.LifeStage == PlantLifeStage.Sowing ||
                victim.def == ThingDefOf.BurnedTree)
            {
                return damageResult;
            }

            var deadPlant = (DeadPlant) GenSpawn.Spawn(ThingDefOf.BurnedTree, victim.Position, map);
            deadPlant.Growth = plant.Growth;

            return damageResult;
        }

        // Token: 0x0600001A RID: 26 RVA: 0x00002710 File Offset: 0x00000910
        public virtual void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings,
            bool canThrowMotes)
        {
            base.ExplosionAffectCell(explosion, c, damagedThings, new List<Thing>(), canThrowMotes);
            if (def == DamageDefOf.Flame && Rand.Chance(FireUtility.ChanceToStartFireIn(c, explosion.Map)))
            {
                FireUtility.TryStartFireIn(c, explosion.Map, Rand.Range(0.2f, 0.6f));
            }
        }

        // Token: 0x0600001B RID: 27 RVA: 0x00002770 File Offset: 0x00000970
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