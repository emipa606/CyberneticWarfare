using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CyberneticWarfare;

public class CompWargearWeaponToggle : CompWargearWeapon
{
    private int activeVerbIndex;

    private List<VerbProperties> VerbProps => parent.def.Verbs;

    public new CompProperties_WargearWeaponToggle Props => (CompProperties_WargearWeaponToggle)props;

    public VerbProperties ActiveVerbProps => VerbProps[activeVerbIndex];

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
                    ? $" ({"CyborgWeaponry.VerbActive".Translate().RawText})"
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

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref activeVerbIndex, "activeVerbIndex");
    }
}