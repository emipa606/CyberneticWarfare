using System;
using Verse;

namespace CompOversizedWeapon
{
	// Token: 0x02000002 RID: 2
	public class CompOversizedWeapon : ThingComp
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public CompProperties_OversizedWeapon Props
		{
			get
			{
				return this.props as CompProperties_OversizedWeapon;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002060 File Offset: 0x00000260
		public CompOversizedWeapon()
		{
			bool flag = !(this.props is CompProperties_OversizedWeapon);
			if (flag)
			{
				this.props = new CompProperties_OversizedWeapon();
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020A3 File Offset: 0x000002A3
		public CompEquippable GetEquippable
		{
			get
			{
				ThingWithComps parent = this.parent;
				return (parent != null) ? parent.GetComp<CompEquippable>() : null;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020B7 File Offset: 0x000002B7
		public Pawn GetPawn
		{
			get
			{
				CompEquippable getEquippable = this.GetEquippable;
				Pawn result;
				if (getEquippable == null)
				{
					result = null;
				}
				else
				{
					VerbTracker verbTracker = getEquippable.verbTracker;
					if (verbTracker == null)
					{
						result = null;
					}
					else
					{
						Verb primaryVerb = verbTracker.PrimaryVerb;
						result = ((primaryVerb != null) ? primaryVerb.CasterPawn : null);
					}
				}
				return result;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020E4 File Offset: 0x000002E4
		public bool IsEquipped
		{
			get
			{
				bool flag = Find.TickManager.TicksGame % 60 != 0;
				bool result;
				if (flag)
				{
					result = this.isEquipped;
				}
				else
				{
					this.isEquipped = (this.GetPawn != null);
					result = this.isEquipped;
				}
				return result;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002128 File Offset: 0x00000328
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002130 File Offset: 0x00000330
		public bool FirstAttack
		{
			get
			{
				return this.firstAttack;
			}
			set
			{
				this.firstAttack = value;
			}
		}

		// Token: 0x04000001 RID: 1
		private bool isEquipped = false;

		// Token: 0x04000002 RID: 2
		private bool firstAttack = false;
	}
}
