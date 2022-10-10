using System.Collections.Generic;
using Verse;

namespace CyberneticWarfare;

public class VerbPropertiesCW : VerbProperties
{
    public int pelletCount = 1;

    public bool rapidfire = false;

    public List<ResearchProjectDef> researchPrerequisites;
}