using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CyberneticWarfare
{
    // Token: 0x0200000F RID: 15
    public class DamageWorker_FlameNoCamShake : DamageWorker_AddInjury
    {
        // Token: 0x06000019 RID: 25 RVA: 0x000025A8 File Offset: 0x000007A8
        public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
            Pawn pawn = victim as Pawn;
            bool flag = pawn != null && pawn.Faction == Faction.OfPlayer;
            if (flag)
            {
                Find.TickManager.slower.SignalForceNormalSpeedShort();
            }
            Map map = victim.Map;
            DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);
            bool flag2 = !damageResult.deflected && !dinfo.InstantPermanentInjury;
            if (flag2)
            {
                victim.TryAttachFire(Rand.Range(0.15f, 0.25f));
            }
            bool flag3 = victim.Destroyed && map != null && pawn == null;
            if (flag3)
            {
                foreach (IntVec3 intVec in victim.OccupiedRect())
                {
                    FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_Ash, 1);
                }
                Plant plant = victim as Plant;
                bool flag4 = plant != null && victim.def.plant.IsTree && plant.LifeStage != PlantLifeStage.Sowing && victim.def != ThingDefOf.BurnedTree;
                if (flag4)
                {
                    DeadPlant deadPlant = (DeadPlant)GenSpawn.Spawn(ThingDefOf.BurnedTree, victim.Position, map, WipeMode.Vanish);
                    deadPlant.Growth = plant.Growth;
                }
            }
            return damageResult;
        }

        // Token: 0x0600001A RID: 26 RVA: 0x00002710 File Offset: 0x00000910
        public virtual void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, bool canThrowMotes)
        {
            base.ExplosionAffectCell(explosion, c, damagedThings, new List<Thing>(), canThrowMotes);
            bool flag = this.def == DamageDefOf.Flame && Rand.Chance(FireUtility.ChanceToStartFireIn(c, explosion.Map));
            if (flag)
            {
                FireUtility.TryStartFireIn(c, explosion.Map, Rand.Range(0.2f, 0.6f));
            }
        }

        // Token: 0x0600001B RID: 27 RVA: 0x00002770 File Offset: 0x00000970
        public override void ExplosionStart(Explosion explosion, List<IntVec3> cellsToAffect)
        {
            bool flag = this.def.explosionHeatEnergyPerCell > float.Epsilon;
            if (flag)
            {
                GenTemperature.PushHeat(explosion.Position, explosion.Map, this.def.explosionHeatEnergyPerCell * (float)cellsToAffect.Count);
            }
            MoteMaker.MakeStaticMote(explosion.Position, explosion.Map, ThingDefOf.Mote_ExplosionFlash, explosion.radius * 6f);
        }
    }
}
