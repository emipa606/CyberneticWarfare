using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CyberneticWarfare
{
	// Token: 0x0200000D RID: 13
	public class DamageWorker_BombNoCamShake : DamageWorker_AddInjury
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002440 File Offset: 0x00000640
		public override void ExplosionStart(Explosion explosion, List<IntVec3> cellsToAffect)
		{
			bool flag = this.def.explosionHeatEnergyPerCell > float.Epsilon;
			if (flag)
			{
				GenTemperature.PushHeat(explosion.Position, explosion.Map, this.def.explosionHeatEnergyPerCell * (float)cellsToAffect.Count);
			}
			MoteMaker.MakeStaticMote(explosion.Position, explosion.Map, ThingDefOf.Mote_ExplosionFlash, explosion.radius * 6f);
		}
	}
}
