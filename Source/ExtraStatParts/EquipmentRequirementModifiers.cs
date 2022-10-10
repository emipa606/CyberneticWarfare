using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ExtraStatParts;

public class EquipmentRequirementModifiers : DefModExtension
{
    public EquipmentModifiersDef def;

    public List<StatModifier> modifyTheseStats = new List<StatModifier>();

    public List<string> requireAnyOfTheseApparels = new List<string>();

    public List<string> requireAnyOfTheseHediffs = new List<string>();

    public List<string> requireAnyOfTheseRaces = new List<string>();

    public string requirementNotMetReasonKey = "ESP_EquipmentRequirementNotMetReasonKey";
}