using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CyberneticWarfare
{
	// Token: 0x0200000C RID: 12
	public class CompWargearWeaponToggle : CompWargearWeapon
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000023D6 File Offset: 0x000005D6
		private List<VerbProperties> VerbProps
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000023E8 File Offset: 0x000005E8
		public new CompProperties_WargearWeaponToggle Props
		{
			get
			{
				return (CompProperties_WargearWeaponToggle)this.props;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000023F5 File Offset: 0x000005F5
		public VerbProperties ActiveVerbProps
		{
			get
			{
				return this.VerbProps[this.activeVerbIndex];
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002408 File Offset: 0x00000608
		public override IEnumerable<Gizmo> EquippedGizmos()
		{
			int baseGroupKey = 700000101;
			int num = 0;
			for (int i = 0; i < this.VerbProps.Count; i = num + 1)
			{
				int j = i;
				Command_Action verbSwitchCommand = new Command_Action
				{
					defaultLabel = this.VerbProps[i].label.CapitalizeFirst() + ((i == this.activeVerbIndex) ? (" (" + Translator.Translate("CyborgWeaponry.VerbActive").RawText + ")") : string.Empty),
					defaultDesc = TranslatorFormattedStringExtensions.Translate("CyborgWeaponry.SwitchFireMode", this.VerbProps[i].label),
					icon = this.VerbProps[i].defaultProjectile.uiIcon,
					activateSound = SoundDefOf.Click,
					groupKey = baseGroupKey + i,
					action = delegate()
					{
						this.activeVerbIndex = j;
					}
				};
				VerbProperties verbProperties = this.VerbProps[i];
				VerbPropertiesCW verbPropsCW = verbProperties as VerbPropertiesCW;
				bool flag = verbPropsCW != null && !verbPropsCW.researchPrerequisites.NullOrEmpty<ResearchProjectDef>();
				if (flag)
				{
					foreach (ResearchProjectDef research in verbPropsCW.researchPrerequisites)
					{
						bool flag2 = !research.IsFinished;
						if (flag2)
						{
							verbSwitchCommand.Disable(Translator.Translate("CyborgWeaponry.ResearchRequirementNotMet"));
						}
					}
					List<ResearchProjectDef>.Enumerator enumerator = default(List<ResearchProjectDef>.Enumerator);
				}
				yield return verbSwitchCommand;
				verbSwitchCommand = null;
				verbPropsCW = null;
				num = i;
			}
			yield break;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002418 File Offset: 0x00000618
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.activeVerbIndex, "activeVerbIndex", 0, false);
		}

		// Token: 0x04000006 RID: 6
		private int activeVerbIndex;
	}
}
