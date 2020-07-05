using System;
using RimWorld;
using Verse;

namespace CyberneticWarfare
{
	// Token: 0x02000011 RID: 17
	public class Verb_ShootCW : Verb_LaunchProjectile
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000027F8 File Offset: 0x000009F8
		public VerbPropertiesCW VerbProps
		{
			get
			{
				return this.verbProps as VerbPropertiesCW;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002818 File Offset: 0x00000A18
		protected override int ShotsPerBurst
		{
			get
			{
				bool flag = this.VerbProps.rapidfire && this.caster.Position.InHorDistOf(this.currentTarget.Cell, this.verbProps.range / 2f);
				int result;
				if (flag)
				{
					result = this.verbProps.burstShotCount * 2;
				}
				else
				{
					result = this.verbProps.burstShotCount;
				}
				return result;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002888 File Offset: 0x00000A88
		public override void WarmupComplete()
		{
			base.WarmupComplete();
			Pawn pawn = this.currentTarget.Thing as Pawn;
			bool flag = pawn != null && !pawn.Downed && base.CasterIsPawn && base.CasterPawn.skills != null;
			if (flag)
			{
				float num = (!pawn.HostileTo(this.caster)) ? 20f : 170f;
				float num2 = this.verbProps.AdjustedFullCycleTime(this, base.CasterPawn);
				base.CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2, false);
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002920 File Offset: 0x00000B20
		protected override bool TryCastShot()
		{
			bool flag = base.TryCastShot();
			bool flag2 = flag && base.CasterIsPawn;
			if (flag2)
			{
				base.CasterPawn.records.Increment(RecordDefOf.ShotsFired);
			}
			bool flag3 = flag && this.VerbProps.pelletCount - 1 > 0;
			bool flag4 = flag3;
			if (flag4)
			{
				for (int i = 0; i < this.VerbProps.pelletCount - 1; i++)
				{
					base.TryCastShot();
				}
			}
			return flag;
		}
	}
}
