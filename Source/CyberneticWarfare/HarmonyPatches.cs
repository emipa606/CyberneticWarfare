using HarmonyLib;
using Verse;

namespace CyberneticWarfare;

[StaticConstructorOnStartup]
internal static class HarmonyPatches
{
    static HarmonyPatches()
    {
        new Harmony("rimworld.ogliss.CyberneticWarfare.wargearweapon").PatchAll();
    }
}