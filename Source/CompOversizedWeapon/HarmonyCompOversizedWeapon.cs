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
            harmonyInstance.Patch(typeof(PawnRenderer).GetMethod("DrawEquipmentAiming"),
                new HarmonyMethod(typeof(HarmonyCompOversizedWeapon).GetMethod("DrawEquipmentAimingPreFix")));
            harmonyInstance.Patch(AccessTools.Method(typeof(Thing), "get_DefaultGraphic"), null,
                new HarmonyMethod(typeof(HarmonyCompOversizedWeapon), "get_Graphic_PostFix"));
        }

        // Token: 0x0600000A RID: 10 RVA: 0x000022B0 File Offset: 0x000004B0
        public static bool DrawEquipmentAimingPreFix(PawnRenderer __instance, Thing eq, Vector3 drawLoc, float aimAngle)
        {
            if (eq is not ThingWithComps thingWithComps)
            {
                return true;
            }

            var thingComp = thingWithComps.AllComps.FirstOrDefault(y =>
                y.GetType().ToString() == "CompDeflector.CompDeflector" ||
                y.GetType().BaseType?.ToString() == "CompDeflector.CompDeflector");
            if (thingComp != null)
            {
                var value = Traverse.Create(thingComp).Property("IsAnimatingNow").GetValue<bool>();
                if (value)
                {
                    return false;
                }
            }

            var compOversizedWeapon = thingWithComps.TryGetComp<CompOversizedWeapon>();
            if (compOversizedWeapon == null)
            {
                return true;
            }

            var b = false;
            var num = aimAngle - 90f;
            var value2 = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();
            if (value2 == null)
            {
                return true;
            }

            if (aimAngle > 20f && aimAngle < 160f)
            {
                num += eq.def.equippedAngleOffset;
            }
            else
            {
                if (aimAngle > 200f && aimAngle < 340f)
                {
                    b = true;
                    num -= 180f;
                    num -= eq.def.equippedAngleOffset;
                }
                else
                {
                    num = AdjustOffsetAtPeace(eq, value2, compOversizedWeapon, num);
                }
            }

            if (compOversizedWeapon.Props != null && !value2.IsFighting() &&
                compOversizedWeapon.Props.verticalFlipNorth && value2.Rotation == Rot4.North)
            {
                num += 180f;
            }

            if (!value2.IsFighting())
            {
                num = AdjustNonCombatRotation(value2, num, compOversizedWeapon);
            }

            num %= 360f;
            Material matSingle;
            if (eq.Graphic is Graphic_StackCount graphic_StackCount)
            {
                matSingle = graphic_StackCount.SubGraphicForStackCount(1, eq.def).MatSingle;
            }
            else
            {
                matSingle = eq.Graphic.MatSingle;
            }

            var vector = new Vector3(eq.def.graphicData.drawSize.x, 1f, eq.def.graphicData.drawSize.y);
            var matrix4x = default(Matrix4x4);
            var vector2 = AdjustRenderOffsetFromDir(value2, compOversizedWeapon);
            matrix4x.SetTRS(drawLoc + vector2, Quaternion.AngleAxis(num, Vector3.up), vector);
            Graphics.DrawMesh(!b ? MeshPool.plane10 : MeshPool.plane10Flip, matrix4x, matSingle, 0);
            if (compOversizedWeapon.Props == null || !compOversizedWeapon.Props.isDualWeapon)
            {
                return false;
            }

            vector2 = new Vector3(-1f * vector2.x, vector2.y, vector2.z);
            Mesh mesh2;
            if (value2.Rotation == Rot4.North || value2.Rotation == Rot4.South)
            {
                num += 135f;
                num %= 360f;
                mesh2 = !b ? MeshPool.plane10Flip : MeshPool.plane10;
            }
            else
            {
                vector2 = new Vector3(vector2.x, vector2.y - 0.1f, vector2.z + 0.15f);
                mesh2 = !b ? MeshPool.plane10 : MeshPool.plane10Flip;
            }

            matrix4x.SetTRS(drawLoc + vector2, Quaternion.AngleAxis(num, Vector3.up), vector);
            Graphics.DrawMesh(mesh2, matrix4x, matSingle, 0);

            return false;
        }

        // Token: 0x0600000B RID: 11 RVA: 0x0000263C File Offset: 0x0000083C
        private static float AdjustOffsetAtPeace(Thing eq, Pawn pawn, CompOversizedWeapon compOversizedWeapon,
            float num)
        {
            var num2 = eq.def.equippedAngleOffset;
            if (compOversizedWeapon.Props != null && !pawn.IsFighting() &&
                compOversizedWeapon.Props.verticalFlipOutsideCombat)
            {
                num2 += 180f;
            }

            num += num2;
            return num;
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002698 File Offset: 0x00000898
        private static float AdjustNonCombatRotation(Pawn pawn, float num, CompOversizedWeapon compOversizedWeapon)
        {
            if (compOversizedWeapon.Props == null)
            {
                return num;
            }

            if (pawn.Rotation == Rot4.North)
            {
                num += compOversizedWeapon.Props.angleAdjustmentNorth;
            }
            else
            {
                if (pawn.Rotation == Rot4.East)
                {
                    num += compOversizedWeapon.Props.angleAdjustmentEast;
                }
                else
                {
                    if (pawn.Rotation == Rot4.West)
                    {
                        num += compOversizedWeapon.Props.angleAdjustmentWest;
                    }
                    else
                    {
                        if (pawn.Rotation == Rot4.South)
                        {
                            num += compOversizedWeapon.Props.angleAdjustmentSouth;
                        }
                    }
                }
            }

            return num;
        }

        // Token: 0x0600000D RID: 13 RVA: 0x0000275C File Offset: 0x0000095C
        private static Vector3 AdjustRenderOffsetFromDir(Pawn pawn, CompOversizedWeapon compOversizedWeapon)
        {
            var rotation = pawn.Rotation;
            var result = Vector3.zero;
            if (compOversizedWeapon.Props == null)
            {
                return result;
            }

            result = compOversizedWeapon.Props.northOffset;
            if (rotation == Rot4.East)
            {
                result = compOversizedWeapon.Props.eastOffset;
            }
            else
            {
                if (rotation == Rot4.South)
                {
                    result = compOversizedWeapon.Props.southOffset;
                }
                else
                {
                    if (rotation == Rot4.West)
                    {
                        result = compOversizedWeapon.Props.westOffset;
                    }
                }
            }

            return result;
        }

        // Token: 0x0600000E RID: 14 RVA: 0x000027F8 File Offset: 0x000009F8
        public static void get_Graphic_PostFix(Thing __instance, ref Graphic __result)
        {
            var value = Traverse.Create(__instance).Field("graphicInt").GetValue<Graphic>();
            if (value == null)
            {
                return;
            }

            if (!(__instance is ThingWithComps thingWithComps))
            {
                return;
            }

            if (thingWithComps.ParentHolder is Pawn)
            {
                return;
            }

            var thingComp = thingWithComps.AllComps.FirstOrDefault(y =>
                y.GetType().ToString().Contains("ActivatableEffect"));
            if (thingComp != null)
            {
                var value2 = Traverse.Create(thingComp).Property("GetPawn").GetValue<Pawn>();
                if (value2 != null)
                {
                    return;
                }
            }

            var compOversizedWeapon = thingWithComps.TryGetComp<CompOversizedWeapon>();
            if (compOversizedWeapon == null)
            {
                return;
            }

            var props = compOversizedWeapon.Props;
            if (props?.groundGraphic == null)
            {
                value.drawSize = __instance.def.graphicData.drawSize;
                __result = value;
            }
            else
            {
                var isEquipped = compOversizedWeapon.IsEquipped;
                if (isEquipped)
                {
                    value.drawSize = __instance.def.graphicData.drawSize;
                    __result = value;
                }
                else
                {
                    var props2 = compOversizedWeapon.Props;
                    Graphic graphic;
                    if (props2 == null)
                    {
                        graphic = null;
                    }
                    else
                    {
                        var groundGraphic = props2.groundGraphic;
                        graphic = groundGraphic?.GraphicColoredFor(__instance);
                    }

                    var graphic2 = graphic;
                    if (graphic2 != null)
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