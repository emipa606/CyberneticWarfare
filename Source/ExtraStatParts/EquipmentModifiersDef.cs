using System.Collections.Generic;
using Verse;

namespace ExtraStatParts;

public class EquipmentModifiersDef : Def
{
    public List<ThingDef> requireAnyOfTheseApparels = new List<ThingDef>();

    public List<HediffDef> requireAnyOfTheseHediffs = new List<HediffDef>();

    public List<ThingDef> requireAnyOfTheseRaces = new List<ThingDef>();
}