using System;
using UnityEngine;
using Verse;

namespace CompOversizedWeapon
{
	// Token: 0x02000003 RID: 3
	public class CompProperties_OversizedWeapon : CompProperties
	{
		// Token: 0x06000008 RID: 8 RVA: 0x0000213C File Offset: 0x0000033C
		public CompProperties_OversizedWeapon()
		{
			this.compClass = typeof(CompOversizedWeapon);
		}

		// Token: 0x04000003 RID: 3
		public Vector3 offset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000004 RID: 4
		public Vector3 northOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000005 RID: 5
		public Vector3 eastOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000006 RID: 6
		public Vector3 southOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000007 RID: 7
		public Vector3 westOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04000008 RID: 8
		public bool verticalFlipOutsideCombat = false;

		// Token: 0x04000009 RID: 9
		public bool verticalFlipNorth = false;

		// Token: 0x0400000A RID: 10
		public bool isDualWeapon = false;

		// Token: 0x0400000B RID: 11
		public float angleAdjustmentEast = 0f;

		// Token: 0x0400000C RID: 12
		public float angleAdjustmentWest = 0f;

		// Token: 0x0400000D RID: 13
		public float angleAdjustmentNorth = 0f;

		// Token: 0x0400000E RID: 14
		public float angleAdjustmentSouth = 0f;

		// Token: 0x0400000F RID: 15
		public GraphicData groundGraphic = null;
	}
}
