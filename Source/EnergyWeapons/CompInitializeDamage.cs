using Verse;

namespace EQEnergyWeapons;

public class CompInitializeDamage : ThingComp
{
    public float chanceToProc;

    public CompProperties_DefinitionDMG compProp;

    public int count;

    public int damageAmount;

    public string damageDef;

    public override void Initialize(CompProperties vprops)
    {
        base.Initialize(vprops);
        compProp = vprops as CompProperties_DefinitionDMG;
        if (compProp != null)
        {
            damageDef = compProp.damageDef;
            damageAmount = compProp.damageAmount;
            chanceToProc = compProp.chanceToProc;
        }
        else
        {
            Log.Message("Error");
            count = 9876;
        }
    }

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref count, "count", 1);
    }
}