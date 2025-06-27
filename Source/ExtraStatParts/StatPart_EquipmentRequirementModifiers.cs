using RimWorld;
using Verse;

namespace ExtraStatParts;

public class StatPart_EquipmentRequirementModifiers : StatPart
{
    public override string ExplanationPart(StatRequest req)
    {
        StatModifier statModifier;
        Thing thing;
        EquipmentRequirementModifiers modExtension;
        string result;
        if ((statModifier = getStatModifier(req)) != null && (thing = req.Thing) != null &&
            (modExtension = thing.def.GetModExtension<EquipmentRequirementModifiers>()) != null)
        {
            result =
                "ESP_StatsReport_EquipmentRequirementsNotMet".Translate(modExtension.requirementNotMetReasonKey
                    .Translate()) + ": x" + statModifier.value.ToStringPercent();
        }
        else
        {
            result = null;
        }

        return result;
    }

    public override void TransformValue(StatRequest req, ref float val)
    {
        StatModifier statModifier;
        if ((statModifier = getStatModifier(req)) != null)
        {
            val *= statModifier.value;
        }
    }

    private StatModifier getStatModifier(StatRequest req)
    {
        EquipmentRequirementModifiers eqMods;
        Thing thing;
        StatModifier result;
        if ((thing = req.Thing) != null &&
            (eqMods = thing.def.GetModExtension<EquipmentRequirementModifiers>()) != null)
        {
            var statModifier = eqMods.modifyTheseStats.FirstOrDefault(modifier => modifier.stat == parentStat);
            if (statModifier == null)
            {
                result = null;
            }
            else
            {
                Pawn pawn = null;
                var holdingOwner = thing.holdingOwner;
                Pawn_EquipmentTracker pawn_EquipmentTracker;
                if ((pawn_EquipmentTracker =
                        holdingOwner?.Owner as Pawn_EquipmentTracker) != null)
                {
                    pawn = pawn_EquipmentTracker.pawn;
                }
                else
                {
                    var holdingOwner2 = thing.holdingOwner;
                    Pawn_ApparelTracker pawn_ApparelTracker;
                    if ((pawn_ApparelTracker =
                            holdingOwner2?.Owner as Pawn_ApparelTracker) != null)
                    {
                        pawn = pawn_ApparelTracker.pawn;
                    }
                }

                if (pawn == null)
                {
                    result = null;
                }
                else
                {
                    if (!eqMods.requireAnyOfTheseRaces.NullOrEmpty())
                    {
                        if (eqMods.requireAnyOfTheseRaces.Contains(pawn.def.defName))
                        {
                            return null;
                        }
                    }

                    if (!eqMods.requireAnyOfTheseHediffs.NullOrEmpty())
                    {
                        if (pawn.health.hediffSet.hediffs.Any(hediff =>
                                eqMods.requireAnyOfTheseHediffs.Contains(hediff.def.defName)))
                        {
                            return null;
                        }
                    }

                    if (!eqMods.requireAnyOfTheseApparels.NullOrEmpty() && pawn.apparel != null)
                    {
                        if (pawn.apparel.WornApparel.Any(apparel =>
                                eqMods.requireAnyOfTheseApparels.Contains(apparel.def.defName)))
                        {
                            return null;
                        }
                    }

                    result = statModifier;
                }
            }
        }
        else
        {
            result = null;
        }

        return result;
    }
}