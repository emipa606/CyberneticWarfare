using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CyberneticWarfare
{
    // Token: 0x0200000C RID: 12
    public class CompWargearWeaponToggle : CompWargearWeapon
    {
        // Token: 0x04000006 RID: 6
        private int activeVerbIndex;

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x0600000E RID: 14 RVA: 0x000023D6 File Offset: 0x000005D6
        private List<VerbProperties> VerbProps => parent.def.Verbs;

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x0600000F RID: 15 RVA: 0x000023E8 File Offset: 0x000005E8
        public new CompProperties_WargearWeaponToggle Props => (CompProperties_WargearWeaponToggle)props;

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000010 RID: 16 RVA: 0x000023F5 File Offset: 0x000005F5
        public VerbProperties ActiveVerbProps => VerbProps[activeVerbIndex];

        // Token: 0x06000011 RID: 17 RVA: 0x00002408 File Offset: 0x00000608
        public override IEnumerable<Gizmo> EquippedGizmos()
        {
            var baseGroupKey = 700000101;
            int num;
            for (var i = 0; i < VerbProps.Count; i = num + 1)
            {
                var j = i;
                var verbSwitchCommand = new Command_Action
                {
                    defaultLabel = VerbProps[i].label.CapitalizeFirst() + (i == activeVerbIndex
                        ? " (" + "CyborgWeaponry.VerbActive".Translate().RawText + ")"
                        : string.Empty),
                    defaultDesc = "CyborgWeaponry.SwitchFireMode".Translate(VerbProps[i].label),
                    icon = VerbProps[i].defaultProjectile.uiIcon,
                    activateSound = SoundDefOf.Click,
                    groupKey = baseGroupKey + i,
                    action = delegate { activeVerbIndex = j; }
                };
                var verbProperties = VerbProps[i];
                if (verbProperties is VerbPropertiesCW verbPropsCW && !verbPropsCW.researchPrerequisites.NullOrEmpty())
                {
                    foreach (var research in verbPropsCW.researchPrerequisites)
                    {
                        if (!research.IsFinished)
                        {
                            verbSwitchCommand.Disable("CyborgWeaponry.ResearchRequirementNotMet".Translate());
                        }
                    }
                }

                yield return verbSwitchCommand;
                num = i;
            }
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00002418 File Offset: 0x00000618
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref activeVerbIndex, "activeVerbIndex");
        }
    }
}