using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ExtraStatParts
{
	// Token: 0x02000003 RID: 3
	public class EquipmentRequirementModifiers : DefModExtension
	{
		// Token: 0x04000004 RID: 4
		public string requirementNotMetReasonKey = "ESP_EquipmentRequirementNotMetReasonKey";

		// Token: 0x04000005 RID: 5
		public List<StatModifier> modifyTheseStats = new List<StatModifier>();

		// Token: 0x04000006 RID: 6
		public EquipmentModifiersDef def;

		// Token: 0x04000007 RID: 7
		public List<string> requireAnyOfTheseHediffs = new List<string>();

		// Token: 0x04000008 RID: 8
		public List<string> requireAnyOfTheseApparels = new List<string>();

		// Token: 0x04000009 RID: 9
		public List<string> requireAnyOfTheseRaces = new List<string>();
	}
}
