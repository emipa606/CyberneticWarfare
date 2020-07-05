using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Verse;

namespace CyberneticWarfare
{
	// Token: 0x02000009 RID: 9
	internal static class Patch_Pawn
	{
		// Token: 0x02000015 RID: 21
		[HarmonyPatch(typeof(Pawn))]
		[HarmonyPatch("GetGizmos")]
		private static class Patch_GetGizmos
		{
			// Token: 0x0600002D RID: 45 RVA: 0x00002B0C File Offset: 0x00000D0C
			private static void Postfix(Pawn __instance, ref IEnumerable<Gizmo> __result)
			{
				Pawn_EquipmentTracker equipment = new Pawn_EquipmentTracker(__instance);
				bool flag;
				if (__instance.IsColonistPlayerControlled && __instance.Drafted)
				{
					equipment = __instance.equipment;
					flag = (equipment != null);
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				if (flag2)
				{
					ThingWithComps primary = equipment.Primary;
					CompWargearWeapon comp = new CompWargearWeapon();
					bool flag3;
					if (primary != null)
					{
						comp = primary.GetComp<CompWargearWeapon>();
						flag3 = (comp != null);
					}
					else
					{
						flag3 = false;
					}
					bool flag4 = flag3;
					if (flag4)
					{
						var returnList = __result.ToList();
						foreach (Gizmo item in comp.EquippedGizmos())
						{
							returnList.Add(item);
						}
						__result = returnList;
					}
				}
			}
		}
	}
}
