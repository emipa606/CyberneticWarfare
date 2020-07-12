using System;
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
			EquipmentRequirementModifiers modExtension = new EquipmentRequirementModifiers();
			bool flag = (statModifier = this.GetStatModifier(req)) != null && (thing = req.Thing) != null && (modExtension = thing.def.GetModExtension<EquipmentRequirementModifiers>()) != null;
			string result;
			if (flag)
			{

				result = TranslatorFormattedStringExtensions.Translate("ESP_StatsReport_EquipmentRequirementsNotMet", Translator.Translate(modExtension.requirementNotMetReasonKey)) + ": x" + statModifier.value.ToStringPercent();
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
			bool flag = (statModifier = this.GetStatModifier(req)) != null;
			if (flag)
			{
				val *= statModifier.value;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002160 File Offset: 0x00000360
		private StatModifier GetStatModifier(StatRequest req)
		{
			EquipmentRequirementModifiers eqMods = new EquipmentRequirementModifiers();
			Thing thing;
			bool flag = (thing = req.Thing) != null && (eqMods = thing.def.GetModExtension<EquipmentRequirementModifiers>()) != null;
			StatModifier result;
			if (flag)
			{
				StatModifier statModifier = eqMods.modifyTheseStats.FirstOrDefault((StatModifier modifier) => modifier.stat == this.parentStat);
				bool flag2 = statModifier == null;
				if (flag2)
				{
					result = null;
				}
				else
				{
					Pawn pawn = null;
					ThingOwner holdingOwner = thing.holdingOwner;
					Pawn_EquipmentTracker pawn_EquipmentTracker;
					bool flag3 = (pawn_EquipmentTracker = (((holdingOwner != null) ? holdingOwner.Owner : null) as Pawn_EquipmentTracker)) != null;
					if (flag3)
					{
						pawn = pawn_EquipmentTracker.pawn;
					}
					else
					{
						ThingOwner holdingOwner2 = thing.holdingOwner;
						Pawn_ApparelTracker pawn_ApparelTracker;
						bool flag4 = (pawn_ApparelTracker = (((holdingOwner2 != null) ? holdingOwner2.Owner : null) as Pawn_ApparelTracker)) != null;
						if (flag4)
						{
							pawn = pawn_ApparelTracker.pawn;
						}
					}
					bool flag5 = pawn == null;
					if (flag5)
					{
						result = null;
					}
					else
					{
						bool flag6 = !eqMods.requireAnyOfTheseRaces.NullOrEmpty<string>();
						if (flag6)
						{
							bool flag7 = eqMods.requireAnyOfTheseRaces.Contains(pawn.def.defName);
							if (flag7)
							{
								return null;
							}
						}
						bool flag8 = !eqMods.requireAnyOfTheseHediffs.NullOrEmpty<string>();
						if (flag8)
						{
							bool flag9 = pawn.health.hediffSet.hediffs.Any((Hediff hediff) => eqMods.requireAnyOfTheseHediffs.Contains(hediff.def.defName));
							if (flag9)
							{
								return null;
							}
						}
						bool flag10 = !eqMods.requireAnyOfTheseApparels.NullOrEmpty<string>() && pawn.apparel != null;
						if (flag10)
						{
							bool flag11 = pawn.apparel.WornApparel.Any((Apparel apparel) => eqMods.requireAnyOfTheseApparels.Contains(apparel.def.defName));
							if (flag11)
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
