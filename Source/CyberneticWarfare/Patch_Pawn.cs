using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace CyberneticWarfare
{
    // Token: 0x02000009 RID: 9
    internal static class Patch_Pawn
    {
        // Token: 0x02000015 RID: 21
        [HarmonyPatch(typeof(Pawn))]
        [HarmonyPatch("GetGizmos")]
        [UsedImplicitly]
        private static class Patch_GetGizmos
        {
            // Token: 0x0600002D RID: 45 RVA: 0x00002B0C File Offset: 0x00000D0C
            private static void Postfix(Pawn __instance, ref IEnumerable<Gizmo> __result)
            {
                if (!__instance.IsColonistPlayerControlled)
                {
                    return;
                }

                if (!__instance.Drafted)
                {
                    return;
                }

                var comp = __instance.equipment?.Primary?.GetComp<CompWargearWeapon>();
                if (comp == null)
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