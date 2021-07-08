using RimWorld;
using Verse;

namespace CyberneticWarfare
{
    // Token: 0x02000011 RID: 17
    public class Verb_ShootCW : Verb_LaunchProjectile
    {
        // Token: 0x17000006 RID: 6
        // (get) Token: 0x0600001E RID: 30 RVA: 0x000027F8 File Offset: 0x000009F8
        public VerbPropertiesCW VerbProps => verbProps as VerbPropertiesCW;

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x0600001F RID: 31 RVA: 0x00002818 File Offset: 0x00000A18
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

        // Token: 0x06000020 RID: 32 RVA: 0x00002888 File Offset: 0x00000A88
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

        // Token: 0x06000021 RID: 33 RVA: 0x00002920 File Offset: 0x00000B20
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
}