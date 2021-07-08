using System.Collections.Generic;
using Verse;

namespace ExtraStatParts
{
    // Token: 0x02000002 RID: 2
    public class EquipmentModifiersDef : Def
    {
        // Token: 0x04000002 RID: 2
        public List<ThingDef> requireAnyOfTheseApparels = new List<ThingDef>();

        // Token: 0x04000001 RID: 1
        public List<HediffDef> requireAnyOfTheseHediffs = new List<HediffDef>();

        // Token: 0x04000003 RID: 3
        public List<ThingDef> requireAnyOfTheseRaces = new List<ThingDef>();
    }
}