using System;
using RimWorld;
using Verse;

namespace EQEnergyWeapons
{
	// Token: 0x02000005 RID: 5
	[DefOf]
	public static class ThingDefOf
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002D34 File Offset: 0x00000F34
		static ThingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
		}

		// Token: 0x04000013 RID: 19
		public static ThingDef LaserMoteWorker;
	}
}
