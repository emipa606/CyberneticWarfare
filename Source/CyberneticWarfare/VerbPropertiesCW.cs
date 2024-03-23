using System.Collections.Generic;
using Verse;

namespace CyberneticWarfare;

public class VerbPropertiesCW : VerbProperties
{
    public readonly int pelletCount = 1;

    public readonly bool rapidfire = false;

    public List<ResearchProjectDef> researchPrerequisites;
}