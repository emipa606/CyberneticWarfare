using System.Collections.Generic;
using Verse;

namespace ExtraStatParts;

public class EquipmentModifiersDef : Def
{
    public List<ThingDef> requireAnyOfTheseApparels = [];

    public List<HediffDef> requireAnyOfTheseHediffs = [];

    public List<ThingDef> requireAnyOfTheseRaces = [];
}