using System;
using HarmonyLib;
using Verse;

namespace CyberneticWarfare
{
	// Token: 0x02000008 RID: 8
	internal static class Patch_DamageWorker_AddInjury
	{
		// Token: 0x02000014 RID: 20
		[HarmonyPatch(typeof(DamageWorker_AddInjury))]
		[HarmonyPatch("ApplyToPawn")]
		private static class Patch_ApplyToPawn
		{
			// Token: 0x0600002C RID: 44 RVA: 0x00002AAC File Offset: 0x00000CAC
			private static void Postfix(DamageInfo dinfo, Pawn pawn)
			{
				bool flag = pawn.stances != null;
				if (flag)
				{
					DamageDefExtension damageDefExtension = dinfo.Def.GetModExtension<DamageDefExtension>() ?? DamageDefExtension.defaultValues;
					bool flag2 = damageDefExtension.stunDuration > 0;
					if (flag2)
					{
						pawn.stances.stunner.StunFor(damageDefExtension.stunDuration, dinfo.Instigator);
					}
				}
			}
		}
	}
}
