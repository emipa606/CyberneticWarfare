using System;
using System.Linq;
using HarmonyLib;
using Verse;

namespace CyberneticWarfare
{
	// Token: 0x02000007 RID: 7
	internal static class Patch_CompEquippable
	{
		// Token: 0x02000013 RID: 19
		[HarmonyPatch(typeof(CompEquippable))]
		[HarmonyPatch("PrimaryVerb", MethodType.Getter)]
		private static class Patch_PrimaryVerb
		{
			// Token: 0x0600002B RID: 43 RVA: 0x00002A60 File Offset: 0x00000C60
			private static void Postfix(CompEquippable __instance, ref Verb __result)
			{
				CompWargearWeaponToggle toggleComp = __instance.parent.GetComp<CompWargearWeaponToggle>();
				bool flag = toggleComp != null;
				if (flag)
				{
					__result = __instance.AllVerbs.First((Verb v) => v.verbProps == toggleComp.ActiveVerbProps);
				}
			}
		}
	}
}
