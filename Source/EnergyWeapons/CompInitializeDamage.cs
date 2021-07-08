using Verse;

namespace EQEnergyWeapons
{
    // Token: 0x02000002 RID: 2
    public class CompInitializeDamage : ThingComp
    {
        // Token: 0x04000004 RID: 4
        public float chanceToProc;

        // Token: 0x04000001 RID: 1
        public CompProperties_DefinitionDMG compProp;

        // Token: 0x04000005 RID: 5
        public int count;

        // Token: 0x04000003 RID: 3
        public int damageAmount;

        // Token: 0x04000002 RID: 2
        public string damageDef;

        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
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

        // Token: 0x06000002 RID: 2 RVA: 0x000020CF File Offset: 0x000002CF
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref count, "count", 1);
        }
    }
}