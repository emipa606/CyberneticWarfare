using RimWorld;
using Verse;

namespace EQEnergyWeapons;

[DefOf]
public static class ThingDefOf
{
    public static ThingDef LaserMoteWorker;

    static ThingDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
    }
}