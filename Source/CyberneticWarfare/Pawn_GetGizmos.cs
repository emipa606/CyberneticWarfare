using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Verse;

namespace CyberneticWarfare;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.GetGizmos))]
public static class Pawn_GetGizmos
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