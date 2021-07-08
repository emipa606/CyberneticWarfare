using System.Linq;
using RimWorld;
using Verse;

namespace ExtraStatParts
{
    // Token: 0x02000004 RID: 4
    public class StatPart_EquipmentRequirementModifiers : StatPart
    {
        // Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
        public override string ExplanationPart(StatRequest req)
        {
            StatModifier statModifier;
            Thing thing;
            EquipmentRequirementModifiers modExtension;
            string result;
            if ((statModifier = GetStatModifier(req)) != null && (thing = req.Thing) != null &&
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

        // Token: 0x06000004 RID: 4 RVA: 0x00002134 File Offset: 0x00000334
        public override void TransformValue(StatRequest req, ref float val)
        {
            StatModifier statModifier;
            if ((statModifier = GetStatModifier(req)) != null)
            {
                val *= statModifier.value;
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002160 File Offset: 0x00000360
        private StatModifier GetStatModifier(StatRequest req)
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
}