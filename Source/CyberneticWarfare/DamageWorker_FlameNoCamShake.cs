using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CyberneticWarfare;

public class DamageWorker_FlameNoCamShake : DamageWorker_AddInjury
{
    private readonly ThingDef burnedTree = ThingDef.Named("BurnedTree");

    public override DamageResult Apply(DamageInfo dinfo, Thing thing)
    {
        var pawn = thing as Pawn;
        if (pawn != null && pawn.Faction == Faction.OfPlayer)
        {
            Find.TickManager.slower.SignalForceNormalSpeedShort();
        }

        var map = thing.Map;
        var damageResult = base.Apply(dinfo, thing);
        if (!damageResult.deflected && !dinfo.InstantPermanentInjury)
        {
            thing.TryAttachFire(Rand.Range(0.15f, 0.25f), thing);
        }

        if (!thing.Destroyed || map == null || pawn != null)
        {
            return damageResult;
        }

        foreach (var intVec in thing.OccupiedRect())
        {
            FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_Ash);
        }

        if (thing is not Plant plant || !thing.def.plant.IsTree || plant.LifeStage == PlantLifeStage.Sowing ||
            thing.def == burnedTree)
        {
            return damageResult;
        }

        var deadPlant = (DeadPlant)GenSpawn.Spawn(burnedTree, thing.Position, map);
        deadPlant.Growth = plant.Growth;

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