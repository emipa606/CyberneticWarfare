# GitHub Copilot Instructions for Cybernetic Warfare and Special Weapons (Continued)

## Mod Overview and Purpose

Cybernetic Warfare and Special Weapons (Continued) is an update to the original Trunken and Luizis mod, enhancing the game with a wide variety of advanced weapon systems, including energy claws, energy swords, ballistic combat guns, and more. The mod introduces new special features such as cleave, knockback, napalm, plasma, laser, and more, providing players with a rich and diverse arsenal to choose from. The mod is designed to add depth and excitement to gameplay, making it an essential addition for fans of RimWorld.

## Key Features and Systems

### Weaponry
- **Energy Claws**: Melee weapons with cleave ability.
- **Energy Sword**: Melee energy weapons.
- **Chain Sword**: Anti-personnel melee weapon.
- **Gun Blade**: Hybrid melee-ranged weapon.
- **Shocker**: Knuckle-based melee weapon with stun effects.
- **Disruptor**: Melee weapon with knockout effects.
- **Napalm Blaster**: Flamethrower with napalm effect.
- **Havoc Launcher**: Grenade launcher with multi-burst capability.
- **Dual Eraser**: Machine pistols for dual-wielding.
- **Riot Breaker**: Chain shotgun.
- **Ballistic Shredder**: SMG.
- **Ballistic Combat Gun**: AK-style assault rifle.
- **Ballistic Enforcer**: AR-style assault rifle.
- **Plasma Gun**: Burn-based energy weapon.
- **Recon Bolter**: Laser-based weapon.
- **Accelerator**: Hybrid energy-kinetic weapon.
- **Coil Gun**: Charged energy weapon.

### Special Features
- **Cleave**: Requires Jecstools for multiple target strikes.
- **Knockback**: Applies knockback effect to enemies.
- **Napalm**: Causes area-of-effect damage.
- **Plasma**: Burns enemies over time.
- **Laser**: High-precision ranged weapon.
- **Bio**: Biological weapons.
- **Orbs**: Various projectile types.
- **Electro**: Electric-based weapons.
- **Ferromagnetic**: Magnetic effects.
- **Explosives**: Explosive projectiles.
- **Multi Shots**: Multiple shots per attack.
- **Rapid Fire**: Fast firing rate.
- **Fire Modi**: Different firing modes for weapons.
- **Ammunition Changer**: Ability to switch between different types of ammunition.

### Cyberware
- **Bionic Arm**: Enhances melee combat.
- **Archotech Arm**: Advanced technology enhancements.
- **Power Arms**: Servo-motor enhancements.
- **Energy Core**: Power source for advanced weaponry.
- **Energy Link**: Energy transfer system.
- **Musclewire**: Enhanced muscle strength.

## Coding Patterns and Conventions

- **Modular Design**: The mod is structured into various components, each responsible for specific features or systems.
- **Code Reusability**: Common functionality is abstracted into reusable classes and methods.
- **Event-Driven**: The mod uses event-driven programming to handle interactions and updates.
- **Configuration Files**: XML files are used to define and configure various aspects of the mod, such as weapons, damage types, and hediffs.
- **Harmony Patches**: The mod uses Harmony patching to integrate with and modify existing RimWorld code.

## XML Integration

XML files are extensively used in this mod to define various elements such as:

- **DamageDefs**: Definitions for different types of damage.
- **ThingCategoryDefs**: Categories for different types of things.
- **HediffDefs**: Definitions for various hediffs and effects.
- **ThingDefs**: Definitions for weapons and other items.

### Example XML File

xml
<!-- Damages_MeleeWeapon.xml -->
<Defs>
  <DamageDef>
    <defName>MeleeWeapon_Cleave</defName>
    <label>Cleave</label>
    <description>Cleaves through multiple targets.</description>
    <category>Melee</category>
    <statBases>
      <armorPenetration>2</armorPenetration>
    </statBases>
  </DamageDef>
</Defs>


## Harmony Patching

Harmony patching is used to modify existing RimWorld code to integrate new features and behaviors. Patches are typically defined in C# files and use the Harmony library to apply changes.

### Example Harmony Patch

csharp
// Postfix.cs
using HarmonyLib;
using RimWorld;
using Verse;

namespace CyberneticWarfare
{
    [HarmonyPatch(typeof(DamageWorker_Blunt), "ApplySpecialEffectsToPart")]
    public static class DamageWorker_Blunt_Patch
    {
        public static void Postfix(DamageWorker_Blunt __instance, Pawn pawn, BodyPartRecord part, float totalDamage, DamageInfo dinfo)
        {
            if (dinfo.Def.defName == "MeleeWeapon_Cleave")
            {
                // Apply cleave effect
            }
        }
    }
}


## Suggestions for Copilot

- **C# Coding**: Use Copilot to assist with writing and modifying C# code, particularly for harmony patches and mod components.
- **XML Definitions**: Use Copilot to generate and update XML files for defining weapons, damage types, and other mod elements.
- **Mod Integration**: Use Copilot to help with integrating new features and systems into the mod, ensuring compatibility with existing code.
- **Documentation**: Use Copilot to generate documentation and comments for new code and features, improving maintainability and readability.
- **Debugging**: Use Copilot to assist with debugging, generating potential solutions for common issues and errors.

By following these guidelines, you can leverage GitHub Copilot to enhance your development workflow and create a high-quality mod for RimWorld.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

