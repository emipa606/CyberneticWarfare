using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace CompOversizedWeapon
{
	// Token: 0x02000004 RID: 4
	[StaticConstructorOnStartup]
	internal static class HarmonyCompOversizedWeapon
	{
		// Token: 0x06000009 RID: 9 RVA: 0x0000222C File Offset: 0x0000042C
		static HarmonyCompOversizedWeapon()
		{
			var harmonyInstance = new Harmony("rimworld.jecrell.comps.oversized");
			harmonyInstance.Patch(typeof(PawnRenderer).GetMethod("DrawEquipmentAiming"), new HarmonyMethod(typeof(HarmonyCompOversizedWeapon).GetMethod("DrawEquipmentAimingPreFix")), null, null);
			harmonyInstance.Patch(AccessTools.Method(typeof(Thing), "get_DefaultGraphic", null, null), null, new HarmonyMethod(typeof(HarmonyCompOversizedWeapon), "get_Graphic_PostFix", null), null);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000022B0 File Offset: 0x000004B0
		public static bool DrawEquipmentAimingPreFix(PawnRenderer __instance, Thing eq, Vector3 drawLoc, float aimAngle)
		{
			ThingWithComps thingWithComps = eq as ThingWithComps;
			bool flag = thingWithComps != null;
			if (flag)
			{
				ThingComp thingComp = thingWithComps.AllComps.FirstOrDefault((ThingComp y) => y.GetType().ToString() == "CompDeflector.CompDeflector" || y.GetType().BaseType.ToString() == "CompDeflector.CompDeflector");
				bool flag2 = thingComp != null;
				if (flag2)
				{
					bool value = Traverse.Create(thingComp).Property("IsAnimatingNow", null).GetValue<bool>();
					bool flag3 = value;
					if (flag3)
					{
						return false;
					}
				}
				CompOversizedWeapon compOversizedWeapon = thingWithComps.TryGetComp<CompOversizedWeapon>();
				bool flag4 = compOversizedWeapon != null;
				if (flag4)
				{
					bool flag5 = false;
					float num = aimAngle - 90f;
					Pawn value2 = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();
					bool flag6 = value2 == null;
					if (flag6)
					{
						return true;
					}
					bool flag7 = aimAngle > 20f && aimAngle < 160f;
					if (flag7)
					{
						Mesh mesh = MeshPool.plane10;
						num += eq.def.equippedAngleOffset;
					}
					else
					{
						bool flag8 = aimAngle > 200f && aimAngle < 340f;
						if (flag8)
						{
							Mesh mesh = MeshPool.plane10Flip;
							flag5 = true;
							num -= 180f;
							num -= eq.def.equippedAngleOffset;
						}
						else
						{
							num = HarmonyCompOversizedWeapon.AdjustOffsetAtPeace(eq, value2, compOversizedWeapon, num);
						}
					}
					bool flag9 = compOversizedWeapon.Props != null && !value2.IsFighting() && compOversizedWeapon.Props.verticalFlipNorth && value2.Rotation == Rot4.North;
					if (flag9)
					{
						num += 180f;
					}
					bool flag10 = !value2.IsFighting();
					if (flag10)
					{
						num = HarmonyCompOversizedWeapon.AdjustNonCombatRotation(value2, num, compOversizedWeapon);
					}
					num %= 360f;
					Graphic_StackCount graphic_StackCount = eq.Graphic as Graphic_StackCount;
					bool flag11 = graphic_StackCount != null;
					Material matSingle;
					if (flag11)
					{
						matSingle = graphic_StackCount.SubGraphicForStackCount(1, eq.def).MatSingle;
					}
					else
					{
						matSingle = eq.Graphic.MatSingle;
					}
					Vector3 vector = new Vector3(eq.def.graphicData.drawSize.x, 1f, eq.def.graphicData.drawSize.y);
					Matrix4x4 matrix4x = default(Matrix4x4);
					Vector3 vector2 = HarmonyCompOversizedWeapon.AdjustRenderOffsetFromDir(value2, compOversizedWeapon);
					matrix4x.SetTRS(drawLoc + vector2, Quaternion.AngleAxis(num, Vector3.up), vector);
					Graphics.DrawMesh((!flag5) ? MeshPool.plane10 : MeshPool.plane10Flip, matrix4x, matSingle, 0);
					bool flag12 = compOversizedWeapon.Props != null && compOversizedWeapon.Props.isDualWeapon;
					if (flag12)
					{
						vector2 = new Vector3(-1f * vector2.x, vector2.y, vector2.z);
						bool flag13 = value2.Rotation == Rot4.North || value2.Rotation == Rot4.South;
						Mesh mesh2;
						if (flag13)
						{
							num += 135f;
							num %= 360f;
							mesh2 = ((!flag5) ? MeshPool.plane10Flip : MeshPool.plane10);
						}
						else
						{
							vector2 = new Vector3(vector2.x, vector2.y - 0.1f, vector2.z + 0.15f);
							mesh2 = ((!flag5) ? MeshPool.plane10 : MeshPool.plane10Flip);
						}
						matrix4x.SetTRS(drawLoc + vector2, Quaternion.AngleAxis(num, Vector3.up), vector);
						Graphics.DrawMesh(mesh2, matrix4x, matSingle, 0);
					}
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000263C File Offset: 0x0000083C
		private static float AdjustOffsetAtPeace(Thing eq, Pawn pawn, CompOversizedWeapon compOversizedWeapon, float num)
		{
			Mesh plane = MeshPool.plane10;
			float num2 = eq.def.equippedAngleOffset;
			bool flag = compOversizedWeapon.Props != null && !pawn.IsFighting() && compOversizedWeapon.Props.verticalFlipOutsideCombat;
			if (flag)
			{
				num2 += 180f;
			}
			num += num2;
			return num;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002698 File Offset: 0x00000898
		private static float AdjustNonCombatRotation(Pawn pawn, float num, CompOversizedWeapon compOversizedWeapon)
		{
			bool flag = compOversizedWeapon.Props != null;
			if (flag)
			{
				bool flag2 = pawn.Rotation == Rot4.North;
				if (flag2)
				{
					num += compOversizedWeapon.Props.angleAdjustmentNorth;
				}
				else
				{
					bool flag3 = pawn.Rotation == Rot4.East;
					if (flag3)
					{
						num += compOversizedWeapon.Props.angleAdjustmentEast;
					}
					else
					{
						bool flag4 = pawn.Rotation == Rot4.West;
						if (flag4)
						{
							num += compOversizedWeapon.Props.angleAdjustmentWest;
						}
						else
						{
							bool flag5 = pawn.Rotation == Rot4.South;
							if (flag5)
							{
								num += compOversizedWeapon.Props.angleAdjustmentSouth;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000275C File Offset: 0x0000095C
		private static Vector3 AdjustRenderOffsetFromDir(Pawn pawn, CompOversizedWeapon compOversizedWeapon)
		{
			Rot4 rotation = pawn.Rotation;
			Vector3 result = Vector3.zero;
			bool flag = compOversizedWeapon.Props != null;
			if (flag)
			{
				result = compOversizedWeapon.Props.northOffset;
				bool flag2 = rotation == Rot4.East;
				if (flag2)
				{
					result = compOversizedWeapon.Props.eastOffset;
				}
				else
				{
					bool flag3 = rotation == Rot4.South;
					if (flag3)
					{
						result = compOversizedWeapon.Props.southOffset;
					}
					else
					{
						bool flag4 = rotation == Rot4.West;
						if (flag4)
						{
							result = compOversizedWeapon.Props.westOffset;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000027F8 File Offset: 0x000009F8
		public static void get_Graphic_PostFix(Thing __instance, ref Graphic __result)
		{
			Graphic value = Traverse.Create(__instance).Field("graphicInt").GetValue<Graphic>();
			bool flag = value != null;
			if (flag)
			{
				ThingWithComps thingWithComps = __instance as ThingWithComps;
				bool flag2 = thingWithComps != null;
				if (flag2)
				{
					bool flag3 = thingWithComps.ParentHolder is Pawn;
					if (!flag3)
					{
						ThingComp thingComp = thingWithComps.AllComps.FirstOrDefault((ThingComp y) => y.GetType().ToString().Contains("ActivatableEffect"));
						bool flag4 = thingComp != null;
						if (flag4)
						{
							Pawn value2 = Traverse.Create(thingComp).Property("GetPawn", null).GetValue<Pawn>();
							bool flag5 = value2 != null;
							if (flag5)
							{
								return;
							}
						}
						CompOversizedWeapon compOversizedWeapon = thingWithComps.TryGetComp<CompOversizedWeapon>();
						bool flag6 = compOversizedWeapon != null;
						if (flag6)
						{
							CompProperties_OversizedWeapon props = compOversizedWeapon.Props;
							bool flag7 = ((props != null) ? props.groundGraphic : null) == null;
							if (flag7)
							{
								value.drawSize = __instance.def.graphicData.drawSize;
								__result = value;
							}
							else
							{
								bool isEquipped = compOversizedWeapon.IsEquipped;
								if (isEquipped)
								{
									value.drawSize = __instance.def.graphicData.drawSize;
									__result = value;
								}
								else
								{
									CompProperties_OversizedWeapon props2 = compOversizedWeapon.Props;
									Graphic graphic;
									if (props2 == null)
									{
										graphic = null;
									}
									else
									{
										GraphicData groundGraphic = props2.groundGraphic;
										graphic = ((groundGraphic != null) ? groundGraphic.GraphicColoredFor(__instance) : null);
									}
									Graphic graphic2 = graphic;
									bool flag8 = graphic2 != null;
									if (flag8)
									{
										graphic2.drawSize = compOversizedWeapon.Props.groundGraphic.drawSize;
										__result = graphic2;
									}
									else
									{
										value.drawSize = __instance.def.graphicData.drawSize;
										__result = value;
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
