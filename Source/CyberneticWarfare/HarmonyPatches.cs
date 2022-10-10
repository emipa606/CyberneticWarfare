using HarmonyLib;
using Verse;

namespace CyberneticWarfare;

[StaticConstructorOnStartup]
internal static class HarmonyPatches
{
    static HarmonyPatches()
    {
        var harmonyInstance = new Harmony("rimworld.ogliss.CyberneticWarfare.wargearweapon");
        harmonyInstance.PatchAll();
    }
}