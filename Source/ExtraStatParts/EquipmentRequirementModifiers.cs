using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ExtraStatParts;

public class EquipmentRequirementModifiers : DefModExtension
{
    public readonly List<StatModifier> modifyTheseStats = [];

    public readonly List<string> requireAnyOfTheseApparels = [];

    public readonly List<string> requireAnyOfTheseHediffs = [];

    public readonly List<string> requireAnyOfTheseRaces = [];

    public readonly string requirementNotMetReasonKey = "ESP_EquipmentRequirementNotMetReasonKey";
    public EquipmentModifiersDef def;
}