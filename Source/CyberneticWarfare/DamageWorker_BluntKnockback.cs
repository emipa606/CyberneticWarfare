using System;
using Verse;

namespace CyberneticWarfare
{
	// Token: 0x02000004 RID: 4
	public class DamageWorker_BluntKnockback : DamageWorker_Blunt
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020C4 File Offset: 0x000002C4
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.ApplySpecialEffectsToPart(pawn, totalDamage, dinfo, result);
			Map mapHeld = pawn.MapHeld;
			DamageDefExtension damageDefExtension = dinfo.Def.GetModExtension<DamageDefExtension>() ?? DamageDefExtension.defaultValues;
			int num = GenMath.RoundRandom(totalDamage * damageDefExtension.knockbackDistancePerDamageDealt * (damageDefExtension.scaleKnockbackWithBodySize ? (1f / pawn.BodySize) : 1f));
			IntVec3 b = CyberneticWarfareUtility.IntVec3FromDirection8Way(CyberneticWarfareUtility.Direction8WayFromAngle((pawn.PositionHeld - dinfo.Instigator.PositionHeld).AngleFlat));
			bool flag = num > 0;
			if (flag)
			{
				float num2 = (float)damageDefExtension.stunDuration;
				for (int i = 0; i < num; i++)
				{
					IntVec3 c = pawn.PositionHeld + b;
					bool flag2 = !c.InBounds(mapHeld);
					if (flag2)
					{
						break;
					}
					bool flag3 = c.Impassable(mapHeld);
					if (flag3)
					{
						num2 *= damageDefExtension.hitBuildingStunDurationFactor;
						break;
					}
					IThingHolder parentHolder = pawn.ParentHolder;
					Corpse corpse = parentHolder as Corpse;
					bool flag4 = corpse != null;
					if (flag4)
					{
						corpse.Position += b;
					}
					pawn.Position += b;
				}
				bool flag5 = !pawn.Dead;
				if (flag5)
				{
					pawn.Notify_Teleported(true, false);
				}
				bool flag6 = num2 > 0f && pawn.stances != null;
				if (flag6)
				{
					pawn.stances.stunner.StunFor(GenMath.RoundRandom(num2), dinfo.Instigator);
				}
			}
		}
	}
}
