using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace CyberneticWarfare;

internal static class Patch_Pawn
{
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("GetGizmos")]
    [UsedImplicitly]
    private static class Patch_GetGizmos
    {
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