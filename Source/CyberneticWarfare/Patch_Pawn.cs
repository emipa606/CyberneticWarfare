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
                var equipment = new Pawn_EquipmentTracker(__instance);
                bool b;
                if (__instance.IsColonistPlayerControlled && __instance.Drafted)
                {
                    equipment = __instance.equipment;
                    b = equipment != null;
                }
                else
                {
                    b = false;
                }

                if (!b)
                {
                    return;
                }

                var primary = equipment.Primary;
                var comp = new CompWargearWeapon();
                bool b1;
                if (primary != null)
                {
                    comp = primary.GetComp<CompWargearWeapon>();
                    b1 = comp != null;
                }
                else
                {
                    b1 = false;
                }

                if (!b1)
                {
                    return;
                }

                var returnList = __result.ToList();
                foreach (var item in comp.EquippedGizmos())
                {
                    returnList.Add(item);
                }

                __result = returnList;
            }
        }
    }
}