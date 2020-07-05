using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace EQEnergyWeapons
{
	// Token: 0x02000004 RID: 4
	public class Projectile_BaseEnergyImpact : Projectile
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002111 File Offset: 0x00000311
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.drawingTexture = this.def.DrawMatSingle;
			this.compED = base.GetComp<CompInitializeDamage>();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x0000213A File Offset: 0x0000033A
		public ThingDef_EnergyWeaponsBase Props
		{
			get
			{
				return this.def as ThingDef_EnergyWeaponsBase;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002147 File Offset: 0x00000347
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter", 0, false);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002164 File Offset: 0x00000364
		public override void Tick()
		{
			bool flag = this.tickCounter == 0;
			if (flag)
			{
				this.PerformPreFiringTreatment();
			}
			bool flag2 = this.tickCounter < this.Props.TickOffset;
			if (flag2)
			{
				this.GetPreFiringDrawingParameters();
			}
			else
			{
				bool flag3 = this.tickCounter == this.Props.TickOffset;
				if (flag3)
				{
					this.Fire();
				}
				this.GetPostFiringDrawingParameters();
			}
			bool flag4 = this.tickCounter == this.Props.TickOffset + this.Props.TickOffsetSecond;
			if (flag4)
			{
				base.Tick();
				this.Destroy(DestroyMode.Vanish);
			}
			Thing launcher = this.launcher;
			Pawn pawn = launcher as Pawn;
			Pawn_StanceTracker stances = new Pawn_StanceTracker(pawn);
			bool flag5;
			if (pawn != null)
			{
				stances = pawn.stances;
				flag5 = (stances != null);
			}
			else
			{
				flag5 = false;
			}
			bool flag6 = flag5;
			if (flag6)
			{
				Stance curStance = stances.curStance;
				bool flag7 = !(curStance is Stance_Busy) || pawn.Dead;
				if (flag7)
				{
					this.Destroy(DestroyMode.Vanish);
				}
			}
			this.tickCounter++;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000226C File Offset: 0x0000046C
		public virtual void PerformPreFiringTreatment()
		{
			this.DetermineImpactExactPosition();
			Vector3 vector = (this.destination - this.origin).normalized * 0.9f;
			this.drawingScale = new Vector3(1f, 1f, (this.destination - this.origin).magnitude - vector.magnitude);
			this.drawingPosition = this.origin + vector / 2f + (this.destination - this.origin) / 2f + Vector3.up * this.def.Altitude;
			this.drawingMatrix.SetTRS(this.drawingPosition, this.ExactRotation, this.drawingScale);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002350 File Offset: 0x00000550
		public virtual void GetPreFiringDrawingParameters()
		{
			bool flag = this.Props.TickOffset != 0;
			if (flag)
			{
				this.drawingIntensity = this.Props.DrawingOffset + (this.Props.DrawingOffsetSecond - this.Props.DrawingOffset) * (float)this.tickCounter / (float)this.Props.TickOffset;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023B0 File Offset: 0x000005B0
		public virtual void GetPostFiringDrawingParameters()
		{
			bool flag = this.Props.TickOffsetSecond != 0;
			if (flag)
			{
				this.drawingIntensity = this.Props.DrawingOffsetThird + (this.Props.DrawingOffsetFourth - this.Props.DrawingOffsetThird) * (((float)this.tickCounter - (float)this.Props.TickOffset) / (float)this.Props.TickOffsetSecond);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002420 File Offset: 0x00000620
		protected void DetermineImpactExactPosition()
		{
			Vector3 vector = this.destination - this.origin;
			int num = (int)vector.magnitude;
			Vector3 vector2 = vector / vector.magnitude;
			Vector3 destination = this.origin;
			Vector3 vector3 = this.origin;
			IntVec3 intVec = IntVec3Utility.ToIntVec3(vector3);
			for (int i = 1; i <= num; i++)
			{
				vector3 += vector2;
				intVec = IntVec3Utility.ToIntVec3(vector3);
				bool flag = !GenGrid.InBounds(vector3, base.Map);
				if (flag)
				{
					this.destination = destination;
					break;
				}
				bool flag2 = !this.def.projectile.flyOverhead && this.def.projectile.alwaysFreeIntercept && i >= 5;
				if (flag2)
				{
					List<Thing> list = base.Map.thingGrid.ThingsListAt(base.Position);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						bool flag3 = thing.def.Fillage == FillCategory.Full;
						if (flag3)
						{
							this.destination = intVec.ToVector3Shifted() + new Vector3(Rand.Range(-0.3f, 0.3f), 0f, Rand.Range(-0.3f, 0.3f));
							this.hitThing = thing;
							break;
						}
						bool flag4 = thing.def.category == ThingCategory.Pawn;
						if (flag4)
						{
							Pawn pawn = thing as Pawn;
							float num2 = 0.45f;
							bool downed = pawn.Downed;
							if (downed)
							{
								num2 *= 0.1f;
							}
							float num3 = GenGeo.MagnitudeHorizontal(this.ExactPosition - this.origin);
							bool flag5 = num3 < 4f;
							if (flag5)
							{
								num2 *= 0f;
							}
							else
							{
								bool flag6 = num3 < 7f;
								if (flag6)
								{
									num2 *= 0.5f;
								}
								else
								{
									bool flag7 = num3 < 10f;
									if (flag7)
									{
										num2 *= 0.75f;
									}
								}
							}
							num2 *= pawn.RaceProps.baseBodySize;
							bool flag8 = Rand.Value < num2;
							if (flag8)
							{
								this.destination = intVec.ToVector3Shifted() + new Vector3(Rand.Range(-0.3f, 0.3f), 0f, Rand.Range(-0.3f, 0.3f));
								this.hitThing = pawn;
								break;
							}
						}
					}
				}
				destination = vector3;
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000026C0 File Offset: 0x000008C0
		public virtual void Fire()
		{
			this.ApplyDamage(this.hitThing);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000026D0 File Offset: 0x000008D0
		protected void ApplyDamage(Thing hitThing)
		{
			bool flag = hitThing != null;
			if (flag)
			{
				this.Impact(hitThing);
			}
			else
			{
				this.ImpactSomething();
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000026FC File Offset: 0x000008FC
		private void ImpactSomething()
		{
			bool flyOverhead = this.def.projectile.flyOverhead;
			if (flyOverhead)
			{
				RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
				bool flag = roofDef != null;
				if (flag)
				{
					bool isThickRoof = roofDef.isThickRoof;
					if (isThickRoof)
					{
						this.def.projectile.soundHitThickRoof.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
						this.Destroy(DestroyMode.Vanish);
						return;
					}
					bool flag2 = base.Position.GetEdifice(base.Map) == null || base.Position.GetEdifice(base.Map).def.Fillage != FillCategory.Full;
					if (flag2)
					{
						RoofCollapserImmediate.DropRoofInCells(base.Position, base.Map, null);
					}
				}
			}
			bool flag3 = !this.usedTarget.HasThing || !base.CanHit(this.usedTarget.Thing);
			if (flag3)
			{
				List<Thing> list = new List<Thing>();
				list.Clear();
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					bool flag4 = (thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant) && base.CanHit(thing);
					if (flag4)
					{
						list.Add(thing);
					}
				}
				list.Shuffle<Thing>();
				for (int j = 0; j < list.Count; j++)
				{
					Thing thing2 = list[j];
					Pawn pawn = thing2 as Pawn;
					bool flag5 = pawn != null;
					float num;
					if (flag5)
					{
						num = 0.5f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
						bool flag6 = pawn.GetPosture() != PawnPosture.Standing && GenGeo.MagnitudeHorizontalSquared(this.origin - this.destination) >= 20.25f;
						if (flag6)
						{
							num *= 0.2f;
						}
						bool flag7 = this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction);
						if (flag7)
						{
							num *= VerbUtility.InterceptChanceFactorFromDistance(this.origin, base.Position);
						}
					}
					else
					{
						num = 1.5f * thing2.def.fillPercent;
					}
					bool flag8 = Rand.Chance(num);
					if (flag8)
					{
						this.Impact(list.RandomElement<Thing>());
						return;
					}
				}
				this.Impact(null);
			}
			else
			{
				Pawn pawn2 = this.usedTarget.Thing as Pawn;
				bool flag9 = pawn2 != null && pawn2.GetPosture() != PawnPosture.Standing && GenGeo.MagnitudeHorizontalSquared(this.origin - this.destination) >= 20.25f && !Rand.Chance(0.2f);
				if (flag9)
				{
					this.Impact(null);
				}
				else
				{
					this.Impact(this.usedTarget.Thing);
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002A68 File Offset: 0x00000C68
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			bool flag = hitThing != null;
			if (flag)
			{
				int damageAmount = this.def.projectile.GetDamageAmount((float)base.DamageAmount, null);
				ThingDef equipmentDef = this.equipmentDef;
				float armorPenetration = this.def.projectile.GetArmorPenetration(base.ArmorPenetration, null);
				DamageInfo dinfo = new DamageInfo(this.def.projectile.damageDef, (float)damageAmount, armorPenetration, this.ExactRotation.eulerAngles.y, this.launcher, null, equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, null);
				hitThing.TakeDamage(dinfo);
				Pawn pawn = hitThing as Pawn;
				bool flag2 = pawn != null && !pawn.Downed && Rand.Value < this.compED.chanceToProc;
				if (flag2)
				{
					MoteMaker.ThrowMicroSparks(this.destination, base.Map);
					hitThing.TakeDamage(new DamageInfo(DefDatabase<DamageDef>.GetNamed(this.compED.damageDef, true), (float)this.compED.damageAmount, armorPenetration, this.ExactRotation.eulerAngles.y, this.launcher, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}
			}
			else
			{
				SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(base.Position, map, false));
				MoteMaker.MakeStaticMote(this.ExactPosition, map, ThingDefOf.LaserMoteWorker, 0.5f);
				Projectile_BaseEnergyImpact.ThrowMicroSparksRed(this.ExactPosition, base.Map);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002BE4 File Offset: 0x00000DE4
		public override void Draw()
		{
			base.Comps_PostDraw();
			Graphics.DrawMesh(MeshPool.plane10, this.drawingMatrix, FadedMaterialPool.FadedVersionOf(this.drawingTexture, this.drawingIntensity), 0);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002C14 File Offset: 0x00000E14
		public static void ThrowMicroSparksRed(Vector3 loc, Map map)
		{
			bool flag = !GenView.ShouldSpawnMotesAt(loc, map) || map.moteCounter.SaturatedLowPriority;
			if (!flag)
			{
				Rand.PushState();
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDef.Named("LaserImpactThing"), null);
				moteThrown.Scale = Rand.Range(1f, 1.2f);
				moteThrown.rotationRate = Rand.Range(-12f, 12f);
				moteThrown.exactPosition = loc;
				moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
				moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);
				moteThrown.SetVelocity((float)Rand.Range(35, 45), 1.2f);
				GenSpawn.Spawn(moteThrown, IntVec3Utility.ToIntVec3(loc), map, WipeMode.Vanish);
				Rand.PopState();
			}
		}

		// Token: 0x04000009 RID: 9
		public int tickCounter = 0;

		// Token: 0x0400000A RID: 10
		public Thing hitThing = null;

		// Token: 0x0400000B RID: 11
		public CompInitializeDamage compED;

		// Token: 0x0400000C RID: 12
		public Material preFiringTexture;

		// Token: 0x0400000D RID: 13
		public Material postFiringTexture;

		// Token: 0x0400000E RID: 14
		public Matrix4x4 drawingMatrix = default(Matrix4x4);

		// Token: 0x0400000F RID: 15
		public Vector3 drawingScale;

		// Token: 0x04000010 RID: 16
		public Vector3 drawingPosition;

		// Token: 0x04000011 RID: 17
		public float drawingIntensity = 0f;

		// Token: 0x04000012 RID: 18
		public Material drawingTexture;
	}
}
