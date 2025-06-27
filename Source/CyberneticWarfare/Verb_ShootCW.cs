using RimWorld;
using Verse;

namespace CyberneticWarfare;

public class Verb_ShootCW : Verb_LaunchProjectile
{
    private VerbPropertiesCW VerbProps => verbProps as VerbPropertiesCW;

    protected override int ShotsPerBurst
    {
        get
        {
            int result;
            if (VerbProps.rapidfire && caster.Position.InHorDistOf(currentTarget.Cell, verbProps.range / 2f))
            {
                result = verbProps.burstShotCount * 2;
            }
            else
            {
                result = verbProps.burstShotCount;
            }

            return result;
        }
    }

    public override void WarmupComplete()
    {
        base.WarmupComplete();
        if (currentTarget.Thing is not Pawn pawn || pawn.Downed || !base.CasterIsPawn ||
            base.CasterPawn.skills == null)
        {
            return;
        }

        var num = !pawn.HostileTo(caster) ? 20f : 170f;
        var num2 = verbProps.AdjustedFullCycleTime(this, base.CasterPawn);
        base.CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2);
    }

    protected override bool TryCastShot()
    {
        if (base.TryCastShot() && base.CasterIsPawn)
        {
            base.CasterPawn.records.Increment(RecordDefOf.ShotsFired);
        }

        if (!base.TryCastShot() || VerbProps.pelletCount - 1 <= 0)
        {
            return base.TryCastShot();
        }

        for (var i = 0; i < VerbProps.pelletCount - 1; i++)
        {
            base.TryCastShot();
        }

        return base.TryCastShot();
    }
}