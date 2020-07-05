using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CyberneticWarfare
{
	// Token: 0x0200000E RID: 14
	public class DamageWorker_ExtinguishNoCamShake : DamageWorker
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000024B8 File Offset: 0x000006B8
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			Fire fire = victim as Fire;
			bool flag = fire == null || fire.Destroyed;
			DamageWorker.DamageResult result;
			if (flag)
			{
				result = damageResult;
			}
			else
			{
				base.Apply(dinfo, victim);
				fire.fireSize -= dinfo.Amount * 0.01f;
				bool flag2 = fire.fireSize <= 0.1f;
				if (flag2)
				{
					fire.Destroy(DestroyMode.Vanish);
				}
				result = damageResult;
			}
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002530 File Offset: 0x00000730
		public override void ExplosionStart(Explosion explosion, List<IntVec3> cellsToAffect)
		{
			bool flag = this.def.explosionHeatEnergyPerCell > float.Epsilon;
			if (flag)
			{
				GenTemperature.PushHeat(explosion.Position, explosion.Map, this.def.explosionHeatEnergyPerCell * (float)cellsToAffect.Count);
			}
			MoteMaker.MakeStaticMote(explosion.Position, explosion.Map, ThingDefOf.Mote_ExplosionFlash, explosion.radius * 6f);
		}

		// Token: 0x04000007 RID: 7
		private const float DamageAmountToFireSizeRatio = 0.01f;
	}
}
