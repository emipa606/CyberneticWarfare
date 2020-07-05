using System;
using HarmonyLib;
using Verse;

namespace CyberneticWarfare
{
	// Token: 0x0200000A RID: 10
	[StaticConstructorOnStartup]
	internal static class HarmonyPatches
	{
		// Token: 0x0600000C RID: 12 RVA: 0x0000239C File Offset: 0x0000059C
		static HarmonyPatches()
		{
			var harmonyInstance = new Harmony("rimworld.ogliss.CyberneticWarfare.wargearweapon");
			harmonyInstance.PatchAll();
		}
	}
}
